using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using Infragistics.Web.Mvc;
using System.Collections.Generic;
using System;
using jsreport.Client;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Web;
using System.Security.Claims;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class EmailMessagesController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new EmailMessagesService();
            
            var model = await srv.GetMessages();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> SendTest(string _id)
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var usrSrv = new UserServices();
            var usrModel = await usrSrv.GetUser(UserId); 
            var srv = new EmailMessagesService();
            var modelOut = await srv.GetMessageById(_id);

            var emailSrv = new EmailService();

            try
            {
                var msgSrv = new EmailMessagesService();
                var messageModel = await msgSrv.GetMessageByMessageId(modelOut.MessageID);

                string subject = messageModel.Title;
                string body = messageModel.HTML.Replace("[USER]", usrModel.FirstName).Replace("[DATATYPE]","GERMAN").Replace("[DATESTRING]",DateTime.Now.ToString("MMMM yyy"));
                await emailSrv.SendEmailTo(UserId, subject, body);

            }
            catch (Exception e)
            {
                var eSrv = new ErrorService();
                var eModel = new ErrorModel { Code = "91", Class = "UpsertController Code 91", ErrorMessage = e.Message };
                await eSrv.UpdateError(eModel);
            }

            return RedirectToAction("Index", "EmailMessages");

        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new EmailMessagesService();
            var modelOut = await srv.GetMessageById(_id);

            return View(modelOut);

        }

        [Authorize]
        public IActionResult Add()
        {
    
            var modelOut = new EmailMessagesModel
            {
               HTML = string.Empty,
               Title = string.Empty
            };

            return View(modelOut);
        }
        
        [Authorize]
        public async Task<IActionResult> AddNew(EmailMessagesModel model)
        {
            var srv = new EmailMessagesService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Port Details";
                var modelOut = new EmailMessagesModel
                {
                    HTML = string.Empty,
                    Title = string.Empty
                };
                return View("Add", modelOut);
            }

            await srv.Add(model);
            return RedirectToAction("Index", "EmailMessages");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Save(EmailMessagesModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new EmailMessagesService();
            model.HTML = HttpUtility.UrlDecode(model.HTML);
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.EmailMessagesModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Email Messages Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "EmailMessages");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new EmailMessagesService();
            var modelOut = await srv.GetMessageById(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.EmailMessagesModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Email Messages Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "EmailMessages");

        }
    }
}
