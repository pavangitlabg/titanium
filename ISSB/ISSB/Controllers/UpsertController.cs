using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System;
using System.Linq;
using System.Security.Claims;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class UpsertController : Controller
    {


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new UpsertService();
            var model = await srv.GetHeaderRecords(false);

            //update Header Status

            //var modelUpdate = await srv.GetHeaderRecords(true);
            //var geoSrv = new SourceCountryService();

            //foreach (var Item in modelUpdate)
            //{
            //    var PendingModel = await srv.GetPendingRecordsTopOne(Item.BATCH_NO);
            //    var CountryModel = await geoSrv.GetSourceCountryByGeoCode(PendingModel[0].SC_GEO);
            //    var CountryName = CountryModel.NAME;
            //    var DateString = new DateTime(PendingModel[0].YEAR, PendingModel[0].MONTH, 1).ToString("MMMM yyy");
            //    Item.STATUS = "Batch: " + Item.BATCH_NO + " Data For: " + PendingModel[0].SC_GEO + " " + CountryName + " " + DateString;
            //    await srv.Save(Item);
            //}

            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Remove()
        {
            var srv = new UpsertService();
            var model = await srv.GetHeaderRecords(true);
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Howler(string _id)
        {
            var srv = new UpsertService();
            var model = await srv.GetHeaderById(_id);
            var dataRecs = await srv.GetHowlerRecords(model.BATCH_NO);
            IQueryable data = dataRecs.AsQueryable();
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new UpsertService();
            var modelOut = await srv.GetHeaderById(_id);
            await srv.Delete(modelOut);

            return RedirectToAction("Index", "Upsert");

        }


        [Authorize]
        public async Task<IActionResult> UpsertData(string _id)
        {
            var srv = new UpsertService();
            var modelOut = await srv.GetHeaderById(_id);


            if (modelOut.IMPORT_TYPE.Equals("UK"))
            {

                srv.PostAmendments(modelOut.BATCH_NO , "006");
            }


            var bResult = await srv.UpsertToProduction(modelOut.BATCH_NO);
            if (bResult)
            {
                modelOut.POSTED = true;

                //send emails to notifications

                var geoSrv = new SourceCountryService();
                var PendingModel = await srv.GetPendingRecordsTopOne(modelOut.BATCH_NO);
                var CountryModel = await geoSrv.GetSourceCountryByGeoCode(PendingModel[0].SC_GEO);
                var CountryName = CountryModel.NAME;
                var userSrv = new UserServices();
                var userList = await userSrv.GetUsers();
                var msgSrv = new EmailMessagesService();
                var messageModel = await msgSrv.GetMessageByMessageId(1);
                var DateString = new DateTime(PendingModel[0].YEAR, PendingModel[0].MONTH, 1).ToString("MMMM yyy");

                modelOut.STATUS = "Data For: " + PendingModel[0].SC_GEO + " " + CountryName + " " + DateString;

                await srv.Save(modelOut);

                var emailSrv = new EmailService();
                foreach (var usr in userList)
                {
                    try
                    {
                        if (usr.EmailNotification)
                        {
                            try
                            {

                                string subject = messageModel.Title;
                                string body = messageModel.HTML.Replace("[USER]", usr.FirstName).Replace("[DATATYPE]", CountryName).Replace("[DATESTRING]", DateString);
                                await emailSrv.SendEmailTo(usr.Email, subject, body);

                            }
                            catch (Exception e)
                            {
                                var eSrv = new ErrorService();
                                var eModel = new ErrorModel { Code = "91", Class = "UpsertController Code 91", ErrorMessage = e.Message };
                                await eSrv.UpdateError(eModel);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var eSrv = new ErrorService();
                        var eModel = new ErrorModel { Code = "91", Class = "UpsertController Code 91", ErrorMessage = e.Message };
                        await eSrv.UpdateError(eModel);
                    }
                }

            }
            else
            {
                modelOut.POSTED = false;
                modelOut.STATUS = "[Already on system] " + modelOut.STATUS;
                await srv.Save(modelOut);
            }

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.PendingImportHeaderModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Upsert Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Upsert");
        }

        [Authorize]
        public async Task<IActionResult> DeleteHowler(string _id)
        {
            var srv = new UpsertService();
            var modelOut = await srv.GetMainById(_id);
            await srv.DeleteHowler(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.PendingImportHeaderModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Upsert Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };

            return RedirectToAction("Index", "Upsert");

        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new UpsertService();
            var modelOut = await srv.GetHeaderById(_id);

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> HowlerEdit(string _id)
        {
            var srv = new UpsertService();
            var modelOut = await srv.GetMainById(_id);

            return View(modelOut);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(PendingImportHeaderModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new UpsertService();
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.PendingImportHeaderModel,
                MESSAGE = "Upsert Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Upsert");

        }

        [Authorize]
        public async Task<IActionResult> RemoveProd(string _id)
        {

            var srv = new UpsertService();
            var modelOut = await srv.GetHeaderById(_id);
            await srv.DeleteProduction(modelOut.BATCH_NO);
            modelOut.POSTED = false;
            await srv.Save(modelOut);

            return RedirectToAction("Remove", "Upsert");

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveHowler(PendingImportModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new UpsertService();
            var modelOut = await srv.GetMainById(model._id);
            modelOut.MONETARY_VALUE = model.MONETARY_VALUE;
            modelOut.WEIGHT = model.WEIGHT;

            await srv.SaveMain(modelOut);
            return RedirectToAction("Index", "Upsert");

        }
    }
}
