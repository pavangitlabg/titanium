using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Data.Models;
using System.Security.Claims;
using Data.Enums;
using System;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class ImportFilesController : Controller
    {
        
        [Authorize]
        public async Task<IActionResult> Index()
        {

            var mailSrv = new EmailService();
            var mailModel = await mailSrv.GetEmailCredentials();
            var srv = new FileService();
            var treeModel = await srv.GetFilesFromSystem(mailModel.FilePath);

            IQueryable treeData = treeModel.AsQueryable();
            return View(treeData);

        }

   
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFiles(string DataArray)
        {

            var srv = new FileService();
            var bResult = await srv.RemoveFiles(DataArray);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.EmailCredentialsModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Import Files Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = DataArray
            };
            new EventLog(EventModel);

            return Json(new { success = bResult });
        }

    }
}

