using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;
using Data.Models;
using ISSB.Helpers;
using ISSB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{
    public class DataImportController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var srv = DataImportService.Instance;
            var model = await srv.GetImportedFiles();
            IQueryable data = model.AsQueryable();

            return View(data);
         
        }

        [Authorize]
        public ActionResult Upload()
        {
           
            var modelOut = new DataFileUploadModel
            {
                 Date = DateTime.Now,
                 Source= "UPLOAD",
                 Subject = string.Empty,
                 Message = string.Empty
                 
            };

            return View(modelOut);
        }

        [Authorize]
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Save(DataFileUploadModel model)
        {
            //save file to disk
            var importSrv = DataImportService.Instance;

            var alreadyEntered = await importSrv.DoesExitst(model.File.FileName);

            if (alreadyEntered.FileName.Equals(model.File.FileName))
            {
                ViewData["Message"] = alreadyEntered.FileName + " is already on the system";
                return View("Upload", model);
            }

            if (!model.File.FileName.Contains(".zip"))
            {
                ViewData["Message"] = "Only ZIP files can be uploaded";
                return View("Upload", model);
            }

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid File Name";
                return View("Upload", model);
            }

            var mailSrv = new EmailService();
            var mailModel = await mailSrv.GetEmailCredentials();
            var controlSrv = new SystemControlService();
            var cntl = await controlSrv.GetSystemControl();
#if RELEASE
            if (model.File.FileName.Contains(".zip"))
            {
                string filePath = mailModel.FilePath + cntl.LastFileNumber.ToString() + "-" + model.File.FileName.Replace(".zip",".uploading");
                await model.File.CopyToAsync(new FileStream(filePath, FileMode.Create));
            }
#endif

#if DEBUG
            if (model.File.FileName.Contains(".zip"))
            {
                string filePath = "/Users/charlesjardine/Projects/ISSB/ISSB/wwwroot/reports/" + cntl.LastFileNumber.ToString() + "-" + model.File.FileName.Replace(".zip", ".uploading");
                await model.File.CopyToAsync(new FileStream(filePath, FileMode.Create));
            }
#endif
            //ToDo File Location
            var importModel = new DataFileImportModel
            {
                Idx = cntl.LastFileNumber++,
                Date = DateTime.Now,
                FileLocation = mailModel.FilePath,
                FileName = model.File.FileName,
                Source = "UPLOAD",
                FileStatus = "Uploaded",
                Message = "",
                Subject = model.Subject,

            };
            
            
            await importSrv.SaveImportFile(importModel);
            cntl.LastFileNumber = cntl.LastFileNumber + 1;
            await controlSrv.UpdateControl(cntl);

            //var newPath = filePath.Replace(".uploading", ".zip");
            //System.IO.File.Move(filePath,newPath);        

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.DataFileUploadModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Data Import Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "DataImport");
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var importSrv = DataImportService.Instance;
            var modelOut = await importSrv.GetHeaderById(_id);

            return View(modelOut);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveItem(DataFileImportModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var importSrv = DataImportService.Instance;
            var modelOut = await importSrv.GetHeaderById(model._id);
            modelOut.Subject = model.Subject;
            modelOut.Message = model.Message;

            await importSrv.Update(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.DataFileUploadModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Data Import Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "DataImport");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = DataImportService.Instance;

            await srv.Delete(_id);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.DataFileUploadModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Data Import Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = _id
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "DataImport");

        }
    }
}
