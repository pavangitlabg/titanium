using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using jsreport.Types;
using ISSB.Models;
using jsreport.AspNetCore;
using Microsoft.AspNetCore.Html;
using System.Security.Claims;
using System.Data;
using System;
using System.Web;
using System.Reflection;
using ISSB.Helpers;
using Data.Enums;
using System.Text;
using Data.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;

namespace ISSB.Controllers
{
    public class ReportController : Controller
    {

        public IJsReportMVCService JsReportMVCService { get; }
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public ReportController(IHostingEnvironment env, IJsReportMVCService jsReportMVCService)
        {
            _env = env;
            JsReportMVCService = jsReportMVCService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var srv = new ReportDesignerService();
            var model = await srv.GetReports(UserId);
            foreach (var Item in model)
            {
                if (string.IsNullOrEmpty(Item.BuildStatus)) { Item.BuildStatus = string.Empty; }

                if (Item.BuildStatus.Equals("Done"))
                {
                    Item.Name = Item.Name + " [READY]";
                }
            }
            IQueryable data = model.AsQueryable();

            //  await InitKeys();

            return View(data);

        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var querySrv = new QueryService();

            var srv = new ReportDesignerService();
            var modelOut = await srv.GetReportById(_id);
            var queryList = await querySrv.GetAllReportHeadersByUser(UserId);
            modelOut.Data = queryList;

            if (modelOut.Keys == null || modelOut.Keys.Count == 0)
            {
                modelOut.Keys = await GetFieldList(_id, modelOut.ReportID);
            }

            return View(modelOut);

        }

        [Authorize]
        public async Task<IActionResult> SendTo(string _id)
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;

            var rptSrv = new ReportDesignerService();
            var rptModel = await rptSrv.GetReportById(_id);

            var model = new SendToModel
            {
                FromEmail = UserId,
                RecordQueryId = rptModel.ReportID,
                ReportId = _id,
                ToEmail = string.Empty
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Send(SendToModel model)
        {
            if (model.ToEmail.ToLower().Equals(model.FromEmail.ToLower()))
            {
                ViewData["Message"] = "Query can't be sent you your logon!";
                model.ToEmail = model.ToEmail.ToLower();
                return View("SendTo", model);
            }


            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Please enter an email address";
                return View("SendTo", model);
            }

            var userSrv = new UserServices();
            var userModel = await userSrv.GetUser(model.ToEmail);

            if (userModel.Email == null)
            {
                ViewData["Message"] = "Email address not found!";
                return View("SendTo", model);
            }

            var rptSrv = new ReportDesignerService();
            var rptModel = await rptSrv.GetReportById(model.ReportId);
            rptModel._id = string.Empty;
            rptModel.User = model.ToEmail.ToLower();
            rptModel.Name = rptModel.Name + " from: " + model.FromEmail;


            model.ToEmail = model.ToEmail.ToLower();
            var reportService = new QueryService();
            var reportModel = await reportService.GetReportByID(model.FromEmail, model.RecordQueryId);
            reportModel.User = model.ToEmail;
            reportModel.Id = string.Empty;
            reportModel.Name = reportModel.Name + " from: " + model.FromEmail;
            rptModel.ReportID = rptModel._id;
            var qModel = await reportService.InserNewQuery(reportModel);
            rptModel.ReportID = qModel.Id;
            await rptSrv.Add(rptModel);

            return RedirectToAction("Index", "Report");
        }

        private async Task<List<ReportKeysModel>> GetFieldList(string RecordId, string ReportId)
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var querySrv = new QueryService();

            var srv = new ReportDesignerService();
            var modelOut = await srv.GetReportById(RecordId);
            modelOut.FieldSort = string.Empty;

            var queryList = await querySrv.GetAllReportHeadersByUser(UserId);
            modelOut.Data = queryList;

            var rkSrv = new ReportKeysService();

            modelOut.HTML = HttpUtility.UrlDecode(modelOut.HTML);
            modelOut.Header = HttpUtility.UrlDecode(modelOut.Header);
            modelOut.TableHeader = HttpUtility.UrlDecode(modelOut.TableHeader);
            modelOut.Content = HttpUtility.UrlDecode(modelOut.Content);
            modelOut.ReportID = ReportId;
            var availFields = await SearchResults(ReportId);

            var ListModel = availFields.Data.ToList();
            var sModel = ListModel[0];

            var reportKeys = await rkSrv.GetReportKeys();

            int idx;
            if (sModel.SC_GEO == null)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("SC_GEO"));
                reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("SC_NAME"));
                reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("SC_GEO_REGION"));
                reportKeys.RemoveAt(idx);

            }

            if (sModel.MC_GEO == null)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("MC_GEO"));
                reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("MC_NAME"));
                reportKeys.RemoveAt(idx);

            }

            if (sModel.TARIFF_CODE == null)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("TARIFF_CODE"));
                reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("TARIFF_DESCRIPTION"));
                reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("SIX_DIGIT"));
                reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("TWO_DIGIT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TARIFF_CATEGORY"));
                reportKeys.RemoveAt(idx);
            }

            if (sModel.PORT_ID == 0)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("PORT"));
                reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("PORT_NAME"));
                reportKeys.RemoveAt(idx);

            }

            if (!availFields.Search.GroupByMonth)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("MONTH"));
                reportKeys.RemoveAt(idx);
            }

            if (!availFields.Search.GroupByQuarter)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("QUARTER"));
                reportKeys.RemoveAt(idx);
            }

            if (!availFields.Search.GroupByYear)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("YEAR"));
                reportKeys.RemoveAt(idx);
            }


            if (modelOut.PivotTable)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("YTD_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("YTD_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("YTD_VPT"));
                reportKeys.RemoveAt(idx);

                if (sModel.MONTH.Equals(0) && sModel.YEAR.Equals(0))
                {
                    reportKeys = new List<ReportKeysModel>();
                }


            }

            if (!modelOut.PivotTable)
            {


                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_VPT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_WEIGHT"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_VALUE"));
                reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_VPT"));
                reportKeys.RemoveAt(idx);
            }


            //modelOut.Keys = reportKeys;
            //await srv.Save(modelOut);

            return reportKeys;

        }

        [Authorize]
        public async Task<IActionResult> GetFields(string RecordId, string ReportId)
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var querySrv = new QueryService();

            var srv = new ReportDesignerService();
            var modelOut = await srv.GetReportById(RecordId);
            modelOut.FieldSort = string.Empty;

            var queryList = await querySrv.GetAllReportHeadersByUser(UserId);
            modelOut.Data = queryList;

            var rkSrv = new ReportKeysService();

            modelOut.HTML = HttpUtility.UrlDecode(modelOut.HTML);
            modelOut.Header = HttpUtility.UrlDecode(modelOut.Header);
            modelOut.TableHeader = HttpUtility.UrlDecode(modelOut.TableHeader);
            modelOut.Content = HttpUtility.UrlDecode(modelOut.Content);
            modelOut.ReportID = ReportId;
            var availFields = await SearchResults(ReportId);

            var ListModel = availFields.Data.ToList();
            var sModel = ListModel[0];

            var reportKeys = await rkSrv.GetReportKeys();

            int idx;
            if (sModel.SC_GEO == null)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("SC_GEO"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("SC_NAME"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("SC_GEO_REGION"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

            }

            if (sModel.MC_GEO == null)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("MC_GEO"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("MC_NAME"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

            }

            if (sModel.TARIFF_CODE == null)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("TARIFF_CODE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("TARIFF_DESCRIPTION"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("SIX_DIGIT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("TWO_DIGIT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("TARIFF_CATEGORY"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
            }

            if (sModel.PORT_ID == 0)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("PORT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
                idx = reportKeys.FindIndex(s => s.Key.Equals("PORT_NAME"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

            }

            if (!availFields.Search.GroupByMonth)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("MONTH"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
            }

            if (!availFields.Search.GroupByQuarter)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("QUARTER"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
            }

            if (!availFields.Search.GroupByYear)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("YEAR"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
            }

            if (modelOut.PivotTable)
            {
                if (!modelOut.ShowWeight && !modelOut.ShowWeightValueVpt && !modelOut.ShowWeightVpt)
                {
                    idx = reportKeys.FindIndex(s => s.Key.Equals("WEIGHT"));
                    if (idx >= 0)
                        reportKeys.RemoveAt(idx);

                    idx = reportKeys.FindIndex(s => s.Key.Equals("VALUE"));
                    if (idx >= 0)
                        reportKeys.RemoveAt(idx);

                    idx = reportKeys.FindIndex(s => s.Key.Equals("VPT"));
                    if (idx >= 0)
                        reportKeys.RemoveAt(idx);

                    idx = reportKeys.FindIndex(s => s.Key.Equals("YTD_WEIGHT"));
                    if (idx >= 0)
                        reportKeys.RemoveAt(idx);

                    idx = reportKeys.FindIndex(s => s.Key.Equals("YTD_VALUE"));
                    if (idx >= 0)
                        reportKeys.RemoveAt(idx);

                    idx = reportKeys.FindIndex(s => s.Key.Equals("YTD_VPT"));
                    if (idx >= 0)
                        reportKeys.RemoveAt(idx);

                    if (sModel.MONTH.Equals(0) && sModel.YEAR.Equals(0))
                    {
                        reportKeys = new List<ReportKeysModel>();
                    }
                }
            }

            if (!modelOut.PivotTable)
            {


                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

            }

            if (modelOut.ShowWeight || modelOut.ShowWeightValueVpt || modelOut.ShowWeightVpt)
            {
                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JAN_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("FEB_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAR_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("APR_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("MAY_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUN_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("JUL_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("AUG_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("SEP_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("OCT_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("NOV_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("DEC_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_WEIGHT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_VALUE"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);

                idx = reportKeys.FindIndex(s => s.Key.Equals("TOTAL_VPT"));
                if (idx >= 0)
                    reportKeys.RemoveAt(idx);
            }

            modelOut.Keys = reportKeys;
            await srv.Save(modelOut);

            return Json(new { success = true });

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var querySrv = new QueryService();

            var ReportHeaderStr = "<div style='text-align:center; font-size: 10px; width: 100%'>Page number <span class='pageNumber'></span>&nbsp;of&nbsp;<span class='totalPages'></span></div>";

            string ReportContent = "<meta content='text / html; charset = utf-8' http-equiv='Content-Type'><link rel='stylesheet' href='https://cdn.rawgit.com/olton/Metro-UI-CSS/master/build/css/metro.min.css'>" +
                                  "<link rel='stylesheet' href='https://cdn.rawgit.com/olton/Metro-UI-CSS/master/build/css/metro.min.css'>" +
                                  "<style>.footer { position: fixed; left: 0; bottom: 0;  width: 100%;  height: 12px;  background-color: grey; color: white; text-align: center;font-size: 10px; }</style>" +
                                  "<h4>[REPORT_TITLE]</h4><hr/>[REPORT_DATA]" +
                                  "<div class='footer'>" +
                                  "<p>ISSB Report</p></div>";


            var rkSrv = new ReportKeysService();

            var reportKeys = await rkSrv.GetReportKeys();


            var modelOut = new ReportDesignerModel
            {
                Header = ReportHeaderStr,
                Content = ReportContent,
                Name = string.Empty,
                MarginTop = "1.5cm",
                MarginBottom = "1.5cm",
                MarginLeft = "1.5cm",
                MarginRight = "1.5cm",
                Scale = 1,
                PageSize = "A4",
                Landscape = false,
                ReportID = string.Empty,
                IncludeHeader = false,
                FileType = "PDF",
                ValueFormat = "£{0:#,0}",
                WeightFormat = "{0:#,0}",
                PageRanges = "1-100",
                FontSize = "10px",
                TableHeader = "<p><br></p>",
                Keys = reportKeys,
                TableStyle = "<table id='1' class='table striped' style=\'font-size: 10px'>",
                CSS = "<style></style>"

            };

            var queryList = await querySrv.GetAllReportHeadersByUser(UserId);
            modelOut.Data = queryList;

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(ReportDesignerModel model)
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            model.User = UserId;

            var srv = new ReportDesignerService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Report Details";


                var rkSrv = new ReportKeysService();
                var querySrv = new QueryService();

                var reportKeys = await rkSrv.GetReportKeys();
                model.Keys = reportKeys;
                model.Header = HttpUtility.UrlDecode(model.Header);

                model.TableHeader = HttpUtility.UrlDecode(model.TableHeader);

                var queryList = await querySrv.GetAllReportHeadersByUser(UserId);
                model.Data = queryList;
                model.Content = HttpUtility.UrlDecode(model.Content);
                model.HTML = HttpUtility.UrlDecode(model.HTML);

                return View("Add", model);
            }
            model.HTML = HttpUtility.UrlDecode(model.HTML);
            model.Header = HttpUtility.UrlDecode(model.Header);
            model.TableHeader = HttpUtility.UrlDecode(model.TableHeader);
            model.Content = HttpUtility.UrlDecode(model.Content);

            await srv.Add(model);
            return RedirectToAction("Index", "Report");
        }

        [Authorize]
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> Save(ReportDesignerModel model)
        {
            var srv = new ReportDesignerService();
            var modelUpDate = await srv.GetReportById(model._id);

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";

                var querySrv = new QueryService();
                var queryList = await querySrv.GetAllReportHeadersByUser(model.User);
                model.Data = queryList;


                model.Keys = modelUpDate.Keys;
                model.Header = HttpUtility.UrlDecode(model.Header);
                model.TableHeader = HttpUtility.UrlDecode(model.TableHeader);

                // model.Data = model.Data;
                model.Content = HttpUtility.UrlDecode(model.Content);
                model.HTML = HttpUtility.UrlDecode(model.HTML);

                return View("Edit", model);
            }

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            model.User = UserId;
            model.Keys = modelUpDate.Keys;
            model.HTML = string.Empty; //HttpUtility.UrlDecode(model.HTML);
            model.Header = HttpUtility.UrlDecode(model.Header);
            model.TableHeader = HttpUtility.UrlDecode(model.TableHeader);
            model.Content = HttpUtility.UrlDecode(model.Content);

            await srv.Save(model);

            //var UserId = User.FindFirst(ClaimTypes.Email).Value;            
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ReportDesignerModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Reports Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Report");

        }



        [Authorize]
        public async Task<IActionResult> ReportTXTOutput(string _id)
        {

            var srv = new ReportDesignerService();
            var model = await srv.GetReportById(_id);
            var savedQuerySrv = new QueryService();
            var savedQModel = await savedQuerySrv.GetReportByID(model.ReportID);
            var tariffCategoryList = new List<ProductCustomFilterModel>();

            if (savedQModel.ProductGroupType.Equals("Custom"))
            {
                var filterSrv = new ProductCustomFilterService();
                tariffCategoryList = await filterSrv.GetCustomProductsbyReport(model.ReportID);
            }

            model.FileType = "TXT";
            ReportEngineModel keyData = new ReportEngineModel();

            keyData = await SearchResults(model.ReportID);


            var DataList = new List<ReportWriterModel>();
            var regionService = new SourceCountryFilterService();
            var regionList = await regionService.GetRegionCodes();
            var twoDigitService = new ProductService();
            var twoDigitList = await twoDigitService.GetAllMedumProducts();
            ReportOutModelJsonModel dataOut = new ReportOutModelJsonModel();

            if (keyData.Data != null)
            {
                foreach (var Item in keyData.Data)
                {
                    var TwoDigit = string.Empty;
                    var TariffCategory = string.Empty;

                    var outModel = new ReportWriterModel
                    {
                        ID = Item.ID,
                        MC_GEO = Item.MC_GEO,
                        MC_NAME = Item.MC_NAME,
                        MONETARY_VALUE = Item.MONETARY_VALUE,
                        MONTH = Item.MONTH,
                        PORT_ID = Item.PORT_ID,
                        PORT_NAME = Item.PORT_NAME,
                        QUARTER = Item.QUARTER,
                        SC_GEO = Item.SC_GEO,
                        SC_NAME = Item.SC_NAME,
                        SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                        TARIFF_CODE = Item.TARIFF_CODE,
                        TARIFF_LEGEND = Item.TARIFF_LEGEND,
                        TIME_ID = Item.TIME_ID,
                        WEIGHT = Item.WEIGHT,
                        YEAR = Item.YEAR,
                        YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                        YTD_WEIGHT = Item.YTD_WEIGHT

                    };

                    if (!string.IsNullOrEmpty(Item.SC_GEO))
                    {

                        var rModel = regionList.Find(x => x.GeoCode.Equals(Item.SC_GEO));
                        if (rModel != null)
                            outModel.SC_GEO_REGION = rModel.RegionName;
                        else
                            outModel.SC_GEO_REGION = "--";
                    }
                    else
                    {
                        outModel.SC_GEO_REGION = "--";
                    }

                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                    {
                        outModel.TARIFF_CATEGORY = Item.TARIFF_CODE;
                        outModel.SIX_DIGIT = Item.TARIFF_CODE.Substring(0, 6);
                        foreach (var Prod in twoDigitList)
                        {

                            foreach (var item in Prod.Items)
                            {
                                if (item.TariffCode.Equals(outModel.SIX_DIGIT))
                                {
                                    TwoDigit = Prod.Code + " " + Prod.Name;
                                    TwoDigit = TwoDigit.Replace(",", " ");
                                }

                                foreach (var custItem in tariffCategoryList)
                                {
                                    var cModel = custItem.Items.FirstOrDefault(x => x.TariffCode.Contains(Item.TARIFF_CODE));
                                    if (cModel != null)
                                    {
                                        TariffCategory = custItem.Name;
                                        break;
                                    }
                                    else
                                    {
                                        TariffCategory = "--";
                                    }
                                }
                            }
                        }


                    }

                    outModel.TARIFF_CATEGORY = TariffCategory;
                    outModel.TWO_DIGIT = TwoDigit;
                    DataList.Add(outModel);

                }

                dataOut = new ReportOutModelJsonModel { data = DataList };
            }

            if (model.PivotTable)
            {
                var fList = new List<string>();

                if (!string.IsNullOrEmpty(model.FieldSort))
                {
                    string[] words = model.FieldSort.Split(',');

                    foreach (var f in words)
                    {
                        if (!string.IsNullOrEmpty(f))
                        {
                            fList.Add(f);
                        }
                    }
                }
                int idx = 0;
                if (!model.GroupByFlow)
                {
                    idx = fList.FindIndex(s => s.Equals("MONTH"));
                    if (idx >= 0)
                        fList.RemoveAt(idx);
                }

                if (keyData.Search.GroupByYear && !keyData.Search.GroupByMonth && !keyData.Search.GroupByQuarter)
                {
                    idx = fList.FindIndex(s => s.Equals("YEAR"));
                    if (idx >= 0)
                        fList.RemoveAt(idx);
                }

                idx = fList.FindIndex(s => s.Equals("WEIGHT"));
                if (idx >= 0)
                    fList.RemoveAt(idx);

                foreach (var Item in dataOut.data)
                {
                    var key = string.Empty;

                    foreach (var kItem in fList)
                    {
                        if (kItem.Equals("SC_GEO"))
                        {
                            if (string.IsNullOrEmpty(Item.SC_GEO))
                            {
                                Item.SC_GEO = string.Empty;
                            }
                            else
                                key += Item.SC_GEO + "|";
                        }

                        if (kItem.Equals("SC_NAME"))
                        {
                            if (string.IsNullOrEmpty(Item.SC_NAME))
                            {
                                Item.SC_NAME = string.Empty;
                            }
                            else
                                key += Item.SC_NAME + "|";
                        }

                        if (kItem.Equals("SC_GEO_REGION"))
                        {
                            if (string.IsNullOrEmpty(Item.SC_GEO_REGION))
                            {
                                Item.SC_GEO_REGION = string.Empty;
                            }
                            else
                                key += Item.SC_GEO_REGION + "|";
                        }

                        if (kItem.Equals("MC_GEO"))
                        {
                            if (string.IsNullOrEmpty(Item.MC_GEO))
                            {
                                Item.MC_GEO = string.Empty;
                            }
                            else
                                key += Item.MC_GEO + "|";
                        }

                        if (kItem.Equals("MC_NAME"))
                        {
                            if (string.IsNullOrEmpty(Item.MC_NAME))
                            {
                                Item.MC_NAME = string.Empty;
                            }
                            else
                                key += Item.MC_NAME + "|";
                        }
                        if (kItem.Equals("SIDE_OF_TRADE"))
                        {

                            if (string.IsNullOrEmpty(Item.SIDE_OF_TRADE))
                            {
                                Item.SIDE_OF_TRADE = string.Empty;
                            }
                            else
                                key += Item.SIDE_OF_TRADE + "|";

                        }

                        if (kItem.Equals("PORT"))
                        {
                            if (Item.PORT_ID == 0)
                            {
                                Item.PORT_ID = 0;
                            }
                            else
                                key += Item.PORT_ID + "|";
                        }

                        if (kItem.Equals("PORT_NAME"))
                        {
                            if (string.IsNullOrEmpty(Item.PORT_NAME))
                            {
                                Item.PORT_NAME = string.Empty;
                            }
                            else
                                key += Item.PORT_NAME + "|";
                        }
                        if (kItem.Equals("TARIFF_CODE"))
                        {
                            if (string.IsNullOrEmpty(Item.TARIFF_CODE))
                            {
                                Item.TARIFF_CODE = string.Empty;
                            }
                            else
                                key += Item.TARIFF_CODE + "|";
                        }
                        if (kItem.Equals("TARIFF_DESCRIPTION"))
                        {
                            if (string.IsNullOrEmpty(Item.TARIFF_LEGEND))
                            {
                                Item.TARIFF_LEGEND = string.Empty;
                            }
                            else
                                key += Item.TARIFF_LEGEND + "|";
                        }
                        if (kItem.Equals("SIX_DIGIT"))
                        {
                            if (string.IsNullOrEmpty(Item.SIX_DIGIT))
                            {
                                Item.SIX_DIGIT = string.Empty;
                            }
                            else
                                key += Item.SIX_DIGIT + "|";
                        }
                        if (kItem.Equals("TWO_DIGIT"))
                        {
                            if (string.IsNullOrEmpty(Item.TWO_DIGIT))
                            {
                                Item.TWO_DIGIT = string.Empty;
                            }
                            else
                                key += Item.TWO_DIGIT + "|";

                        }

                        if (kItem.Equals("TARIFF_CATEGORY"))
                        {
                            if (string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                            {
                                Item.TARIFF_CATEGORY = string.Empty;
                            }
                            else
                                key += Item.TARIFF_CATEGORY + "|";

                        }


                        if (kItem.Equals("YEAR"))
                        {
                            key += Item.YEAR + "|";
                        }

                        if (kItem.Equals("MONTH"))
                        {
                            key += Item.MONTH + "|";
                        }
                    }

                    Item.KEY = key.Remove(key.Length - 1);

                }

                List<ReportWriterModel> rData = dataOut.data;

                var pivotArray = new object();
                var jsonMonth = string.Empty;
                var jsonQuarter = string.Empty;
                var jsonYear = string.Empty;
                var jsonFlow = string.Empty;
                var selectedCurrency = keyData.SelectedCurrency;

                List<ReportPivotMonthModel> reportByMonthWeight = new List<ReportPivotMonthModel>();
                List<ReportPivotMonthModel> reportByMonthValue = new List<ReportPivotMonthModel>();
                List<ReportPivotYearModel> reportByYearWeight = new List<ReportPivotYearModel>();

                List<ReportPivotQuarterModel> reportByQuarterWeight = new List<ReportPivotQuarterModel>();
                List<ReportPivotQuarterModel> reportByQuarterValue = new List<ReportPivotQuarterModel>();
                List<ReportPivotYearModel> reportByYearValue = new List<ReportPivotYearModel>();

                List<ReportPivotFlowModel> reportByFlowWeight = new List<ReportPivotFlowModel>();
                List<ReportPivotFlowModel> reportByFlowValue = new List<ReportPivotFlowModel>();

                //Batch Start


                bool bIgnore = false;
                if (model.GroupByFlow)
                {
                    bIgnore = true;
                    pivotArray = rData.ToPivotArrayMany(
                                  item => item.SIDE_OF_TRADE,
                                  item => item.KEY,
                                  items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                    jsonFlow = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    reportByFlowWeight = JsonConvert.DeserializeObject<List<ReportPivotFlowModel>>(jsonFlow);

                    //var dataCnt = rData.Count / 1000;
                    //var remainder = rData.Count % 1000;
                    //var lastRange = 1000;
                    //var startRange = 0;

                    //for (int i = 0; i <= dataCnt - 1; i++)
                    //{
                    //    var DataBatch = rData.Skip(startRange).Take(1000).ToArray();
                    //    pivotArray = DataBatch.ToPivotArrayMany(
                    //                  item => item.SIDE_OF_TRADE,
                    //                  item => item.KEY,
                    //                  items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                    //    jsonFlow = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    //    var pMonth = new List<ReportPivotFlowModel>();
                    //    reportByFlowWeight = JsonConvert.DeserializeObject<List<ReportPivotFlowModel>>(jsonFlow);

                    //    reportByFlowWeight.AddRange(pMonth);
                    //    startRange = lastRange + 1;
                    //    lastRange += 1000;
                    //}

                    //if (remainder > 0)
                    //{
                    //    var DataBatch = rData.Skip(startRange).Take(remainder).ToArray();
                    //    pivotArray = DataBatch.ToPivotArrayMany(
                    //                  item => item.SIDE_OF_TRADE,
                    //                  item => item.KEY,
                    //                  items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                    //    jsonFlow = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    //    var pMonth = new List<ReportPivotFlowModel>();
                    //    reportByFlowWeight = JsonConvert.DeserializeObject<List<ReportPivotFlowModel>>(jsonFlow);
                    //    reportByFlowWeight.AddRange(pMonth);
                    //}

                    pivotArray = rData.ToPivotArrayMany(
                                  item => item.SIDE_OF_TRADE,
                                  item => item.KEY,
                                  items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                    jsonFlow = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    reportByFlowValue = JsonConvert.DeserializeObject<List<ReportPivotFlowModel>>(jsonFlow);

                //    dataCnt = rData.Count / 1000;
                //    remainder = rData.Count % 1000;
                //    lastRange = 1000;
                //    startRange = 0;

                //    for (int i = 0; i <= dataCnt - 1; i++)
                //    {
                //        var DataBatch = rData.Skip(startRange).Take(1000).ToArray();
                //        pivotArray = DataBatch.ToPivotArrayMany(
                //                      item => item.SIDE_OF_TRADE,
                //                      item => item.KEY,
                //                      items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                //        jsonFlow = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                //        var pMonth = new List<ReportPivotFlowModel>();
                //        reportByFlowValue = JsonConvert.DeserializeObject<List<ReportPivotFlowModel>>(jsonFlow);

                //        reportByFlowValue.AddRange(pMonth);
                //        startRange = lastRange + 1;
                //        lastRange += 1000;
                //    }

                //    if (remainder > 0)
                //    {
                //        var DataBatch = rData.Skip(startRange).Take(remainder).ToArray();
                //        pivotArray = DataBatch.ToPivotArrayMany(
                //                      item => item.SIDE_OF_TRADE,
                //                      item => item.KEY,
                //                      items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                //        jsonFlow = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                //        var pMonth = new List<ReportPivotFlowModel>();
                //        reportByFlowValue = JsonConvert.DeserializeObject<List<ReportPivotFlowModel>>(jsonFlow);
                //        reportByFlowValue.AddRange(pMonth);
                //    }
                }


                if (keyData.Search.GroupByMonth && bIgnore == false)
                {
                    var DataBatch = rData;//.Skip(startRange).Take(1000).ToArray();
                    pivotArray = DataBatch.ToPivotArrayMany(
                                  item => item.MONTH,
                                  item => item.KEY,
                                  items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                    jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    var pMonth = new List<ReportPivotMonthModel>();
                    pMonth = JsonConvert.DeserializeObject<List<ReportPivotMonthModel>>(jsonMonth);

                    reportByMonthWeight.AddRange(pMonth);



                    /*  for (int i = 0; i <= dataCnt - 1; i++)
                      {
                          var DataBatch = rData.Skip(startRange).Take(1000).ToArray();
                          pivotArray = DataBatch.ToPivotArrayMany(
                                        item => item.MONTH,
                                        item => item.KEY,
                                        items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                          jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                          var pMonth = new List<ReportPivotMonthModel>();
                          pMonth = JsonConvert.DeserializeObject<List<ReportPivotMonthModel>>(jsonMonth);

                          reportByMonthWeight.AddRange(pMonth);
                          startRange = lastRange + 1;
                          lastRange += 1000;
                      }

                      if (remainder > 0)
                      {
                          var DataBatch = rData.Skip(startRange).Take(remainder).ToArray();
                          pivotArray = DataBatch.ToPivotArrayMany(
                                        item => item.MONTH,
                                        item => item.KEY,
                                        items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                          jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                          var pMonth = new List<ReportPivotMonthModel>();
                          pMonth = JsonConvert.DeserializeObject<List<ReportPivotMonthModel>>(jsonMonth);

                          reportByMonthWeight.AddRange(pMonth);
                      }
                    */
                }

                if (keyData.Search.GroupByYear && !keyData.Search.GroupByMonth && bIgnore == false)
                {
                    pivotArray = rData.ToPivotArrayMany(
                                  item => item.YEAR,
                                  item => item.KEY,
                                  items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                    jsonYear = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    reportByYearWeight = JsonConvert.DeserializeObject<List<ReportPivotYearModel>>(jsonYear);

                    /*  var dataCnt = rData.Count / 1000;
                      var remainder = rData.Count % 1000;
                      var lastRange = 1000;
                      var startRange = 0;

                      for (int i = 0; i <= dataCnt - 1; i++)
                      {
                          var DataBatch = rData.Skip(startRange).Take(1000).ToArray();
                          pivotArray = DataBatch.ToPivotArrayMany(
                                       item => item.YEAR,
                                       item => item.KEY,
                                       items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                          jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                          var pMonth = new List<ReportPivotYearModel>();
                          pMonth = JsonConvert.DeserializeObject<List<ReportPivotYearModel>>(jsonMonth);

                          reportByYearWeight.AddRange(pMonth);
                          startRange = lastRange + 1;
                          lastRange += 1000;
                      }

                      if (remainder > 0)
                      {
                          var DataBatch = rData.Skip(startRange).Take(remainder).ToArray();
                          pivotArray = DataBatch.ToPivotArrayMany(
                                       item => item.YEAR,
                                       item => item.KEY,
                                       items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                          jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                          var pMonth = new List<ReportPivotYearModel>();
                          pMonth = JsonConvert.DeserializeObject<List<ReportPivotYearModel>>(jsonMonth);

                          reportByYearWeight.AddRange(pMonth);
                      }*/
                }

                if (keyData.Search.GroupByQuarter && bIgnore == false)
                {
                    pivotArray = rData.ToPivotArrayMany(
                                  item => item.QUARTER,
                                  item => item.KEY,
                                  items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                    jsonQuarter = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    reportByQuarterWeight = JsonConvert.DeserializeObject<List<ReportPivotQuarterModel>>(jsonQuarter);

                    /*  var dataCnt = rData.Count / 1000;
                      var remainder = rData.Count % 1000;
                      var lastRange = 1000;
                      var startRange = 0;

                      for (int i = 0; i <= dataCnt - 1; i++)
                      {
                          var DataBatch = rData.Skip(startRange).Take(1000).ToArray();
                          pivotArray = DataBatch.ToPivotArrayMany(
                                      item => item.QUARTER,
                                      item => item.KEY,
                                      items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                          jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                          var pMonth = new List<ReportPivotQuarterModel>();
                          pMonth = JsonConvert.DeserializeObject<List<ReportPivotQuarterModel>>(jsonMonth);

                          reportByQuarterWeight.AddRange(pMonth);
                          startRange = lastRange + 1;
                          lastRange += 1000;
                      }

                      if (remainder > 0)
                      {
                          var DataBatch = rData.Skip(startRange).Take(remainder).ToArray();
                          pivotArray = DataBatch.ToPivotArrayMany(
                                      item => item.QUARTER,
                                      item => item.KEY,
                                      items => items.Any() ? items.Sum(x => x.WEIGHT) : 0);

                          jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                          var pMonth = new List<ReportPivotQuarterModel>();
                          pMonth = JsonConvert.DeserializeObject<List<ReportPivotQuarterModel>>(jsonMonth);

                          reportByQuarterWeight.AddRange(pMonth);
                      }*/
                }

                if (keyData.Search.GroupByMonth && bIgnore == false)
                {
                    pivotArray = rData.ToPivotArrayMany(
                              item => item.MONTH,
                              item => item.KEY,
                              items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                    jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    reportByMonthValue = JsonConvert.DeserializeObject<List<ReportPivotMonthModel>>(jsonMonth);
                    /*
                    var dataCnt = rData.Count / 1000;
                    var remainder = rData.Count % 1000;
                    var lastRange = 1000;
                    var startRange = 0;

                    for (int i = 0; i <= dataCnt - 1; i++)
                    {
                        var DataBatch = rData.Skip(startRange).Take(1000).ToArray();
                        pivotArray = DataBatch.ToPivotArrayMany(
                                      item => item.MONTH,
                                      item => item.KEY,
                                      items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                        jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                        var pMonth = new List<ReportPivotMonthModel>();
                        pMonth = JsonConvert.DeserializeObject<List<ReportPivotMonthModel>>(jsonMonth);

                        reportByMonthValue.AddRange(pMonth);
                        startRange = lastRange + 1;
                        lastRange += 1000;
                    }

                    if (remainder > 0)
                    {
                        var DataBatch = rData.Skip(startRange).Take(remainder).ToArray();
                        pivotArray = DataBatch.ToPivotArrayMany(
                                      item => item.MONTH,
                                      item => item.KEY,
                                       items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                        jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                        var pMonth = new List<ReportPivotMonthModel>();
                        pMonth = JsonConvert.DeserializeObject<List<ReportPivotMonthModel>>(jsonMonth);

                        reportByMonthValue.AddRange(pMonth);
                    }*/
                }

                if (keyData.Search.GroupByYear && !keyData.Search.GroupByMonth && bIgnore == false)
                {
                    pivotArray = rData.ToPivotArrayMany(
                              item => item.YEAR,
                              item => item.KEY,
                              items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                    jsonYear = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    reportByYearValue = JsonConvert.DeserializeObject<List<ReportPivotYearModel>>(jsonYear);

                    /* var dataCnt = rData.Count / 1000;
                     var remainder = rData.Count % 1000;
                     var lastRange = 1000;
                     var startRange = 0;

                     for (int i = 0; i <= dataCnt - 1; i++)
                     {
                         var DataBatch = rData.Skip(startRange).Take(1000).ToArray();
                         pivotArray = DataBatch.ToPivotArrayMany(
                                      item => item.YEAR,
                                      item => item.KEY,
                                      items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                         jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                         var pMonth = new List<ReportPivotYearModel>();
                         pMonth = JsonConvert.DeserializeObject<List<ReportPivotYearModel>>(jsonMonth);

                         reportByYearValue.AddRange(pMonth);
                         startRange = lastRange + 1;
                         lastRange += 1000;
                     }

                     if (remainder > 0)
                     {
                         var DataBatch = rData.Skip(startRange).Take(remainder).ToArray();
                         pivotArray = DataBatch.ToPivotArrayMany(
                                      item => item.YEAR,
                                      item => item.KEY,
                                      items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                         jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                         var pMonth = new List<ReportPivotYearModel>();
                         pMonth = JsonConvert.DeserializeObject<List<ReportPivotYearModel>>(jsonMonth);

                         reportByYearValue.AddRange(pMonth);
                     }*/
                }

                if (keyData.Search.GroupByQuarter && bIgnore == false)
                {
                    pivotArray = rData.ToPivotArrayMany(
                              item => item.QUARTER,
                              item => item.KEY,
                              items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                    jsonQuarter = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                    reportByQuarterValue = JsonConvert.DeserializeObject<List<ReportPivotQuarterModel>>(jsonQuarter);

                    /*   var dataCnt = rData.Count / 1000;
                       var remainder = rData.Count % 1000;
                       var lastRange = 1000;
                       var startRange = 0;

                       for (int i = 0; i <= dataCnt - 1; i++)
                       {
                           var DataBatch = rData.Skip(startRange).Take(1000).ToArray();
                           pivotArray = DataBatch.ToPivotArrayMany(
                                        item => item.QUARTER,
                                        item => item.KEY,
                                        items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                           jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                           var pMonth = new List<ReportPivotQuarterModel>();
                           pMonth = JsonConvert.DeserializeObject<List<ReportPivotQuarterModel>>(jsonMonth);

                           reportByQuarterValue.AddRange(pMonth);
                           startRange = lastRange + 1;
                           lastRange += 1000;
                       }

                       if (remainder > 0)
                       {
                           var DataBatch = rData.Skip(startRange).Take(remainder).ToArray();
                           pivotArray = DataBatch.ToPivotArrayMany(
                                        item => item.QUARTER,
                                        item => item.KEY,
                                        items => items.Any() ? items.Sum(x => x.MONETARY_VALUE) : 0);

                           jsonMonth = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                           var pMonth = new List<ReportPivotQuarterModel>();
                           pMonth = JsonConvert.DeserializeObject<List<ReportPivotQuarterModel>>(jsonMonth);

                           reportByQuarterValue.AddRange(pMonth);
                       }
                    */
                }


                //Batch End

                if (model.GroupByFlow && bIgnore == true)
                {
                    var OutSrv = PivotHelper.Instance;
                    var outList = OutSrv.GroupByFlow(fList, reportByFlowWeight, reportByFlowValue, keyData);

                    if (model.FileType.Equals("TXT"))
                    {

                        MemoryStream ms = new MemoryStream();
                        TextWriter tw = new StreamWriter(ms);
                        int cnt = 0, Total = 0;
                        var writeOutList = new List<string>();

                        Total++;
                        foreach (var Item in outList)
                        {
                            var outLine = string.Empty;
                            var headerLine = string.Empty;

                            headerLine += "ID,";
                            outLine = Item.ID.ToString() + ",";

                            foreach (var kItem in fList)
                            {
                                if (kItem.Equals("SC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO))
                                    {
                                        headerLine += "SC_GEO,";
                                        outLine += Item.SC_GEO + ",";
                                    }
                                }
                                if (kItem.Equals("SC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_NAME))
                                    {
                                        headerLine += "SC_NAME,";
                                        outLine += Item.SC_NAME + ",";
                                    }
                                }
                                if (kItem.Equals("SC_GEO_REGION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO_REGION))
                                    {
                                        headerLine += "SC_GEO_REGION,";
                                        outLine += Item.SC_GEO_REGION + ",";
                                    }
                                }
                                if (kItem.Equals("MC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_GEO))
                                    {
                                        headerLine += "MC_GEO,";
                                        outLine += Item.MC_GEO + ",";
                                    }
                                }

                                if (kItem.Equals("MC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_NAME))
                                    {
                                        headerLine += "MC_NAME,";
                                        outLine += Item.MC_NAME + ",";
                                    }
                                }

                                if (kItem.Equals("TARIFF_CODE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {
                                        headerLine += "TARIFF_CODE,";
                                        outLine += Item.TARIFF_CODE + ",";
                                    }
                                }

                                if (kItem.Equals("TARIFF_DESCRIPTION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {
                                        headerLine += "TARIFF_DESCRIPTION,";
                                        outLine += Item.TARIFF_LEGEND + ",";
                                    }
                                }

                                if (kItem.Equals("TWO_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TWO_DIGIT))
                                    {
                                        headerLine += "TWO_DIGIT,";
                                        outLine += Item.TWO_DIGIT + ",";
                                    }
                                }

                                if (kItem.Equals("SIX_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIX_DIGIT))
                                    {
                                        headerLine += "SIX_DIGIT,";
                                        outLine += Item.SIX_DIGIT + ",";
                                    }
                                }

                                if (kItem.Equals("TARIFF_CATEGORY"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                                    {
                                        headerLine += "TARIFF_CATEGORY,";
                                        outLine += Item.TARIFF_CATEGORY + ",";
                                    }
                                }
                                if (kItem.Equals("YEAR"))
                                {
                                    if (Item.YEAR > 0)
                                    {
                                        headerLine += "YEAR,";
                                        outLine += Item.YEAR + ",";
                                    }
                                }
                                if (kItem.Equals("MONTH"))
                                {
                                    if (Item.MONTH > 0)
                                    {
                                        headerLine += "MONTH,";
                                        outLine += Item.MONTH + ",";
                                    }
                                }
                            }

                            if (model.ShowWeight && !model.ShowWeightValueVpt && !model.ShowWeightVpt)
                            {
                                if (Item.EXPORT_WEIGHT > 0)
                                {
                                    headerLine += "EXPORT,";
                                    outLine += Item.EXPORT_WEIGHT + ",";
                                }

                                if (Item.IMPORT_WEIGHT > 0)
                                {
                                    headerLine += "IMPORT,";
                                    outLine += Item.IMPORT_WEIGHT + ",";
                                }
                            }

                            if (model.ShowWeight && !model.ShowWeightValueVpt && model.ShowWeightVpt)
                            {
                                if (Item.EXPORT_WEIGHT > 0)
                                {
                                    headerLine += "EXPORT," + selectedCurrency + ": EXPORT_VPT,";
                                    outLine += Item.EXPORT_WEIGHT + "," + Item.EXPORT_VPT + ",";
                                }

                                if (Item.IMPORT_WEIGHT > 0)
                                {
                                    headerLine += "IMPORT," + selectedCurrency + ": IMPORT_VPT,";
                                    outLine += Item.IMPORT_WEIGHT + "," + Item.IMPORT_VPT + ",";
                                }
                            }

                            if (model.ShowWeight && model.ShowWeightValueVpt && model.ShowWeightVpt)
                            {
                                if (Item.EXPORT_WEIGHT > 0)
                                {
                                    headerLine += "EXPORT,EXPORT_VPT," + selectedCurrency + ": EXPORT_VALUE,";
                                    outLine += Item.EXPORT_WEIGHT + "," + Item.EXPORT_VPT + "," + Item.EXPORT_VALUE + ",";
                                }

                                if (Item.IMPORT_WEIGHT > 0)
                                {
                                    headerLine += "IMPORT,IMPORT_VPT," + selectedCurrency + ": IMPORT_VALUE,";
                                    outLine += Item.IMPORT_WEIGHT + "," + Item.IMPORT_VPT + "," + Item.IMPORT_VALUE + ",";
                                }
                            }

                            if (cnt == 0)
                            {
                                headerLine = headerLine.Remove(headerLine.Length - 1);
                                writeOutList.Add(headerLine);
                            }

                            if (writeOutList.Count > 0)
                            {
                                if (cnt > 0)
                                    headerLine = headerLine.Remove(headerLine.Length - 1);

                                if (headerLine.Length > writeOutList[0].Length)
                                {
                                    writeOutList[0] = headerLine;
                                }
                            }

                            outLine = outLine.Remove(outLine.Length - 1);
                            writeOutList.Add(outLine);

                            cnt++;
                            Total++;
                        }

                        foreach (var Item in writeOutList)
                            tw.WriteLine(Item);

                        tw.Flush();

                        var FileName = model.FileName + ".csv";

                        var length = ms.Length;
                        tw.Close();
                        var toWrite = new byte[length];
                        Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
                        ms.Close();
                        return File(toWrite, "text/plain", FileName);
                    }

                }

                if (keyData.Search.GroupByYear && !keyData.Search.GroupByQuarter && !keyData.Search.GroupByMonth && bIgnore == false)
                {
                    var OutSrv = PivotHelper.Instance;
                    var outList = OutSrv.GroupByYear(fList, reportByYearWeight, reportByYearValue, keyData);

                    if (model.FileType.Equals("TXT"))
                    {

                        MemoryStream ms = new MemoryStream();
                        TextWriter tw = new StreamWriter(ms);
                        int cnt = 0, Total = 0;
                        var writeOutList = new List<string>();

                        Total++;
                        foreach (var Item in outList)
                        {
                            var outLine = string.Empty;
                            var headerLine = string.Empty;

                            foreach (var kItem in fList)
                            {
                                if (kItem.Equals("SC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO))
                                    {
                                        headerLine += "SC_GEO,";
                                        outLine += Item.SC_GEO + ",";
                                    }
                                }
                                if (kItem.Equals("SC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_NAME))
                                    {
                                        headerLine += "SC_NAME,";
                                        outLine += Item.SC_NAME + ",";
                                    }
                                }
                                if (kItem.Equals("SC_GEO_REGION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO_REGION))
                                    {
                                        headerLine += "SC_GEO_REGION,";
                                        outLine += Item.SC_GEO_REGION + ",";
                                    }
                                }
                                if (kItem.Equals("MC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_GEO))
                                    {
                                        headerLine += "MC_GEO,";
                                        outLine += Item.MC_GEO + ",";
                                    }
                                }

                                if (kItem.Equals("MC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_NAME))
                                    {
                                        headerLine += "MC_NAME,";
                                        outLine += Item.MC_NAME + ",";
                                    }
                                }
                                if (kItem.Equals("TARIFF_CODE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {
                                        headerLine += "TARIFF_CODE,";
                                        outLine += Item.TARIFF_CODE + ",";
                                    }
                                }

                                if (kItem.Equals("TARIFF_DESCRIPTION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {
                                        headerLine += "TARIFF_DESCRIPTION,";
                                        outLine += Item.TARIFF_LEGEND + ",";
                                    }
                                }

                                if (kItem.Equals("TWO_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TWO_DIGIT))
                                    {
                                        headerLine += "TWO_DIGIT,";
                                        outLine += Item.TWO_DIGIT + ",";
                                    }
                                }

                                if (kItem.Equals("SIX_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIX_DIGIT))
                                    {
                                        headerLine += "SIX_DIGIT,";
                                        outLine += Item.SIX_DIGIT + ",";
                                    }
                                }

                                if (kItem.Equals("TARIFF_CATEGORY"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                                    {
                                        headerLine += "TARIFF_CATEGORY,";
                                        outLine += Item.TARIFF_CATEGORY + ",";
                                    }
                                }

                                if (kItem.Equals("SIDE_OF_TRADE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIDE_OF_TRADE))
                                    {
                                        headerLine += "SIDE_OF_TRADE,";
                                        outLine += Item.SIDE_OF_TRADE + ",";
                                    }
                                }
                                //if (kItem.Equals("YEAR"))
                                //{
                                //    if (Item.YEAR > 0)
                                //    {
                                //        headerLine += "YEAR,";
                                //        outLine += Item.YEAR + ",";
                                //    }
                                //}
                                //if (kItem.Equals("QUARTER"))
                                //{
                                //    if (Item.QUARTER > 0)
                                //    {
                                //        headerLine += "QUARTER,";
                                //        outLine += Item.QUARTER + ",";
                                //    }
                                //}
                            }


                            if (model.ShowWeight && !model.ShowWeightValueVpt && !model.ShowWeightVpt)
                            {
                                if (Item.Y1990_WEIGHT >= 0)
                                    headerLine += "1990_WEIGHT,";
                                if (Item.Y1991_WEIGHT >= 0)
                                    headerLine += "1991_WEIGHT,";
                                if (Item.Y1992_WEIGHT >= 0)
                                    headerLine += "1992_WEIGHT,";
                                if (Item.Y1993_WEIGHT >= 0)
                                    headerLine += "1993_WEIGHT,";
                                if (Item.Y1994_WEIGHT >= 0)
                                    headerLine += "1994_WEIGHT,";
                                if (Item.Y1995_WEIGHT >= 0)
                                    headerLine += "1995_WEIGHT,";
                                if (Item.Y1996_WEIGHT >= 0)
                                    headerLine += "1996_WEIGHT,";
                                if (Item.Y1997_WEIGHT >= 0)
                                    headerLine += "1997_WEIGHT,";
                                if (Item.Y1998_WEIGHT >= 0)
                                    headerLine += "1998_WEIGHT,";
                                if (Item.Y1999_WEIGHT >= 0)
                                    headerLine += "1999_WEIGHT,";

                                if (Item.Y2000_WEIGHT >= 0)
                                    headerLine += "2000_WEIGHT,";
                                if (Item.Y2001_WEIGHT >= 0)
                                    headerLine += "2001_WEIGHT,";
                                if (Item.Y2002_WEIGHT >= 0)
                                    headerLine += "2002_WEIGHT,";
                                if (Item.Y2003_WEIGHT >= 0)
                                    headerLine += "2003_WEIGHT,";
                                if (Item.Y2004_WEIGHT >= 0)
                                    headerLine += "2004_WEIGHT,";
                                if (Item.Y2005_WEIGHT >= 0)
                                    headerLine += "2005_WEIGHT,";
                                if (Item.Y2006_WEIGHT >= 0)
                                    headerLine += "2006_WEIGHT,";
                                if (Item.Y2007_WEIGHT >= 0)
                                    headerLine += "2007_WEIGHT,";
                                if (Item.Y2008_WEIGHT >= 0)
                                    headerLine += "2008_WEIGHT,";
                                if (Item.Y2009_WEIGHT >= 0)
                                    headerLine += "2009_WEIGHT,";

                                if (Item.Y2010_WEIGHT >= 0)
                                    headerLine += "2010_WEIGHT,";
                                if (Item.Y2011_WEIGHT >= 0)
                                    headerLine += "2011_WEIGHT,";
                                if (Item.Y2012_WEIGHT >= 0)
                                    headerLine += "2012_WEIGHT,";
                                if (Item.Y2013_WEIGHT >= 0)
                                    headerLine += "2013_WEIGHT,";
                                if (Item.Y2014_WEIGHT >= 0)
                                    headerLine += "2014_WEIGHT,";
                                if (Item.Y2015_WEIGHT >= 0)
                                    headerLine += "2015_WEIGHT,";
                                if (Item.Y2016_WEIGHT >= 0)
                                    headerLine += "2016_WEIGHT,";
                                if (Item.Y2017_WEIGHT >= 0)
                                    headerLine += "2017_WEIGHT,";
                                if (Item.Y2018_WEIGHT >= 0)
                                    headerLine += "2018_WEIGHT,";
                                if (Item.Y2019_WEIGHT >= 0)
                                    headerLine += "2019_WEIGHT,";

                                if (Item.Y2020_WEIGHT >= 0)
                                    headerLine += "2020_WEIGHT,";
                                if (Item.Y2021_WEIGHT >= 0)
                                    headerLine += "2021_WEIGHT,";
                                if (Item.Y2022_WEIGHT >= 0)
                                    headerLine += "2022_WEIGHT,";
                                if (Item.Y2023_WEIGHT >= 0)
                                    headerLine += "2023_WEIGHT,";
                                if (Item.Y2024_WEIGHT >= 0)
                                    headerLine += "2024_WEIGHT,";
                                if (Item.Y2025_WEIGHT >= 0)
                                    headerLine += "2025_WEIGHT,";
                                if (Item.Y2026_WEIGHT >= 0)
                                    headerLine += "2026_WEIGHT,";
                                if (Item.Y2027_WEIGHT >= 0)
                                    headerLine += "2027_WEIGHT,";
                                if (Item.Y2028_WEIGHT >= 0)
                                    headerLine += "2028_WEIGHT,";
                                if (Item.Y2029_WEIGHT >= 0)
                                    headerLine += "2029_WEIGHT,";
                                if (Item.Y2030_WEIGHT >= 0)
                                    headerLine += "2030_WEIGHT,";

                                if (Item.Y1990_WEIGHT >= 0)
                                    outLine += Item.Y1990_WEIGHT + ",";
                                if (Item.Y1991_WEIGHT >= 0)
                                    outLine += Item.Y1991_WEIGHT + ",";
                                if (Item.Y1992_WEIGHT >= 0)
                                    outLine += Item.Y1992_WEIGHT + ",";
                                if (Item.Y1993_WEIGHT >= 0)
                                    outLine += Item.Y1993_WEIGHT + ",";
                                if (Item.Y1994_WEIGHT >= 0)
                                    outLine += Item.Y1994_WEIGHT + ",";
                                if (Item.Y1995_WEIGHT >= 0)
                                    outLine += Item.Y1995_WEIGHT + ",";
                                if (Item.Y1996_WEIGHT >= 0)
                                    outLine += Item.Y1996_WEIGHT + ",";
                                if (Item.Y1997_WEIGHT >= 0)
                                    outLine += Item.Y1997_WEIGHT + ",";
                                if (Item.Y1998_WEIGHT >= 0)
                                    outLine += Item.Y1998_WEIGHT + ",";
                                if (Item.Y1999_WEIGHT >= 0)
                                    outLine += Item.Y1999_WEIGHT + ",";

                                if (Item.Y2000_WEIGHT >= 0)
                                    outLine += Item.Y2000_WEIGHT + ",";
                                if (Item.Y2001_WEIGHT >= 0)
                                    outLine += Item.Y2001_WEIGHT + ",";
                                if (Item.Y2002_WEIGHT >= 0)
                                    outLine += Item.Y2002_WEIGHT + ",";
                                if (Item.Y2003_WEIGHT >= 0)
                                    outLine += Item.Y2003_WEIGHT + ",";
                                if (Item.Y2004_WEIGHT >= 0)
                                    outLine += Item.Y2004_WEIGHT + ",";
                                if (Item.Y2005_WEIGHT >= 0)
                                    outLine += Item.Y2005_WEIGHT + ",";
                                if (Item.Y2006_WEIGHT >= 0)
                                    outLine += Item.Y2006_WEIGHT + ",";
                                if (Item.Y2007_WEIGHT >= 0)
                                    outLine += Item.Y2007_WEIGHT + ",";
                                if (Item.Y2008_WEIGHT >= 0)
                                    outLine += Item.Y2008_WEIGHT + ",";
                                if (Item.Y2009_WEIGHT >= 0)
                                    outLine += Item.Y2009_WEIGHT + ",";

                                if (Item.Y2010_WEIGHT >= 0)
                                    outLine += Item.Y2010_WEIGHT + ",";
                                if (Item.Y2011_WEIGHT >= 0)
                                    outLine += Item.Y2011_WEIGHT + ",";
                                if (Item.Y2012_WEIGHT >= 0)
                                    outLine += Item.Y2012_WEIGHT + ",";
                                if (Item.Y2013_WEIGHT >= 0)
                                    outLine += Item.Y2013_WEIGHT + ",";
                                if (Item.Y2014_WEIGHT >= 0)
                                    outLine += Item.Y2014_WEIGHT + ",";
                                if (Item.Y2015_WEIGHT >= 0)
                                    outLine += Item.Y2015_WEIGHT + ",";
                                if (Item.Y2016_WEIGHT >= 0)
                                    outLine += Item.Y2016_WEIGHT + ",";
                                if (Item.Y2017_WEIGHT >= 0)
                                    outLine += Item.Y2017_WEIGHT + ",";
                                if (Item.Y2018_WEIGHT >= 0)
                                    outLine += Item.Y2018_WEIGHT + ",";
                                if (Item.Y2019_WEIGHT >= 0)
                                    outLine += Item.Y2019_WEIGHT + ",";

                                if (Item.Y2020_WEIGHT >= 0)
                                    outLine += Item.Y2020_WEIGHT + ",";
                                if (Item.Y2021_WEIGHT >= 0)
                                    outLine += Item.Y2021_WEIGHT + ",";
                                if (Item.Y2022_WEIGHT >= 0)
                                    outLine += Item.Y2022_WEIGHT + ",";
                                if (Item.Y2023_WEIGHT >= 0)
                                    outLine += Item.Y2023_WEIGHT + ",";
                                if (Item.Y2024_WEIGHT >= 0)
                                    outLine += Item.Y2024_WEIGHT + ",";
                                if (Item.Y2025_WEIGHT >= 0)
                                    outLine += Item.Y2025_WEIGHT + ",";
                                if (Item.Y2026_WEIGHT >= 0)
                                    outLine += Item.Y2026_WEIGHT + ",";
                                if (Item.Y2027_WEIGHT >= 0)
                                    outLine += Item.Y2027_WEIGHT + ",";
                                if (Item.Y2028_WEIGHT >= 0)
                                    outLine += Item.Y2028_WEIGHT + ",";
                                if (Item.Y2029_WEIGHT >= 0)
                                    outLine += Item.Y2029_WEIGHT + ",";
                                if (Item.Y2030_WEIGHT >= 0)
                                    outLine += Item.Y2030_WEIGHT + ",";

                                if (cnt == 0)
                                {
                                    headerLine = headerLine.Remove(headerLine.Length - 1);
                                    writeOutList.Add(headerLine);
                                    //tw.WriteLine(headerLine);
                                }
                            }

                            if (model.ShowWeight && !model.ShowWeightValueVpt && model.ShowWeightVpt)
                            {
                                if (Item.Y1990_WEIGHT > 0)
                                    headerLine += "1990_WEIGHT,1990_VPT,";
                                if (Item.Y1991_WEIGHT > 0)
                                    headerLine += "1991_WEIGHT,1991_VPT,";
                                if (Item.Y1992_WEIGHT > 0)
                                    headerLine += "1992_WEIGHT,1992_VPT,";
                                if (Item.Y1993_WEIGHT > 0)
                                    headerLine += "1993_WEIGHT,1993_VPT,";
                                if (Item.Y1994_WEIGHT > 0)
                                    headerLine += "1994_WEIGHT,1994_VPT,";
                                if (Item.Y1995_WEIGHT > 0)
                                    headerLine += "1995_WEIGHT,1995_VPT,";
                                if (Item.Y1996_WEIGHT > 0)
                                    headerLine += "1996_WEIGHT,1996_VPT,";
                                if (Item.Y1997_WEIGHT > 0)
                                    headerLine += "1997_WEIGHT,1997_VPT,";
                                if (Item.Y1998_WEIGHT > 0)
                                    headerLine += "1998_WEIGHT,1998_VPT,";
                                if (Item.Y1999_WEIGHT > 0)
                                    headerLine += "1999_WEIGHT,1999_VPT,";

                                if (Item.Y2000_WEIGHT > 0)
                                    headerLine += "2000_WEIGHT,2000_VPT,";
                                if (Item.Y2001_WEIGHT > 0)
                                    headerLine += "2001_WEIGHT,2001_VPT,";
                                if (Item.Y2002_WEIGHT > 0)
                                    headerLine += "2002_WEIGHT,2002_VPT,";
                                if (Item.Y2003_WEIGHT > 0)
                                    headerLine += "2003_WEIGHT,2003_VPT,";
                                if (Item.Y2004_WEIGHT > 0)
                                    headerLine += "2004_WEIGHT,2004_VPT,";
                                if (Item.Y2005_WEIGHT > 0)
                                    headerLine += "2005_WEIGHT,2005_VPT,";
                                if (Item.Y2006_WEIGHT > 0)
                                    headerLine += "2006_WEIGHT,2006_VPT,";
                                if (Item.Y2007_WEIGHT > 0)
                                    headerLine += "2007_WEIGHT,2007_VPT,";
                                if (Item.Y2008_WEIGHT > 0)
                                    headerLine += "2008_WEIGHT,2008_VPT,";
                                if (Item.Y2009_WEIGHT > 0)
                                    headerLine += "2009_WEIGHT,2009_VPT,";

                                if (Item.Y2010_WEIGHT > 0)
                                    headerLine += "2010_WEIGHT,2010_VPT,";
                                if (Item.Y2011_WEIGHT > 0)
                                    headerLine += "2011_WEIGHT,2011_VPT,";
                                if (Item.Y2012_WEIGHT > 0)
                                    headerLine += "2012_WEIGHT,2012_VPT,";
                                if (Item.Y2013_WEIGHT > 0)
                                    headerLine += "2013_WEIGHT,2013_VPT,";
                                if (Item.Y2014_WEIGHT > 0)
                                    headerLine += "2014_WEIGHT,2014_VPT,";
                                if (Item.Y2015_WEIGHT > 0)
                                    headerLine += "2015_WEIGHT,2015_VPT,";
                                if (Item.Y2016_WEIGHT > 0)
                                    headerLine += "2016_WEIGHT,2016_VPT,";
                                if (Item.Y2017_WEIGHT > 0)
                                    headerLine += "2017_WEIGHT,2017_VPT,";
                                if (Item.Y2018_WEIGHT > 0)
                                    headerLine += "2018_WEIGHT,2018_VPT,";
                                if (Item.Y2019_WEIGHT > 0)
                                    headerLine += "2019_WEIGHT,2019_VPT,";

                                if (Item.Y2020_WEIGHT > 0)
                                    headerLine += "2020_WEIGHT,2020_VPT,";
                                if (Item.Y2021_WEIGHT > 0)
                                    headerLine += "2021_WEIGHT,2021_VPT,";
                                if (Item.Y2022_WEIGHT > 0)
                                    headerLine += "2022_WEIGHT,2022_VPT,";
                                if (Item.Y2023_WEIGHT > 0)
                                    headerLine += "2023_WEIGHT,2023_VPT,";
                                if (Item.Y2024_WEIGHT > 0)
                                    headerLine += "2024_WEIGHT,2024_VPT,";
                                if (Item.Y2025_WEIGHT > 0)
                                    headerLine += "2025_WEIGHT,2025_VPT,";
                                if (Item.Y2026_WEIGHT > 0)
                                    headerLine += "2026_WEIGHT,2026_VPT,";
                                if (Item.Y2027_WEIGHT > 0)
                                    headerLine += "2027_WEIGHT,2027_VPT,";
                                if (Item.Y2028_WEIGHT > 0)
                                    headerLine += "2028_WEIGHT,2028_VPT,";
                                if (Item.Y2029_WEIGHT > 0)
                                    headerLine += "2029_WEIGHT,2029_VPT,";
                                if (Item.Y2030_WEIGHT > 0)
                                    headerLine += "2030_WEIGHT,2030_VPT,";

                                if (Item.Y1990_WEIGHT > 0)
                                    outLine += Item.Y1990_WEIGHT + "," + Item.Y1990_VPT + ",";
                                if (Item.Y1991_WEIGHT > 0)
                                    outLine += Item.Y1991_WEIGHT + "," + Item.Y1991_VPT + ",";
                                if (Item.Y1992_WEIGHT > 0)
                                    outLine += Item.Y1992_WEIGHT + "," + Item.Y1992_VPT + ",";
                                if (Item.Y1993_WEIGHT > 0)
                                    outLine += Item.Y1993_WEIGHT + "," + Item.Y1993_VPT + ",";
                                if (Item.Y1994_WEIGHT > 0)
                                    outLine += Item.Y1994_WEIGHT + "," + Item.Y1994_VPT + ",";
                                if (Item.Y1995_WEIGHT > 0)
                                    outLine += Item.Y1995_WEIGHT + "," + Item.Y1995_VPT + ",";
                                if (Item.Y1996_WEIGHT > 0)
                                    outLine += Item.Y1996_WEIGHT + "," + Item.Y1996_VPT + ",";
                                if (Item.Y1997_WEIGHT > 0)
                                    outLine += Item.Y1997_WEIGHT + "," + Item.Y1997_VPT + ",";
                                if (Item.Y1998_WEIGHT > 0)
                                    outLine += Item.Y1998_WEIGHT + "," + Item.Y1998_VPT + ",";
                                if (Item.Y1999_WEIGHT > 0)
                                    outLine += Item.Y1999_WEIGHT + "," + Item.Y1999_VPT + ",";

                                if (Item.Y2000_WEIGHT > 0)
                                    outLine += Item.Y2000_WEIGHT + "," + Item.Y2000_VPT + ",";
                                if (Item.Y2001_WEIGHT > 0)
                                    outLine += Item.Y2001_WEIGHT + "," + Item.Y2001_VPT + ",";
                                if (Item.Y2002_WEIGHT > 0)
                                    outLine += Item.Y2002_WEIGHT + "," + Item.Y2002_VPT + ",";
                                if (Item.Y2003_WEIGHT > 0)
                                    outLine += Item.Y2003_WEIGHT + "," + Item.Y2003_VPT + ",";
                                if (Item.Y2004_WEIGHT > 0)
                                    outLine += Item.Y2004_WEIGHT + "," + Item.Y2004_VPT + ",";
                                if (Item.Y2005_WEIGHT > 0)
                                    outLine += Item.Y2005_WEIGHT + "," + Item.Y2005_VPT + ",";
                                if (Item.Y2006_WEIGHT > 0)
                                    outLine += Item.Y2006_WEIGHT + "," + Item.Y2006_VPT + ",";
                                if (Item.Y2007_WEIGHT > 0)
                                    outLine += Item.Y2007_WEIGHT + "," + Item.Y2007_VPT + ",";
                                if (Item.Y2008_WEIGHT > 0)
                                    outLine += Item.Y2008_WEIGHT + "," + Item.Y2008_VPT + ",";
                                if (Item.Y2009_WEIGHT > 0)
                                    outLine += Item.Y2009_WEIGHT + "," + Item.Y2009_VPT + ",";

                                if (Item.Y2010_WEIGHT > 0)
                                    outLine += Item.Y2010_WEIGHT + "," + Item.Y2010_VPT + ",";
                                if (Item.Y2011_WEIGHT > 0)
                                    outLine += Item.Y2011_WEIGHT + "," + Item.Y2011_VPT + ",";
                                if (Item.Y2012_WEIGHT > 0)
                                    outLine += Item.Y2012_WEIGHT + "," + Item.Y2012_VPT + ",";
                                if (Item.Y2013_WEIGHT > 0)
                                    outLine += Item.Y2013_WEIGHT + "," + Item.Y2013_VPT + ",";
                                if (Item.Y2014_WEIGHT > 0)
                                    outLine += Item.Y2014_WEIGHT + "," + Item.Y2014_VPT + ",";
                                if (Item.Y2015_WEIGHT > 0)
                                    outLine += Item.Y2015_WEIGHT + "," + Item.Y2015_VPT + ",";
                                if (Item.Y2016_WEIGHT > 0)
                                    outLine += Item.Y2016_WEIGHT + "," + Item.Y2016_VPT + ",";
                                if (Item.Y2017_WEIGHT > 0)
                                    outLine += Item.Y2017_WEIGHT + "," + Item.Y2017_VPT + ",";
                                if (Item.Y2018_WEIGHT > 0)
                                    outLine += Item.Y2018_WEIGHT + "," + Item.Y2018_VPT + ",";
                                if (Item.Y2019_WEIGHT > 0)
                                    outLine += Item.Y2019_WEIGHT + "," + Item.Y2019_VPT + ",";

                                if (Item.Y2020_WEIGHT > 0)
                                    outLine += Item.Y2020_WEIGHT + "," + Item.Y2020_VPT + ",";
                                if (Item.Y2021_WEIGHT > 0)
                                    outLine += Item.Y2021_WEIGHT + "," + Item.Y2021_VPT + ",";
                                if (Item.Y2022_WEIGHT > 0)
                                    outLine += Item.Y2022_WEIGHT + "," + Item.Y2022_VPT + ",";
                                if (Item.Y2023_WEIGHT > 0)
                                    outLine += Item.Y2023_WEIGHT + "," + Item.Y2023_VPT + ",";
                                if (Item.Y2024_WEIGHT > 0)
                                    outLine += Item.Y2024_WEIGHT + "," + Item.Y2024_VPT + ",";
                                if (Item.Y2025_WEIGHT > 0)
                                    outLine += Item.Y2025_WEIGHT + "," + Item.Y2025_VPT + ",";
                                if (Item.Y2026_WEIGHT > 0)
                                    outLine += Item.Y2026_WEIGHT + "," + Item.Y2026_VPT + ",";
                                if (Item.Y2027_WEIGHT > 0)
                                    outLine += Item.Y2027_WEIGHT + "," + Item.Y2027_VPT + ",";
                                if (Item.Y2028_WEIGHT > 0)
                                    outLine += Item.Y2028_WEIGHT + "," + Item.Y2028_VPT + ",";
                                if (Item.Y2029_WEIGHT > 0)
                                    outLine += Item.Y2029_WEIGHT + "," + Item.Y2029_VPT + ",";
                                if (Item.Y2030_WEIGHT > 0)
                                    outLine += Item.Y2030_WEIGHT + "," + Item.Y2030_VPT + ",";

                                if (cnt == 0)
                                {
                                    headerLine = headerLine.Remove(headerLine.Length - 1);
                                    writeOutList.Add(headerLine);
                                    // tw.WriteLine(headerLine);
                                }
                            }

                            if (model.ShowWeight && model.ShowWeightValueVpt && model.ShowWeightVpt)
                            {
                                if (Item.Y1990_WEIGHT > 0)
                                    headerLine += "1990_WEIGHT,1990_VPT,1990_VALUE,";
                                if (Item.Y1991_WEIGHT > 0)
                                    headerLine += "1991_WEIGHT,1991_VPT,1991_VALUE,";
                                if (Item.Y1992_WEIGHT > 0)
                                    headerLine += "1992_WEIGHT,1992_VPT,1992_VALUE,";
                                if (Item.Y1993_WEIGHT > 0)
                                    headerLine += "1993_WEIGHT,1993_VPT,1993_VALUE,";
                                if (Item.Y1994_WEIGHT > 0)
                                    headerLine += "1994_WEIGHT,1994_VPT,1994_VALUE,";
                                if (Item.Y1995_WEIGHT > 0)
                                    headerLine += "1995_WEIGHT,1995_VPT,1995_VALUE,";
                                if (Item.Y1996_WEIGHT > 0)
                                    headerLine += "1996_WEIGHT,1996_VPT,1996_VALUE,";
                                if (Item.Y1997_WEIGHT > 0)
                                    headerLine += "1997_WEIGHT,1997_VPT,1997_VALUE,";
                                if (Item.Y1998_WEIGHT > 0)
                                    headerLine += "1998_WEIGHT,1998_VPT,1998_VALUE,";
                                if (Item.Y1999_WEIGHT > 0)
                                    headerLine += "1999_WEIGHT,1999_VPT,1999_VALUE,";

                                if (Item.Y2000_WEIGHT > 0)
                                    headerLine += "2000_WEIGHT,2000_VPT,2000_VALUE,";
                                if (Item.Y2001_WEIGHT > 0)
                                    headerLine += "2001_WEIGHT,2001_VPT,2001_VALUE,";
                                if (Item.Y2002_WEIGHT > 0)
                                    headerLine += "2002_WEIGHT,2002_VPT,2002_VALUE,";
                                if (Item.Y2003_WEIGHT > 0)
                                    headerLine += "2003_WEIGHT,2003_VPT,2003_VALUE,";
                                if (Item.Y2004_WEIGHT > 0)
                                    headerLine += "2004_WEIGHT,2004_VPT,2004_VALUE,";
                                if (Item.Y2005_WEIGHT > 0)
                                    headerLine += "2005_WEIGHT,2005_VPT,2005_VALUE,";
                                if (Item.Y2006_WEIGHT > 0)
                                    headerLine += "2006_WEIGHT,2006_VPT,2006_VALUE,";
                                if (Item.Y2007_WEIGHT > 0)
                                    headerLine += "2007_WEIGHT,2007_VPT,2007_VALUE,";
                                if (Item.Y2008_WEIGHT > 0)
                                    headerLine += "2008_WEIGHT,2008_VPT,2008_VALUE,";
                                if (Item.Y2009_WEIGHT > 0)
                                    headerLine += "2009_WEIGHT,2009_VPT,2009_VALUE,";

                                if (Item.Y2010_WEIGHT > 0)
                                    headerLine += "2010_WEIGHT,2010_VPT,2010_VALUE,";
                                if (Item.Y2011_WEIGHT > 0)
                                    headerLine += "2011_WEIGHT,2011_VPT,2011_VALUE,";
                                if (Item.Y2012_WEIGHT > 0)
                                    headerLine += "2012_WEIGHT,2012_VPT,2012_VALUE,";
                                if (Item.Y2013_WEIGHT > 0)
                                    headerLine += "2013_WEIGHT,2013_VPT,2013_VALUE,";
                                if (Item.Y2014_WEIGHT > 0)
                                    headerLine += "2014_WEIGHT,2014_VPT,2014_VALUE,";
                                if (Item.Y2015_WEIGHT > 0)
                                    headerLine += "2015_WEIGHT,2015_VPT,2015_VALUE,";
                                if (Item.Y2016_WEIGHT > 0)
                                    headerLine += "2016_WEIGHT,2016_VPT,2016_VALUE,";
                                if (Item.Y2017_WEIGHT > 0)
                                    headerLine += "2017_WEIGHT,2017_VPT,2017_VALUE,";
                                if (Item.Y2018_WEIGHT > 0)
                                    headerLine += "2018_WEIGHT,2018_VPT,2018_VALUE,";
                                if (Item.Y2019_WEIGHT > 0)
                                    headerLine += "2019_WEIGHT,2019_VPT,2019_VALUE,";

                                if (Item.Y2020_WEIGHT > 0)
                                    headerLine += "2020_WEIGHT,2020_VPT,2020_VALUE,";
                                if (Item.Y2021_WEIGHT > 0)
                                    headerLine += "2021_WEIGHT,2021_VPT,2021_VALUE,";
                                if (Item.Y2022_WEIGHT > 0)
                                    headerLine += "2022_WEIGHT,2022_VPT,2022_VALUE,";
                                if (Item.Y2023_WEIGHT > 0)
                                    headerLine += "2023_WEIGHT,2023_VPT,2023_VALUE,";
                                if (Item.Y2024_WEIGHT > 0)
                                    headerLine += "2024_WEIGHT,2024_VPT,2024_VALUE,";
                                if (Item.Y2025_WEIGHT > 0)
                                    headerLine += "2025_WEIGHT,2025_VPT,2025_VALUE,";
                                if (Item.Y2026_WEIGHT > 0)
                                    headerLine += "2026_WEIGHT,2026_VPT,2026_VALUE,";
                                if (Item.Y2027_WEIGHT > 0)
                                    headerLine += "2027_WEIGHT,2027_VPT,2027_VALUE,";
                                if (Item.Y2028_WEIGHT > 0)
                                    headerLine += "2028_WEIGHT,2028_VPT,2028_VALUE,";
                                if (Item.Y2029_WEIGHT > 0)
                                    headerLine += "2029_WEIGHT,2029_VPT,2029_VALUE,";
                                if (Item.Y2030_WEIGHT > 0)
                                    headerLine += "2030_WEIGHT,2030_VPT,2030_VALUE,";

                                if (Item.Y1990_WEIGHT > 0)
                                    outLine += Item.Y1990_WEIGHT + "," + Item.Y1990_VPT + "," + Item.Y1990_VALUE + ",";
                                if (Item.Y1991_WEIGHT > 0)
                                    outLine += Item.Y1991_WEIGHT + "," + Item.Y1991_VPT + "," + Item.Y1991_VALUE + ",";
                                if (Item.Y1992_WEIGHT > 0)
                                    outLine += Item.Y1992_WEIGHT + "," + Item.Y1992_VPT + "," + Item.Y1992_VALUE + ",";
                                if (Item.Y1993_WEIGHT > 0)
                                    outLine += Item.Y1993_WEIGHT + "," + Item.Y1993_VPT + "," + Item.Y1993_VALUE + ",";
                                if (Item.Y1994_WEIGHT > 0)
                                    outLine += Item.Y1994_WEIGHT + "," + Item.Y1994_VPT + "," + Item.Y1994_VALUE + ",";
                                if (Item.Y1995_WEIGHT > 0)
                                    outLine += Item.Y1995_WEIGHT + "," + Item.Y1995_VPT + "," + Item.Y1995_VALUE + ",";
                                if (Item.Y1996_WEIGHT > 0)
                                    outLine += Item.Y1996_WEIGHT + "," + Item.Y1996_VPT + "," + Item.Y1996_VALUE + ",";
                                if (Item.Y1997_WEIGHT > 0)
                                    outLine += Item.Y1997_WEIGHT + "," + Item.Y1997_VPT + "," + Item.Y1997_VALUE + ",";
                                if (Item.Y1998_WEIGHT > 0)
                                    outLine += Item.Y1998_WEIGHT + "," + Item.Y1998_VPT + "," + Item.Y1998_VALUE + ",";
                                if (Item.Y1999_WEIGHT > 0)
                                    outLine += Item.Y1999_WEIGHT + "," + Item.Y1999_VPT + "," + Item.Y1999_VALUE + ",";

                                if (Item.Y2000_WEIGHT > 0)
                                    outLine += Item.Y2000_WEIGHT + "," + Item.Y2000_VPT + "," + Item.Y2000_VALUE + ",";
                                if (Item.Y2001_WEIGHT > 0)
                                    outLine += Item.Y2001_WEIGHT + "," + Item.Y2001_VPT + "," + Item.Y2001_VALUE + ",";
                                if (Item.Y2002_WEIGHT > 0)
                                    outLine += Item.Y2002_WEIGHT + "," + Item.Y2002_VPT + "," + Item.Y2002_VALUE + ",";
                                if (Item.Y2003_WEIGHT > 0)
                                    outLine += Item.Y2003_WEIGHT + "," + Item.Y2003_VPT + "," + Item.Y2003_VALUE + ",";
                                if (Item.Y2004_WEIGHT > 0)
                                    outLine += Item.Y2004_WEIGHT + "," + Item.Y2004_VPT + "," + Item.Y2004_VALUE + ",";
                                if (Item.Y2005_WEIGHT > 0)
                                    outLine += Item.Y2005_WEIGHT + "," + Item.Y2005_VPT + "," + Item.Y2005_VALUE + ",";
                                if (Item.Y2006_WEIGHT > 0)
                                    outLine += Item.Y2006_WEIGHT + "," + Item.Y2006_VPT + "," + Item.Y2006_VALUE + ",";
                                if (Item.Y2007_WEIGHT > 0)
                                    outLine += Item.Y2007_WEIGHT + "," + Item.Y2007_VPT + "," + Item.Y2007_VALUE + ",";
                                if (Item.Y2008_WEIGHT > 0)
                                    outLine += Item.Y2008_WEIGHT + "," + Item.Y2008_VPT + "," + Item.Y2008_VALUE + ",";
                                if (Item.Y2009_WEIGHT > 0)
                                    outLine += Item.Y2009_WEIGHT + "," + Item.Y2009_VPT + "," + Item.Y2009_VALUE + ",";

                                if (Item.Y2010_WEIGHT > 0)
                                    outLine += Item.Y2010_WEIGHT + "," + Item.Y2010_VPT + "," + Item.Y2010_VALUE + ",";
                                if (Item.Y2011_WEIGHT > 0)
                                    outLine += Item.Y2011_WEIGHT + "," + Item.Y2011_VPT + "," + Item.Y2011_VALUE + ",";
                                if (Item.Y2012_WEIGHT > 0)
                                    outLine += Item.Y2012_WEIGHT + "," + Item.Y2012_VPT + "," + Item.Y2012_VALUE + ",";
                                if (Item.Y2013_WEIGHT > 0)
                                    outLine += Item.Y2013_WEIGHT + "," + Item.Y2013_VPT + "," + Item.Y2013_VALUE + ",";
                                if (Item.Y2014_WEIGHT > 0)
                                    outLine += Item.Y2014_WEIGHT + "," + Item.Y2014_VPT + "," + Item.Y2014_VALUE + ",";
                                if (Item.Y2015_WEIGHT > 0)
                                    outLine += Item.Y2015_WEIGHT + "," + Item.Y2015_VPT + "," + Item.Y2015_VALUE + ",";
                                if (Item.Y2016_WEIGHT > 0)
                                    outLine += Item.Y2016_WEIGHT + "," + Item.Y2016_VPT + "," + Item.Y2016_VALUE + ",";
                                if (Item.Y2017_WEIGHT > 0)
                                    outLine += Item.Y2017_WEIGHT + "," + Item.Y2017_VPT + "," + Item.Y2017_VALUE + ",";
                                if (Item.Y2018_WEIGHT > 0)
                                    outLine += Item.Y2018_WEIGHT + "," + Item.Y2018_VPT + "," + Item.Y2018_VALUE + ",";
                                if (Item.Y2019_WEIGHT > 0)
                                    outLine += Item.Y2019_WEIGHT + "," + Item.Y2019_VPT + "," + Item.Y2019_VALUE + ",";

                                if (Item.Y2020_WEIGHT > 0)
                                    outLine += Item.Y2020_WEIGHT + "," + Item.Y2020_VPT + "," + Item.Y2020_VALUE + ",";
                                if (Item.Y2021_WEIGHT > 0)
                                    outLine += Item.Y2021_WEIGHT + "," + Item.Y2021_VPT + "," + Item.Y2021_VALUE + ",";
                                if (Item.Y2022_WEIGHT > 0)
                                    outLine += Item.Y2022_WEIGHT + "," + Item.Y2022_VPT + "," + Item.Y2022_VALUE + ",";
                                if (Item.Y2023_WEIGHT > 0)
                                    outLine += Item.Y2023_WEIGHT + "," + Item.Y2023_VPT + "," + Item.Y2023_VALUE + ",";
                                if (Item.Y2024_WEIGHT > 0)
                                    outLine += Item.Y2024_WEIGHT + "," + Item.Y2024_VPT + "," + Item.Y2024_VALUE + ",";
                                if (Item.Y2025_WEIGHT > 0)
                                    outLine += Item.Y2025_WEIGHT + "," + Item.Y2025_VPT + "," + Item.Y2025_VALUE + ",";
                                if (Item.Y2026_WEIGHT > 0)
                                    outLine += Item.Y2026_WEIGHT + "," + Item.Y2026_VPT + "," + Item.Y2026_VALUE + ",";
                                if (Item.Y2027_WEIGHT > 0)
                                    outLine += Item.Y2027_WEIGHT + "," + Item.Y2027_VPT + "," + Item.Y2027_VALUE + ",";
                                if (Item.Y2028_WEIGHT > 0)
                                    outLine += Item.Y2028_WEIGHT + "," + Item.Y2028_VPT + "," + Item.Y2028_VALUE + ",";
                                if (Item.Y2029_WEIGHT > 0)
                                    outLine += Item.Y2029_WEIGHT + "," + Item.Y2029_VPT + "," + Item.Y2029_VALUE + ",";
                                if (Item.Y2030_WEIGHT > 0)
                                    outLine += Item.Y2030_WEIGHT + "," + Item.Y2030_VPT + "," + Item.Y2030_VALUE + ",";

                                if (cnt == 0)
                                {
                                    headerLine = headerLine.Remove(headerLine.Length - 1);
                                    writeOutList.Add(headerLine);
                                    //tw.WriteLine(headerLine);
                                }
                            }

                            if (writeOutList.Count > 0)
                            {
                                if (cnt > 0)
                                    headerLine = headerLine.Remove(headerLine.Length - 1);

                                if (headerLine.Length > writeOutList[0].Length)
                                {
                                    writeOutList[0] = headerLine;
                                }
                            }

                            outLine = outLine.Remove(outLine.Length - 1);
                            writeOutList.Add(outLine);
                            // tw.WriteLine(outLine);
                            cnt++;
                            Total++;
                        }

                        foreach (var Item in writeOutList)
                            tw.WriteLine(Item);


                        tw.Flush();

                        var FileName = model.FileName + ".csv";

                        var length = ms.Length;
                        tw.Close();
                        var toWrite = new byte[length];
                        Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
                        ms.Close();
                        return File(toWrite, "text/plain", FileName);
                    }
                }

                if (keyData.Search.GroupByQuarter && !keyData.Search.GroupByMonth && bIgnore == false)
                {
                    var OutSrv = PivotHelper.Instance;
                    var outList = OutSrv.GroupByQuarter(fList, reportByQuarterWeight, reportByQuarterValue, keyData);

                    if (model.FileType.Equals("TXT"))
                    {

                        MemoryStream ms = new MemoryStream();
                        TextWriter tw = new StreamWriter(ms);
                        int cnt = 0, Total = 0;
                        var writeOutList = new List<string>();

                        Total++;
                        foreach (var Item in outList)
                        {
                            var outLine = string.Empty;
                            var headerLine = string.Empty;

                            headerLine += "ID,";
                            outLine = Item.ID.ToString() + ",";

                            foreach (var kItem in fList)
                            {
                                if (kItem.Equals("SC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO))
                                    {
                                        headerLine += "SC_GEO,";
                                        outLine += Item.SC_GEO + ",";
                                    }
                                }
                                if (kItem.Equals("SC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_NAME))
                                    {
                                        headerLine += "SC_NAME,";
                                        outLine += Item.SC_NAME + ",";
                                    }
                                }
                                if (kItem.Equals("SC_GEO_REGION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SC_GEO_REGION))
                                    {
                                        headerLine += "SC_GEO_REGION,";
                                        outLine += Item.SC_GEO_REGION + ",";
                                    }
                                }
                                if (kItem.Equals("MC_GEO"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_GEO))
                                    {
                                        headerLine += "MC_GEO,";
                                        outLine += Item.MC_GEO + ",";
                                    }
                                }

                                if (kItem.Equals("MC_NAME"))
                                {
                                    if (!string.IsNullOrEmpty(Item.MC_NAME))
                                    {
                                        headerLine += "MC_NAME,";
                                        outLine += Item.MC_NAME + ",";
                                    }
                                }

                                if (kItem.Equals("TARIFF_CODE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {
                                        headerLine += "TARIFF_CODE,";
                                        outLine += Item.TARIFF_CODE + ",";
                                    }
                                }

                                if (kItem.Equals("TARIFF_DESCRIPTION"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                    {
                                        headerLine += "TARIFF_DESCRIPTION,";
                                        outLine += Item.TARIFF_LEGEND + ",";
                                    }
                                }

                                if (kItem.Equals("TWO_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TWO_DIGIT))
                                    {
                                        headerLine += "TWO_DIGIT,";
                                        outLine += Item.TWO_DIGIT + ",";
                                    }
                                }

                                if (kItem.Equals("SIX_DIGIT"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIX_DIGIT))
                                    {
                                        headerLine += "SIX_DIGIT,";
                                        outLine += Item.SIX_DIGIT + ",";
                                    }
                                }

                                if (kItem.Equals("TARIFF_CATEGORY"))
                                {
                                    if (!string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                                    {
                                        headerLine += "TARIFF_CATEGORY,";
                                        outLine += Item.TARIFF_CATEGORY + ",";
                                    }
                                }

                                if (kItem.Equals("SIDE_OF_TRADE"))
                                {
                                    if (!string.IsNullOrEmpty(Item.SIDE_OF_TRADE))
                                    {
                                        headerLine += "SIDE_OF_TRADE,";
                                        outLine += Item.SIDE_OF_TRADE + ",";
                                    }
                                }
                                if (kItem.Equals("YEAR"))
                                {
                                    if (Item.YEAR > 0)
                                    {
                                        headerLine += "YEAR,";
                                        outLine += Item.YEAR + ",";
                                    }
                                }
                                //if (kItem.Equals("QUARTER"))
                                //{
                                //    if (Item.QUARTER > 0)
                                //    {
                                //        headerLine += "QUARTER,";
                                //        outLine += Item.QUARTER + ",";
                                //    }
                                //}
                            }

                            if (model.ShowWeight && !model.ShowWeightValueVpt && !model.ShowWeightVpt)
                            {
                                if (Item.Q1_WEIGHT > 0)
                                    headerLine += "Q1_WEIGHT,";
                                if (Item.Q2_WEIGHT > 0)
                                    headerLine += "Q2_WEIGHT,";
                                if (Item.Q3_WEIGHT > 0)
                                    headerLine += "Q3_WEIGHT,";
                                if (Item.Q4_WEIGHT > 0)
                                    headerLine += "Q4_WEIGHT,";

                                if (Item.Q1_WEIGHT > 0)
                                    outLine += Item.Q1_WEIGHT + ",";
                                if (Item.Q2_WEIGHT > 0)
                                    outLine += Item.Q2_WEIGHT + ",";
                                if (Item.Q3_WEIGHT > 0)
                                    outLine += Item.Q3_WEIGHT + ",";
                                if (Item.Q4_WEIGHT > 0)
                                    outLine += Item.Q4_WEIGHT + ",";


                                if (cnt == 0)
                                {
                                    headerLine = headerLine.Remove(headerLine.Length - 1);
                                    writeOutList.Add(headerLine);
                                }
                            }

                            if (model.ShowWeight && !model.ShowWeightValueVpt && model.ShowWeightVpt)
                            {
                                if (Item.Q1_WEIGHT > 0)
                                    headerLine += "Q1_WEIGHT,Q1_VPT,";
                                if (Item.Q2_WEIGHT > 0)
                                    headerLine += "Q2_WEIGHT,Q2_VPT,";
                                if (Item.Q3_WEIGHT > 0)
                                    headerLine += "Q3_WEIGHT,Q3_VPT,";
                                if (Item.Q4_WEIGHT > 0)
                                    headerLine += "Q4_WEIGHT,Q4_VPT,";

                                if (Item.Q1_WEIGHT > 0)
                                    outLine += Item.Q1_WEIGHT + "," + Item.Q1_VPT + ",";
                                if (Item.Q2_WEIGHT > 0)
                                    outLine += Item.Q2_WEIGHT + "," + Item.Q2_VPT + ",";
                                if (Item.Q3_WEIGHT > 0)
                                    outLine += Item.Q3_WEIGHT + "," + Item.Q3_VPT + ",";
                                if (Item.Q4_WEIGHT > 0)
                                    outLine += Item.Q4_WEIGHT + "," + Item.Q4_VPT + ",";


                                if (cnt == 0)
                                {
                                    headerLine = headerLine.Remove(headerLine.Length - 1);
                                    writeOutList.Add(headerLine);
                                }
                            }

                            if (model.ShowWeight && model.ShowWeightValueVpt && model.ShowWeightVpt)
                            {
                                if (Item.Q1_WEIGHT > 0)
                                    headerLine += "Q1_WEIGHT,Q1_VPT,Q1_VALUE,";
                                if (Item.Q2_WEIGHT > 0)
                                    headerLine += "Q2_WEIGHT,Q2_VPT,Q2_VALUE,";
                                if (Item.Q3_WEIGHT > 0)
                                    headerLine += "Q3_WEIGHT,Q3_VPT,Q3_VALUE,";
                                if (Item.Q4_WEIGHT > 0)
                                    headerLine += "Q4_WEIGHT,Q4_VPT,Q4_VALUE,";

                                if (Item.Q1_WEIGHT > 0)
                                    outLine += Item.Q1_WEIGHT + "," + Item.Q1_VPT + "," + Item.Q1_VALUE + ",";
                                if (Item.Q2_WEIGHT > 0)
                                    outLine += Item.Q2_WEIGHT + "," + Item.Q2_VPT + "," + Item.Q2_VALUE + ",";
                                if (Item.Q3_WEIGHT > 0)
                                    outLine += Item.Q3_WEIGHT + "," + Item.Q3_VPT + "," + Item.Q3_VALUE + ",";
                                if (Item.Q4_WEIGHT > 0)
                                    outLine += Item.Q4_WEIGHT + "," + Item.Q4_VPT + "," + Item.Q4_VALUE + ",";


                                if (cnt == 0)
                                {
                                    headerLine = headerLine.Remove(headerLine.Length - 1);
                                    writeOutList.Add(headerLine);
                                }
                            }

                            if (writeOutList.Count > 0)
                            {
                                if (cnt > 0)
                                    headerLine = headerLine.Remove(headerLine.Length - 1);

                                if (headerLine.Length > writeOutList[0].Length)
                                {
                                    writeOutList[0] = headerLine;
                                }
                            }

                            outLine = outLine.Remove(outLine.Length - 1);
                            writeOutList.Add(outLine);

                            cnt++;
                            Total++;
                        }

                        foreach (var Item in writeOutList)
                            tw.WriteLine(Item);

                        tw.Flush();

                        var FileName = model.FileName + ".csv";

                        var length = ms.Length;
                        tw.Close();
                        var toWrite = new byte[length];
                        Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
                        ms.Close();
                        return File(toWrite, "text/plain", FileName);
                    }
                }


                if (keyData.Search.GroupByMonth && !keyData.Search.GroupByQuarter && bIgnore == false)
                {

                    var OutSrv = PivotHelper.Instance;
                    var outList = OutSrv.GroupByMonth(fList, reportByMonthWeight, reportByMonthValue, keyData);

                    bool bByMonthHorizontal = true;
                    var tmpOutList = new List<string>();

                    foreach (var Item in keyData.Search.Dates)
                    {
                        var sd = Item.ToString().Substring(0, 4);
                        tmpOutList.Add(sd);
                    }

                    var distinctYears = new List<string>(tmpOutList.Distinct());

                    var tmpList = new List<ReportWriterByMonthOutModel>();

                    foreach (var Item in outList)
                    {
                        var yearslist = distinctYears.OrderBy(p => p.Substring(0)).ToList();


                        foreach (var Year in yearslist)
                        {
                            var findModel = outList.Where(x =>
                            x.SC_GEO == Item.SC_GEO
                            && x.MC_GEO == Item.MC_GEO
                            && x.TARIFF_CODE == Item.TARIFF_CODE
                            && x.SIDE_OF_TRADE == Item.SIDE_OF_TRADE
                            && x.KEY.Contains(Year)).ToList();

                            if (findModel.Count == 0)
                            {
                                string[] words = Item.KEY.Split('|');
                                words[7] = Year;
                                string NewKey = string.Empty;
                                foreach (var key in words)
                                {
                                    NewKey += key + "|";
                                }
                                NewKey = NewKey.Remove(NewKey.Length - 1);

                                var newInsertModel = new ReportWriterByMonthOutModel
                                {
                                    APR_VALUE = 0,
                                    APR_VPT = 0,
                                    APR_WEIGHT = 0,
                                    AUG_VPT = 0,
                                    AUG_VALUE = 0,
                                    AUG_WEIGHT = 0,
                                    DEC_VALUE = 0,
                                    DEC_VPT = 0,
                                    DEC_WEIGHT = 0,
                                    FEB_VALUE = 0,
                                    FEB_VPT = 0,
                                    FEB_WEIGHT = 0,
                                    JAN_VALUE = 0,
                                    ID = 0,
                                    JAN_VPT = 0,
                                    JAN_WEIGHT = 0,
                                    JLY_VALUE = 0,
                                    JLY_VPT = 0,
                                    JLY_WEIGHT = 0,
                                    JUN_VALUE = 0,
                                    JUN_VPT = 0,
                                    JUN_WEIGHT = 0,
                                    MAR_VALUE = 0,
                                    MAR_VPT = 0,
                                    MAR_WEIGHT = 0,
                                    MAY_VALUE = 0,
                                    MAY_VPT = 0,
                                    MAY_WEIGHT = 0,
                                    NOV_VALUE = 0,
                                    NOV_VPT = 0,
                                    NOV_WEIGHT = 0,
                                    OCT_VALUE = 0,
                                    OCT_VPT = 0,
                                    OCT_WEIGHT = 0,
                                    PORT_ID = 0,
                                    PORT_NAME = string.Empty,
                                    QUARTER = Item.QUARTER,
                                    SEP_VALUE = 0,
                                    SEP_VPT = 0,
                                    SEP_WEIGHT = 0,
                                    SIX_DIGIT = Item.SIX_DIGIT,
                                    TARIFF_CATEGORY = Item.TARIFF_CATEGORY,
                                    TARIFF_CODE = Item.TARIFF_CODE,
                                    TARIFF_LEGEND = Item.TARIFF_LEGEND,
                                    TIME_ID = 0,
                                    TOTAL_VALUE = 0,
                                    TOTAL_VPT = 0,
                                    TOTAL_WEIGHT = 0,
                                    TWO_DIGIT = Item.TWO_DIGIT,
                                    YEAR = int.Parse(Year),

                                    KEY = NewKey,
                                    SC_GEO = Item.SC_GEO,
                                    SC_GEO_REGION = Item.SC_GEO_REGION,
                                    SC_NAME = Item.SC_NAME,
                                    MC_GEO = Item.MC_GEO,
                                    MC_NAME = Item.MC_NAME,
                                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,

                                };


                                if (int.Parse(Year) == newInsertModel.YEAR)
                                {
                                    newInsertModel.YEAR = int.Parse(Year);

                                    tmpList.Remove(newInsertModel);
                                    tmpList.Add(newInsertModel);

                                }
                            }
                            else
                            {
                                tmpList.Remove(Item);
                                tmpList.Add(Item);

                            }

                        }

                    }

                    var dbList = new List<ReportCacheModel>();
                    foreach (var ItemC in tmpList)
                    {
                        var cacheModel = new ReportCacheModel
                        {
                            Key = ItemC.KEY,
                            ReportID = model.ReportID,
                            Data = ItemC
                        };
                        dbList.Add(cacheModel);
                    }

                    tmpList = new List<ReportWriterByMonthOutModel>();


                    if (bByMonthHorizontal)
                    {
                        var ExportList = new List<CrossTabMultiMonthModel>();
                        var ImportList = new List<CrossTabMultiMonthModel>();
                        var NewKey = string.Empty;
                        var outListByHor = outList.GroupBy(x => x.YEAR).ToList();

                        outListByHor = outListByHor.OrderBy(x => x.Key).ToList();

                        if (model.FileType.Equals("TXT"))
                        {

                            var outLine = string.Empty;
                            var headerLine = string.Empty;
                            var SearchKey = string.Empty;
                            var LastKey = string.Empty;

                            var searchList = new List<SearchPivotModel>();

                            foreach (var keyItem in outListByHor)
                            {


                                foreach (var Item in keyItem)
                                {
                                    if (keyItem.Key != Item.YEAR)
                                        break;


                                    SearchKey = string.Empty;
                                    foreach (var kItem in fList)
                                    {
                                        if (kItem.Equals("SC_GEO"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SC_GEO))
                                            {
                                                SearchKey += Item.SC_GEO + "|";
                                            }
                                        }
                                        if (kItem.Equals("SC_NAME"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SC_NAME))
                                            {

                                                SearchKey += Item.SC_NAME + "|";
                                            }
                                        }
                                        if (kItem.Equals("SC_GEO_REGION"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SC_GEO_REGION))
                                            {

                                                SearchKey += Item.SC_GEO_REGION + "|";
                                            }
                                        }
                                        if (kItem.Equals("MC_GEO"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.MC_GEO))
                                            {

                                                SearchKey += Item.MC_GEO + "|";
                                            }
                                        }

                                        if (kItem.Equals("MC_NAME"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.MC_NAME))
                                            {

                                                SearchKey += Item.MC_NAME + "|";
                                            }
                                        }

                                        if (kItem.Equals("PORT"))
                                        {

                                            SearchKey += Item.PORT_ID + "|";

                                        }

                                        if (kItem.Equals("PORT_NAME"))
                                        {

                                            SearchKey += Item.PORT_NAME + "|";

                                        }

                                        if (kItem.Equals("TARIFF_CODE"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                            {

                                                SearchKey += Item.TARIFF_CODE + "|";
                                            }
                                        }

                                        if (kItem.Equals("TARIFF_DESCRIPTION"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                            {

                                                SearchKey += Item.TARIFF_LEGEND + "|";
                                            }
                                        }

                                        if (kItem.Equals("TWO_DIGIT"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.TWO_DIGIT))
                                            {

                                                SearchKey += Item.TWO_DIGIT + "|";
                                            }
                                        }

                                        if (kItem.Equals("SIX_DIGIT"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SIX_DIGIT))
                                            {
                                                SearchKey += Item.SIX_DIGIT + "|";
                                            }
                                        }

                                        if (kItem.Equals("TARIFF_CATEGORY"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                                            {
                                                SearchKey += Item.TARIFF_CATEGORY + "|";
                                            }
                                        }


                                        if (kItem.Equals("SIDE_OF_TRADE"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SIDE_OF_TRADE))
                                            {

                                                SearchKey += Item.SIDE_OF_TRADE + "|";
                                            }
                                        }
                                    }


                                    headerLine = string.Empty;
                                    outLine = string.Empty;

                                    foreach (var kItem in fList)
                                    {
                                        if (kItem.Equals("SC_GEO"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SC_GEO))
                                            {
                                                headerLine += "SC_GEO,";
                                                outLine += Item.SC_GEO + ",";

                                            }
                                        }
                                        if (kItem.Equals("SC_NAME"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SC_NAME))
                                            {
                                                headerLine += "SC_NAME,";
                                                outLine += Item.SC_NAME + ",";

                                            }
                                        }
                                        if (kItem.Equals("SC_GEO_REGION"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SC_GEO_REGION))
                                            {
                                                headerLine += "SC_GEO_REGION,";
                                                outLine += Item.SC_GEO_REGION + ",";

                                            }
                                        }
                                        if (kItem.Equals("MC_GEO"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.MC_GEO))
                                            {
                                                headerLine += "MC_GEO,";
                                                outLine += Item.MC_GEO + ",";

                                            }
                                        }

                                        if (kItem.Equals("MC_NAME"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.MC_NAME))
                                            {
                                                headerLine += "MC_NAME,";
                                                outLine += Item.MC_NAME + ",";

                                            }
                                        }

                                        if (kItem.Equals("PORT"))
                                        {

                                            headerLine += "PORT,";
                                            outLine += Item.PORT_ID + ",";


                                        }

                                        if (kItem.Equals("PORT_NAME"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.PORT_NAME))
                                            {
                                                headerLine += "PORT_NAME,";
                                                outLine += Item.PORT_NAME + ",";

                                            }
                                        }

                                        if (kItem.Equals("TARIFF_CODE"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                            {
                                                headerLine += "TARIFF_CODE,";
                                                outLine += Item.TARIFF_CODE + ",";

                                            }
                                        }

                                        if (kItem.Equals("TARIFF_DESCRIPTION"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                            {
                                                headerLine += "TARIFF_DESCRIPTION,";
                                                outLine += Item.TARIFF_LEGEND + ",";

                                            }
                                        }

                                        if (kItem.Equals("TWO_DIGIT"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.TWO_DIGIT))
                                            {
                                                headerLine += "TWO_DIGIT,";
                                                outLine += Item.TWO_DIGIT + ",";

                                            }
                                        }

                                        if (kItem.Equals("SIX_DIGIT"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SIX_DIGIT))
                                            {
                                                headerLine += "SIX_DIGIT,";
                                                outLine += Item.SIX_DIGIT + ",";

                                            }
                                        }

                                        if (kItem.Equals("TARIFF_CATEGORY"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                                            {
                                                headerLine += "TARIFF_CATEGORY,";
                                                outLine += Item.TARIFF_CATEGORY + ",";

                                            }
                                        }

                                        if (kItem.Equals("SIDE_OF_TRADE"))
                                        {
                                            if (!string.IsNullOrEmpty(Item.SIDE_OF_TRADE))
                                            {
                                                headerLine += "SIDE_OF_TRADE,";
                                                outLine += Item.SIDE_OF_TRADE + ",";

                                            }
                                        }

                                    }

                                    var hearderConcat = string.Empty;
                                    var dataConcat = string.Empty;
                                    if (model.ShowWeight && !model.ShowWeightValueVpt && !model.ShowWeightVpt)
                                    {
                                        int iResult = searchList.FindIndex(x => x.Key.Equals(SearchKey) && x.Year == Item.YEAR);

                                        if (iResult == -1)
                                        {
                                            if (Item.JAN_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JAN_WEIGHT,";
                                                outLine += Item.JAN_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " JAN_WEIGHT,";
                                                dataConcat += Item.JAN_WEIGHT + ",";
                                            }

                                            if (Item.FEB_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " FEB_WEIGHT,";
                                                outLine += Item.FEB_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " FEB_WEIGHT,";
                                                dataConcat += Item.FEB_WEIGHT + ",";
                                            }
                                            if (Item.MAR_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " MAR_WEIGHT,";
                                                outLine += Item.MAR_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " MAR_WEIGHT,";
                                                dataConcat += Item.MAR_WEIGHT + ",";
                                            }
                                            if (Item.APR_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " APR_WEIGHT,";
                                                outLine += Item.APR_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " APR_WEIGHT,";
                                                dataConcat += Item.APR_WEIGHT + ",";
                                            }
                                            if (Item.MAY_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " MAY_WEIGHT,";
                                                outLine += Item.MAY_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " MAY_WEIGHT,";
                                                dataConcat += Item.MAY_WEIGHT + ",";
                                            }
                                            if (Item.JUN_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JUN_WEIGHT,";
                                                outLine += Item.JUN_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " JUN_WEIGHT,";
                                                dataConcat += Item.JUN_WEIGHT + ",";
                                            }
                                            if (Item.JLY_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JLY_WEIGHT,";
                                                outLine += Item.JLY_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " JLY_WEIGHT,";
                                                dataConcat += Item.JLY_WEIGHT + ",";
                                            }
                                            if (Item.AUG_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + "AUG_WEIGHT,";
                                                outLine += Item.AUG_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " AUG_WEIGHT,";
                                                dataConcat += Item.AUG_WEIGHT + ",";
                                            }
                                            if (Item.SEP_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " SEP_WEIGHT,";
                                                outLine += Item.SEP_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " SEP_WEIGHT,";
                                                dataConcat += Item.SEP_WEIGHT + ",";
                                            }
                                            if (Item.OCT_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " OCT_WEIGHT,";
                                                outLine += Item.OCT_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " OCT_WEIGHT,";
                                                dataConcat += Item.OCT_WEIGHT + ",";
                                            }
                                            if (Item.NOV_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " NOV_WEIGHT,";
                                                outLine += Item.NOV_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " NOV_WEIGHT,";
                                                dataConcat += Item.NOV_WEIGHT + ",";
                                            }
                                            if (Item.DEC_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " DEC_WEIGHT,";
                                                outLine += Item.DEC_WEIGHT + ",";
                                                hearderConcat += Item.YEAR + " DEC_WEIGHT,";
                                                dataConcat += Item.DEC_WEIGHT + ",";
                                            }
                                        }

                                        var searchModel = new SearchPivotModel { Key = SearchKey, Year = Item.YEAR };
                                        searchList.Add(searchModel);

                                    }

                                    if (model.ShowWeight && !model.ShowWeightValueVpt && model.ShowWeightVpt)
                                    {
                                        int iResult = searchList.FindIndex(x => x.Key.Equals(SearchKey) && x.Year == Item.YEAR);

                                        if (iResult == -1)
                                        {
                                            if (Item.JAN_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JAN_WEIGHT,VPT,";
                                                outLine += Item.JAN_WEIGHT + "," + Item.JAN_VPT + ",";
                                                hearderConcat += Item.YEAR + " JAN_WEIGHT,VPT,";
                                                dataConcat += Item.JAN_WEIGHT + "," + Item.JAN_VPT + ",";
                                            }
                                            if (Item.FEB_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " FEB_WEIGHT,VPT,";
                                                outLine += Item.FEB_WEIGHT + "," + Item.FEB_VPT + ",";
                                                hearderConcat += Item.YEAR + " FEB_WEIGHT,VPT,";
                                                dataConcat += Item.FEB_WEIGHT + "," + Item.FEB_VPT + ",";
                                            }
                                            if (Item.MAR_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " MAR_WEIGHT,VPT,";
                                                outLine += Item.MAR_WEIGHT + "," + Item.MAR_VPT + ",";
                                                hearderConcat += Item.YEAR + " MAR_WEIGHT,VPT,";
                                                dataConcat += Item.MAR_WEIGHT + "," + Item.MAR_VPT + ",";
                                            }
                                            if (Item.APR_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " APR_WEIGHT,VPT,";
                                                outLine += Item.APR_WEIGHT + "," + Item.APR_VPT + ",";
                                                hearderConcat += Item.YEAR + " APR_WEIGHT,VPT,";
                                                dataConcat += Item.APR_WEIGHT + "," + Item.APR_VPT + ",";
                                            }
                                            if (Item.MAY_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " MAY_WEIGHT,VPT,";
                                                outLine += Item.MAY_WEIGHT + "," + Item.MAY_VPT + ",";
                                                hearderConcat += Item.YEAR + " MAY_WEIGHT,VPT,";
                                                dataConcat += Item.MAY_WEIGHT + "," + Item.MAY_VPT + ",";
                                            }
                                            if (Item.JUN_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JUN_WEIGHT,VPT,";
                                                outLine += Item.JUN_WEIGHT + "," + Item.JUN_VPT + ",";
                                                hearderConcat += Item.YEAR + " JUN_WEIGHT,VPT,";
                                                dataConcat += Item.JUN_WEIGHT + "," + Item.JUN_VPT + ",";
                                            }
                                            if (Item.JLY_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JLY_WEIGHT,VPT,";
                                                outLine += Item.JLY_WEIGHT + "," + Item.JLY_VPT + ",";
                                                hearderConcat += Item.YEAR + " JLY_WEIGHT,VPT,";
                                                dataConcat += Item.JLY_WEIGHT + "," + Item.JLY_VPT + ",";
                                            }
                                            if (Item.AUG_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + "AUG_WEIGHT,VPT,";
                                                outLine += Item.AUG_WEIGHT + "," + Item.AUG_VPT + ",";
                                                hearderConcat += Item.YEAR + " AUG_WEIGHT,VPT,";
                                                dataConcat += Item.AUG_WEIGHT + "," + Item.AUG_VPT + ",";
                                            }
                                            if (Item.SEP_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " SEP_WEIGHT,VPT,";
                                                outLine += Item.SEP_WEIGHT + "," + Item.SEP_VPT + ",";
                                                hearderConcat += Item.YEAR + " SEP_WEIGHT,VPT,";
                                                dataConcat += Item.SEP_WEIGHT + "," + Item.SEP_VPT + ",";
                                            }
                                            if (Item.OCT_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " OCT_WEIGHT,VPT,";
                                                outLine += Item.OCT_WEIGHT + "," + Item.OCT_VPT + ",";
                                                hearderConcat += Item.YEAR + " OCT_WEIGHT,VPT,";
                                                dataConcat += Item.OCT_WEIGHT + "," + Item.OCT_VPT + ",";
                                            }
                                            if (Item.NOV_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " NOV_WEIGHT,VPT,";
                                                outLine += Item.NOV_WEIGHT + "," + Item.NOV_VPT + ",";
                                                hearderConcat += Item.YEAR + " NOV_WEIGHT,VPT,";
                                                dataConcat += Item.NOV_WEIGHT + "," + Item.NOV_VPT + ",";
                                            }
                                            if (Item.DEC_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " DEC_WEIGHT,VPT,";
                                                outLine += Item.DEC_WEIGHT + "," + Item.DEC_VPT + ",";
                                                hearderConcat += Item.YEAR + " DEC_WEIGHT,VPT,";
                                                dataConcat += Item.DEC_WEIGHT + "," + Item.DEC_VPT + ",";
                                            }
                                        }
                                    }

                                    //CHAS
                                    if (model.ShowWeight && model.ShowWeightValueVpt && model.ShowWeightVpt)
                                    {
                                        int iResult = searchList.FindIndex(x => x.Key.Equals(SearchKey) && x.Year == Item.YEAR);

                                        if (iResult == -1)
                                        {
                                            if (Item.JAN_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JAN_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.JAN_WEIGHT + "," + Item.JAN_VPT + "," + Item.JAN_VALUE + ",";
                                                hearderConcat += Item.YEAR + " JAN_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.JAN_WEIGHT + "," + Item.JAN_VPT + "," + Item.JAN_VALUE + ",";
                                            }
                                            if (Item.FEB_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " FEB_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.FEB_WEIGHT + "," + Item.FEB_VPT + "," + Item.FEB_VALUE + ",";
                                                hearderConcat += Item.YEAR + " FEB_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.FEB_WEIGHT + "," + Item.FEB_VPT + "," + Item.FEB_VALUE + ",";
                                            }
                                            if (Item.MAR_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " MAR_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.MAR_WEIGHT + "," + Item.MAR_VPT + "," + Item.MAR_VALUE + ",";
                                                hearderConcat += Item.YEAR + " MAR_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.MAR_WEIGHT + "," + Item.MAR_VPT + "," + Item.MAR_VALUE + ",";
                                            }
                                            if (Item.APR_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " APR_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.APR_WEIGHT + "," + Item.APR_VPT + "," + Item.APR_VALUE + ",";
                                                hearderConcat += Item.YEAR + " APR_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.APR_WEIGHT + "," + Item.APR_VPT + "," + Item.APR_VALUE + ",";
                                            }
                                            if (Item.MAY_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " MAY_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.MAY_WEIGHT + "," + Item.MAY_VPT + "," + Item.MAY_VALUE + ",";
                                                hearderConcat += Item.YEAR + " MAY_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.MAY_WEIGHT + "," + Item.MAY_VPT + "," + Item.MAY_VALUE + ",";
                                            }
                                            if (Item.JUN_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JUN_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.JUN_WEIGHT + "," + Item.JUN_VPT + "," + Item.JUN_VALUE + ",";
                                                hearderConcat += Item.YEAR + " JUN_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.JUN_WEIGHT + "," + Item.JUN_VPT + "," + Item.JUN_VALUE + ",";
                                            }
                                            if (Item.JLY_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " JLY_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.JLY_WEIGHT + "," + Item.JLY_VPT + "," + Item.JLY_VALUE + ",";
                                                hearderConcat += Item.YEAR + " JLY_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.JLY_WEIGHT + "," + Item.JLY_VPT + "," + Item.JLY_VALUE + ",";
                                            }
                                            if (Item.AUG_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " AUG_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.AUG_WEIGHT + "," + Item.AUG_VPT + "," + Item.AUG_VALUE + ",";
                                                hearderConcat += Item.YEAR + " AUG_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.AUG_WEIGHT + "," + Item.AUG_VPT + "," + Item.AUG_VALUE + ",";
                                            }
                                            if (Item.SEP_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " SEP_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.SEP_WEIGHT + "," + Item.SEP_VPT + "," + Item.SEP_VALUE + ",";
                                                hearderConcat += Item.YEAR + " SEP_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.SEP_WEIGHT + "," + Item.SEP_VPT + "," + Item.SEP_VALUE + ",";
                                            }
                                            if (Item.OCT_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " OCT_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.OCT_WEIGHT + "," + Item.OCT_VPT + "," + Item.OCT_VALUE + ",";
                                                hearderConcat += Item.YEAR + " OCT_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.OCT_WEIGHT + "," + Item.OCT_VPT + "," + Item.OCT_VALUE + ",";
                                            }
                                            if (Item.NOV_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " NOV_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.NOV_WEIGHT + "," + Item.NOV_VPT + "," + Item.NOV_VALUE + ",";
                                                hearderConcat += Item.YEAR + " NOV_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.NOV_WEIGHT + "," + Item.NOV_VPT + "," + Item.NOV_VALUE + ",";
                                            }
                                            if (Item.DEC_WEIGHT >= 0)
                                            {
                                                headerLine += Item.YEAR + " DEC_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                outLine += Item.DEC_WEIGHT + "," + Item.DEC_VPT + "," + Item.DEC_VALUE + ",";
                                                hearderConcat += Item.YEAR + " DEC_WEIGHT,VPT," + selectedCurrency + ": VALUE,";
                                                dataConcat += Item.DEC_WEIGHT + "," + Item.DEC_VPT + "," + Item.DEC_VALUE + ",";
                                            }
                                        }
                                    }

                                    if (Item.SIDE_OF_TRADE.Equals("Export"))
                                    {
                                        var iFind = ExportList.FindIndex(x => x.Key.Equals(SearchKey));

                                        if (iFind >= 0)
                                        {
                                            var foundModel = ExportList[iFind];
                                            foundModel.Header += hearderConcat;
                                            foundModel.Data += dataConcat;

                                            ExportList[iFind] = foundModel;
                                            hearderConcat = string.Empty;
                                            dataConcat = string.Empty;
                                        }
                                        else
                                        {
                                            var exportModel = new CrossTabMultiMonthModel
                                            {
                                                Key = SearchKey,
                                                Data = outLine,
                                                Header = headerLine
                                            };

                                            ExportList.Add(exportModel);
                                        }
                                    }

                                    if (Item.SIDE_OF_TRADE.Equals("Import"))
                                    {
                                        var iFind = ImportList.FindIndex(x => x.Key.Equals(SearchKey));

                                        if (iFind >= 0)
                                        {
                                            var foundModel = ImportList[iFind];
                                            foundModel.Header += hearderConcat;
                                            foundModel.Data += dataConcat;

                                            ImportList[iFind] = foundModel;
                                            hearderConcat = string.Empty;
                                            dataConcat = string.Empty;
                                        }
                                        else
                                        {
                                            var importModel = new CrossTabMultiMonthModel
                                            {
                                                Key = SearchKey,
                                                Data = outLine,
                                                Header = headerLine
                                            };

                                            ImportList.Add(importModel);
                                        }
                                    }
                                }


                                //End
                            }
                            //END
                        }

                        MemoryStream ms = new MemoryStream();
                        TextWriter tw = new StreamWriter(ms);

                        if (ExportList.Count > 0)
                        {
                            var sModel = ExportList.OrderByDescending(s => s.Header.Length).First();
                            var hItem = sModel.Header.Remove(sModel.Header.Length - 1);
                            tw.WriteLine(hItem);

                            foreach (var Item in ExportList)
                            {
                                var dataItem = Item.Data.Remove(Item.Data.Length - 1);

                                string[] dt = dataItem.Split(',');
                                var listOfNumber = new List<int>();
                                Array.Reverse(dt);

                                foreach (var dtItem in dt)
                                {
                                    bool res = int.TryParse(dtItem, out int iNumber);
                                    if (res)
                                    {
                                        listOfNumber.Add(iNumber);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                var sum = listOfNumber.Sum();

                                if (sum > 0)
                                    tw.WriteLine(dataItem);
                            }
                        }

                        if (ImportList.Count > 0)
                        {
                            int cnt = ImportList.Count;
                            ImportList.RemoveAt(cnt - 1);

                            if (ExportList.Count == 0)
                            {

                                var sModel = ImportList.OrderByDescending(s => s.Header.Length).First();
                                var hItem = sModel.Header.Remove(sModel.Header.Length - 1);
                                tw.WriteLine(hItem);
                            }
                            foreach (var Item in ImportList)
                            {
                                var dataItem = Item.Data.Remove(Item.Data.Length - 1);
                                string[] dt = dataItem.Split(',');
                                var listOfNumber = new List<int>();
                                Array.Reverse(dt);

                                foreach (var dtItem in dt)
                                {
                                    bool res = int.TryParse(dtItem, out int iNumber);
                                    if (res)
                                    {
                                        listOfNumber.Add(iNumber);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                var sum = listOfNumber.Sum();

                                if (sum > 0)
                                    tw.WriteLine(dataItem);
                            }
                        }

                        tw.Flush();

                        var FileName = model.FileName + ".csv";

                        var length = ms.Length;
                        tw.Close();
                        var toWrite = new byte[length];
                        Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
                        ms.Close();
                        return File(toWrite, "text/plain", FileName);

                    }
                    else
                    {
                        if (model.FileType.Equals("TXT"))
                        {

                            MemoryStream ms = new MemoryStream();
                            TextWriter tw = new StreamWriter(ms);
                            int cnt = 0, Total = 0;
                            var writeOutList = new List<string>();


                            Total++;
                            foreach (var Item in outList)
                            {
                                var outLine = string.Empty;
                                var headerLine = string.Empty;

                                // headerLine += "ID,";
                                // outLine = Item.ID.ToString() + ",";

                                foreach (var kItem in fList)
                                {
                                    if (kItem.Equals("SC_GEO"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.SC_GEO))
                                        {
                                            headerLine += "SC_GEO,";
                                            outLine += Item.SC_GEO + ",";
                                        }
                                    }
                                    if (kItem.Equals("SC_NAME"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.SC_NAME))
                                        {
                                            headerLine += "SC_NAME,";
                                            outLine += Item.SC_NAME + ",";
                                        }
                                    }
                                    if (kItem.Equals("SC_GEO_REGION"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.SC_GEO_REGION))
                                        {
                                            headerLine += "SC_GEO_REGION,";
                                            outLine += Item.SC_GEO_REGION + ",";
                                        }
                                    }
                                    if (kItem.Equals("MC_GEO"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.MC_GEO))
                                        {
                                            headerLine += "MC_GEO,";
                                            outLine += Item.MC_GEO + ",";
                                        }
                                    }

                                    if (kItem.Equals("MC_NAME"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.MC_NAME))
                                        {
                                            headerLine += "MC_NAME,";
                                            outLine += Item.MC_NAME + ",";
                                        }
                                    }

                                    if (kItem.Equals("TARIFF_CODE"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                        {
                                            headerLine += "TARIFF_CODE,";
                                            outLine += Item.TARIFF_CODE + ",";
                                        }
                                    }

                                    if (kItem.Equals("TARIFF_DESCRIPTION"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                                        {
                                            headerLine += "TARIFF_DESCRIPTION,";
                                            outLine += Item.TARIFF_LEGEND + ",";
                                        }
                                    }

                                    if (kItem.Equals("TWO_DIGIT"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.TWO_DIGIT))
                                        {
                                            headerLine += "TWO_DIGIT,";
                                            outLine += Item.TWO_DIGIT + ",";
                                        }
                                    }

                                    if (kItem.Equals("SIX_DIGIT"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.SIX_DIGIT))
                                        {
                                            headerLine += "SIX_DIGIT,";
                                            outLine += Item.SIX_DIGIT + ",";
                                        }
                                    }

                                    if (kItem.Equals("TARIFF_CATEGORY"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.TARIFF_CATEGORY))
                                        {
                                            headerLine += "TARIFF_CATEGORY,";
                                            outLine += Item.TARIFF_CATEGORY + ",";
                                        }
                                    }

                                    if (kItem.Equals("SIDE_OF_TRADE"))
                                    {
                                        if (!string.IsNullOrEmpty(Item.SIDE_OF_TRADE))
                                        {
                                            headerLine += "SIDE_OF_TRADE,";
                                            outLine += Item.SIDE_OF_TRADE + ",";
                                        }
                                    }
                                    if (kItem.Equals("YEAR"))
                                    {
                                        if (Item.YEAR > 0)
                                        {
                                            headerLine += "YEAR,";
                                            outLine += Item.YEAR + ",";
                                        }
                                    }
                                    if (kItem.Equals("QUARTER"))
                                    {
                                        if (Item.QUARTER > 0)
                                        {
                                            headerLine += "QUARTER,";
                                            outLine += Item.QUARTER + ",";
                                        }
                                    }
                                }


                                if (model.ShowWeight && !model.ShowWeightValueVpt && !model.ShowWeightVpt)
                                {
                                    if (Item.JAN_WEIGHT > 0)
                                        headerLine += "JAN_WEIGHT,";
                                    if (Item.FEB_WEIGHT > 0)
                                        headerLine += "FEB_WEIGHT,";
                                    if (Item.MAR_WEIGHT > 0)
                                        headerLine += "MAR_WEIGHT,";
                                    if (Item.APR_WEIGHT > 0)
                                        headerLine += "APR_WEIGHT,";
                                    if (Item.MAY_WEIGHT > 0)
                                        headerLine += "MAY_WEIGHT,";
                                    if (Item.JUN_WEIGHT > 0)
                                        headerLine += "JUN_WEIGHT,";
                                    if (Item.JLY_WEIGHT > 0)
                                        headerLine += "JLY_WEIGHT,";
                                    if (Item.AUG_WEIGHT > 0)
                                        headerLine += "AUG_WEIGHT,";
                                    if (Item.SEP_WEIGHT > 0)
                                        headerLine += "SEP_WEIGHT,";
                                    if (Item.OCT_WEIGHT > 0)
                                        headerLine += "OCT_WEIGHT,";
                                    if (Item.NOV_WEIGHT > 0)
                                        headerLine += "NOV_WEIGHT,";
                                    if (Item.DEC_WEIGHT > 0)
                                        headerLine += "DEC_WEIGHT,";


                                    if (Item.JAN_WEIGHT > 0)
                                        outLine += Item.JAN_WEIGHT + ",";
                                    if (Item.FEB_WEIGHT > 0)
                                        outLine += Item.FEB_WEIGHT + ",";
                                    if (Item.MAR_WEIGHT > 0)
                                        outLine += Item.MAR_WEIGHT + ",";
                                    if (Item.APR_WEIGHT > 0)
                                        outLine += Item.APR_WEIGHT + ",";
                                    if (Item.MAY_WEIGHT > 0)
                                        outLine += Item.MAY_WEIGHT + ",";
                                    if (Item.JUN_WEIGHT > 0)
                                        outLine += Item.JUN_WEIGHT + ",";
                                    if (Item.JLY_WEIGHT > 0)
                                        outLine += Item.JLY_WEIGHT + ",";
                                    if (Item.AUG_WEIGHT > 0)
                                        outLine += Item.AUG_WEIGHT + ",";
                                    if (Item.SEP_WEIGHT > 0)
                                        outLine += Item.SEP_WEIGHT + ",";
                                    if (Item.OCT_WEIGHT > 0)
                                        outLine += Item.OCT_WEIGHT + ",";
                                    if (Item.NOV_WEIGHT > 0)
                                        outLine += Item.NOV_WEIGHT + ",";
                                    if (Item.DEC_WEIGHT > 0)
                                        outLine += Item.DEC_WEIGHT + ",";

                                    if (cnt == 0)
                                    {
                                        headerLine = headerLine.Remove(headerLine.Length - 1);
                                        writeOutList.Add(headerLine);
                                    }
                                }

                                if (model.ShowWeight && !model.ShowWeightValueVpt && model.ShowWeightVpt)
                                {
                                    if (Item.JAN_WEIGHT > 0)
                                        headerLine += "JAN_WEIGHT,JAN_VPT,";
                                    if (Item.FEB_WEIGHT > 0)
                                        headerLine += "FEB_WEIGHT,FEB_VPT,";
                                    if (Item.MAR_WEIGHT > 0)
                                        headerLine += "MAR_WEIGHT,MAR_VPT,";
                                    if (Item.APR_WEIGHT > 0)
                                        headerLine += "APR_WEIGHT,APR_VPT,";
                                    if (Item.MAY_WEIGHT > 0)
                                        headerLine += "MAY_WEIGHT,MAY_VPT,";
                                    if (Item.JUN_WEIGHT > 0)
                                        headerLine += "JUN_WEIGHT,JUN_VPT,";
                                    if (Item.JLY_WEIGHT > 0)
                                        headerLine += "JLY_WEIGHT,JLY_VPT,";
                                    if (Item.AUG_WEIGHT > 0)
                                        headerLine += "AUG_WEIGHT,AUG_VPT,";
                                    if (Item.SEP_WEIGHT > 0)
                                        headerLine += "SEP_WEIGHT,SEP_VPT,";
                                    if (Item.OCT_WEIGHT > 0)
                                        headerLine += "OCT_WEIGHT,OCT_VPT,";
                                    if (Item.NOV_WEIGHT > 0)
                                        headerLine += "NOV_WEIGHT,NOV_VPT,";
                                    if (Item.DEC_WEIGHT > 0)
                                        headerLine += "DEC_WEIGHT,DEC_VPT,";


                                    if (Item.JAN_WEIGHT > 0)
                                        outLine += Item.JAN_WEIGHT + "," + Item.JAN_VPT + ",";
                                    if (Item.FEB_WEIGHT > 0)
                                        outLine += Item.FEB_WEIGHT + "," + Item.FEB_VPT + ",";
                                    if (Item.MAR_WEIGHT > 0)
                                        outLine += Item.MAR_WEIGHT + "," + Item.MAR_VPT + ",";
                                    if (Item.APR_WEIGHT > 0)
                                        outLine += Item.APR_WEIGHT + "," + Item.APR_VPT + ",";
                                    if (Item.MAY_WEIGHT > 0)
                                        outLine += Item.MAY_WEIGHT + "," + Item.MAY_VPT + ",";
                                    if (Item.JUN_WEIGHT > 0)
                                        outLine += Item.JUN_WEIGHT + "," + Item.JUN_VPT + ",";
                                    if (Item.JLY_WEIGHT > 0)
                                        outLine += Item.JLY_WEIGHT + "," + Item.JLY_VPT + ",";
                                    if (Item.AUG_WEIGHT > 0)
                                        outLine += Item.AUG_WEIGHT + "," + Item.AUG_VPT + ",";
                                    if (Item.SEP_WEIGHT > 0)
                                        outLine += Item.SEP_WEIGHT + "," + Item.SEP_VPT + ",";
                                    if (Item.OCT_WEIGHT > 0)
                                        outLine += Item.OCT_WEIGHT + "," + Item.OCT_VPT + ",";
                                    if (Item.NOV_WEIGHT > 0)
                                        outLine += Item.NOV_WEIGHT + "," + Item.NOV_VPT + ",";
                                    if (Item.DEC_WEIGHT > 0)
                                        outLine += Item.DEC_WEIGHT + "," + Item.DEC_VPT + ",";

                                    if (cnt == 0)
                                    {
                                        headerLine = headerLine.Remove(headerLine.Length - 1);
                                        writeOutList.Add(headerLine);
                                    }
                                }

                                if (model.ShowWeight && model.ShowWeightValueVpt && model.ShowWeightVpt)
                                {
                                    if (Item.JAN_WEIGHT > 0)
                                        headerLine += "JAN_WEIGHT,JAN_VPT,JAN_VALUE,";
                                    if (Item.FEB_WEIGHT > 0)
                                        headerLine += "FEB_WEIGHT,FEB_VPT,FEB_VALUE,";
                                    if (Item.MAR_WEIGHT > 0)
                                        headerLine += "MAR_WEIGHT,MAR_VPT,MAR_VALUE,";
                                    if (Item.APR_WEIGHT > 0)
                                        headerLine += "APR_WEIGHT,APR_VPT,APR_VALUE,";
                                    if (Item.MAY_WEIGHT > 0)
                                        headerLine += "MAY_WEIGHT,MAY_VPT,MAY_VALUE,";
                                    if (Item.JUN_WEIGHT > 0)
                                        headerLine += "JUN_WEIGHT,JUN_VPT,JUN_VALUE,";
                                    if (Item.JLY_WEIGHT > 0)
                                        headerLine += "JLY_WEIGHT,JLY_VPT,JLY_VALUE,";
                                    if (Item.AUG_WEIGHT > 0)
                                        headerLine += "AUG_WEIGHT,AUG_VPT,AUG_VALUE,";
                                    if (Item.SEP_WEIGHT > 0)
                                        headerLine += "SEP_WEIGHT,SEP_VPT,SEP_VALUE,";
                                    if (Item.OCT_WEIGHT > 0)
                                        headerLine += "OCT_WEIGHT,OCT_VPT,OCT_VALUE,";
                                    if (Item.NOV_WEIGHT > 0)
                                        headerLine += "NOV_WEIGHT,NOV_VPT,NOV_VALUE,";
                                    if (Item.DEC_WEIGHT > 0)
                                        headerLine += "DEC_WEIGHT,DEC_VPT,DEC_VALUE,";


                                    if (Item.JAN_WEIGHT > 0)
                                        outLine += Item.JAN_WEIGHT + "," + Item.JAN_VPT + "," + Item.JAN_VALUE + ",";
                                    if (Item.FEB_WEIGHT > 0)
                                        outLine += Item.FEB_WEIGHT + "," + Item.FEB_VPT + "," + Item.FEB_VALUE + ",";
                                    if (Item.MAR_WEIGHT > 0)
                                        outLine += Item.MAR_WEIGHT + "," + Item.MAR_VPT + "," + Item.MAR_VALUE + ",";
                                    if (Item.APR_WEIGHT > 0)
                                        outLine += Item.APR_WEIGHT + "," + Item.APR_VPT + "," + Item.APR_VALUE + ",";
                                    if (Item.MAY_WEIGHT > 0)
                                        outLine += Item.MAY_WEIGHT + "," + Item.MAY_VPT + "," + Item.MAY_VALUE + ",";
                                    if (Item.JUN_WEIGHT > 0)
                                        outLine += Item.JUN_WEIGHT + "," + Item.JUN_VPT + "," + Item.JUN_VALUE + ",";
                                    if (Item.JLY_WEIGHT > 0)
                                        outLine += Item.JLY_WEIGHT + "," + Item.JLY_VPT + "," + Item.JLY_VALUE + ",";
                                    if (Item.AUG_WEIGHT > 0)
                                        outLine += Item.AUG_WEIGHT + "," + Item.AUG_VPT + "," + Item.AUG_VALUE + ",";
                                    if (Item.SEP_WEIGHT > 0)
                                        outLine += Item.SEP_WEIGHT + "," + Item.SEP_VPT + "," + Item.SEP_VALUE + ",";
                                    if (Item.OCT_WEIGHT > 0)
                                        outLine += Item.OCT_WEIGHT + "," + Item.OCT_VPT + "," + Item.OCT_VALUE + ",";
                                    if (Item.NOV_WEIGHT > 0)
                                        outLine += Item.NOV_WEIGHT + "," + Item.NOV_VPT + "," + Item.NOV_VALUE + ",";
                                    if (Item.DEC_WEIGHT > 0)
                                        outLine += Item.DEC_WEIGHT + "," + Item.DEC_VPT + "," + Item.DEC_VALUE + ",";

                                    if (cnt == 0)
                                    {
                                        headerLine = headerLine.Remove(headerLine.Length - 1);
                                        writeOutList.Add(headerLine);
                                    }
                                }


                                if (writeOutList.Count > 0)
                                {
                                    if (cnt > 0)
                                        headerLine = headerLine.Remove(headerLine.Length - 1);

                                    if (headerLine.Length > writeOutList[0].Length)
                                    {
                                        writeOutList[0] = headerLine;
                                    }
                                }

                                outLine = outLine.Remove(outLine.Length - 1);
                                writeOutList.Add(outLine);
                                cnt++;
                                Total++;
                            }

                            foreach (var Item in writeOutList)
                                tw.WriteLine(Item);

                            tw.Flush();

                            var FileName = model.FileName + ".csv";

                            var length = ms.Length;
                            tw.Close();
                            var toWrite = new byte[length];
                            Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
                            ms.Close();
                            return File(toWrite, "text/plain", FileName);
                        }
                    }
                }
            } //CHAS END
            else
            {

                //END Test
                dataOut.IsPivotData = model.PivotTable;
                dataOut.CSS = model.CSS;

                // var LinierTable = new List<string>();

                var HeaderLine = string.Empty;
                var FieldList = new List<string>();

                var selectedCurrency = keyData.SelectedCurrency;

                if (!string.IsNullOrEmpty(model.FieldSort))
                {
                    string[] words = model.FieldSort.Split(',');

                    foreach (var f in words)
                    {
                        if (!string.IsNullOrEmpty(f))
                        {
                            if (f.Contains("VALUE"))
                            {
                                var hItem = string.Empty;
                                hItem = selectedCurrency + ": " + f;
                                HeaderLine += hItem + ",";
                                FieldList.Add(f);
                            }
                            else
                            {
                                FieldList.Add(f);
                                HeaderLine += f + ",";
                            }
                        }
                    }

                    HeaderLine = HeaderLine.Remove(HeaderLine.Length - 1);

                    // LinierTable = await LinierTableBuilderWithSort(keyData, dataOut, FieldList, model, ",");

                }

                IEnumerable<ReportWriterModel> rData = dataOut.data;

                if (model.FileType.Equals("TXT"))
                {

                    MemoryStream ms = new MemoryStream();
                    TextWriter tw = new StreamWriter(ms);
                    int cnt = 0, Total = 0;

                    tw.WriteLine(HeaderLine);
                    Total++;
                    var outLine = string.Empty;

                    foreach (var Item in rData)
                    {
                        outLine = string.Empty;
                        foreach (var hText in FieldList)
                        {
                            if (hText.Equals("SC_GEO"))
                            {
                                outLine += Item.SC_GEO + ",";
                               
                            }

                            if (hText.Equals("SC_NAME"))
                            {
                               
                                outLine += Item.SC_NAME + ",";
                            }

                            if (hText.Equals("SC_GEO_REGION"))
                            {
                                outLine += Item.SC_GEO_REGION + ",";
                            }

                            if (hText.Equals("MC_GEO"))
                            {
                                outLine += Item.MC_GEO + ",";
                                outLine += Item.MC_NAME + ",";
                            }

                            if (hText.Equals("SIDE_OF_TRADE"))
                            {
                                outLine += Item.SIDE_OF_TRADE + ",";

                            }

                            if (hText.Equals("TARIFF_CODE"))
                            {
                                outLine += Item.TARIFF_CODE + ",";

                            }

                            if (hText.Equals("TARIFF_DESCRIPTION"))
                            {

                                outLine += Item.TARIFF_LEGEND.Replace(",", " ") + ",";
                            }

                            if (hText.Equals("PORT"))
                            {
                                outLine += Item.PORT_ID + ",";

                            }

                            if (hText.Equals("PORT_NAME"))
                            {
                                outLine += Item.PORT_NAME.Replace(",", " ") + ",";

                            }

                            if (hText.Equals("TWO_DIGIT"))
                            {
                                outLine += Item.TWO_DIGIT + ",";
                            }

                            if (hText.Equals("SIX_DIGIT"))
                            {
                                outLine += Item.SIX_DIGIT + ",";
                            }

                            if (hText.Equals("TARIFF_CATEGORY"))
                            {
                                outLine += Item.TARIFF_CATEGORY + ",";
                            }

                            if (hText.Equals("YEAR"))
                            {
                                outLine += Item.YEAR + ",";

                            }

                            if (hText.Equals("MONTH"))
                            {
                                outLine += Item.MONTH + ",";

                            }

                            if (hText.Equals("QUARTER"))
                            {
                                outLine += Item.QUARTER + ",";

                            }

                            if (hText.Equals("WEIGHT"))
                            {
                                outLine += Math.Round(Item.WEIGHT, 0) + ",";

                            }
                            if (hText.Equals("VALUE"))
                            {
                                outLine += Math.Round(Item.MONETARY_VALUE, 0) + ",";

                            }

                            if (hText.Equals("YTD_WEIGHT"))
                            {
                                outLine += Math.Round(Item.YTD_WEIGHT, 0) + ",";

                            }

                            if (hText.Equals("YTD_VALUE"))
                            {
                                outLine += Math.Round(Item.YTD_MONETARY_VALUE, 0) + ",";

                            }
                        }


                        outLine = outLine.Remove(outLine.Length - 1);
                        tw.WriteLine(outLine);
                        cnt++;
                        Total++;
                    }

                    tw.Flush();

                    var FileName = model.FileName + ".csv";

                    var length = ms.Length;
                    tw.Close();
                    var toWrite = new byte[length];
                    Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
                    ms.Close();
                    return File(toWrite, "text/plain", FileName);

                }
            }

            return RedirectToAction("Index", "Report");

        }

        [Authorize]
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> ReportOutput(string _id)
        {

            var srv = new ReportDesignerService();
            var model = await srv.GetReportById(_id);
            var keyData = await SearchResults(model.ReportID);
            var DataList = new List<ReportWriterModel>();
            var regionService = new SourceCountryFilterService();
            var regionList = await regionService.GetRegionCodes();
            var twoDigitService = new ProductService();
            var twoDigitList = await twoDigitService.GetAllMedumProducts();
            var savedQuerySrv = new QueryService();
            var savedQModel = await savedQuerySrv.GetReportByID(model.ReportID);
            var tariffCategoryList = new List<ProductCustomFilterModel>();

            if (savedQModel.ProductGroupType.Equals("Custom"))
            {
                var filterSrv = new ProductCustomFilterService();
                tariffCategoryList = await filterSrv.GetCustomProductsbyReport(model.ReportID);
            }


            foreach (var Item in keyData.Data)
            {
                var TwoDigit = string.Empty;

                var outModel = new ReportWriterModel
                {
                    ID = Item.ID,
                    MC_GEO = Item.MC_GEO,
                    MC_NAME = Item.MC_NAME,

                    MONETARY_VALUE = Item.MONETARY_VALUE,
                    MONTH = Item.MONTH,
                    PORT_ID = Item.PORT_ID,
                    PORT_NAME = Item.PORT_NAME,
                    QUARTER = Item.QUARTER,
                    SC_GEO = Item.SC_GEO,
                    SC_NAME = Item.SC_NAME,
                    SIDE_OF_TRADE = Item.SIDE_OF_TRADE,
                    TARIFF_CODE = Item.TARIFF_CODE,
                    TARIFF_LEGEND = Item.TARIFF_LEGEND,
                    TIME_ID = Item.TIME_ID,

                    WEIGHT = Item.WEIGHT,
                    YEAR = Item.YEAR,
                    YTD_MONETARY_VALUE = Item.YTD_MONETARY_VALUE,
                    YTD_WEIGHT = Item.YTD_WEIGHT


                };

                if (!string.IsNullOrEmpty(Item.SC_GEO))
                {

                    var rModel = regionList.Find(x => x.GeoCode.Equals(Item.SC_GEO));
                    if (rModel != null)
                        outModel.SC_GEO_REGION = rModel.RegionName;
                    else
                        outModel.SC_GEO_REGION = "--";
                }
                else
                {
                    outModel.SC_GEO_REGION = "--";
                }

                if (!string.IsNullOrEmpty(Item.TARIFF_CODE))
                {

                    // var result = twoDigitList.Where(a => a.Items.Any(b => b.TariffCode.Equals(Item.TARIFF_CODE)));

                    outModel.SIX_DIGIT = Item.TARIFF_CODE.Substring(0, 6);
                    foreach (var Prod in twoDigitList)
                    {
                        foreach (var item in Prod.Items)
                        {
                            if (item.TariffCode.Equals(outModel.SIX_DIGIT))
                            {
                                TwoDigit = Prod.Code + " " + Prod.Name;
                            }
                        }
                    }


                }
                //else
                //{
                //    outModel.TWO_DIGIT = "--";
                //}

                foreach (var custItem in tariffCategoryList)
                {
                    var cModel = custItem.Items.FirstOrDefault(x => x.TariffCode.Equals(Item.TARIFF_CODE));
                    if (cModel != null)
                    {
                        outModel.TARIFF_CATEGORY = custItem.Name;
                        break;
                    }
                }



                outModel.TWO_DIGIT = TwoDigit;
                DataList.Add(outModel);

            }

            var dataOut = new ReportOutModelJsonModel { data = DataList };

            dataOut.IsPivotData = model.PivotTable;
            dataOut.CSS = model.CSS;

            var LinierTable = new List<string>();

            if (!string.IsNullOrEmpty(model.FieldSort))
            {
                string[] words = model.FieldSort.Split(',');
                var FieldList = new List<string>();
                foreach (var f in words)
                {
                    if (!string.IsNullOrEmpty(f))
                        FieldList.Add(f);
                }

                LinierTable = await LinierTableBuilderWithSort(keyData, dataOut, FieldList, model, ",");

            }

            var ReportFooterDate = DateTime.Now;

            var StartMonth = dataOut.data[0].MONTH;
            var StartYear = dataOut.data[0].YEAR;
            var StartQuarter = dataOut.data[0].QUARTER;

            int dataLen = dataOut.data.Count - 1;
            var EndMonth = dataOut.data[dataLen].MONTH;
            var EndYear = dataOut.data[dataLen].YEAR;
            var EndQuarter = dataOut.data[dataLen].QUARTER;

            //var dataContent = string.Empty;
            StringBuilder dataContent = new StringBuilder();


            foreach (var item in LinierTable)
            {
                dataContent.AppendLine(item);
            }

            var Content = model.Content
                .Replace("[REPORT_TITLE]", model.Name)
                .Replace("[REPORT_DATA]", dataContent.ToString())
                .Replace("[START_MONTH]", StartMonth.ToString())
                .Replace("[START_YEAR]", StartYear.ToString())
                .Replace("[START_QUARTER]", StartQuarter.ToString())
                .Replace("[END_MONTH]", EndMonth.ToString())
                .Replace("[END_YEAR]", EndYear.ToString())
                .Replace("[END_QUARTER]", EndQuarter.ToString())
                .Replace("[FOOTER_DATE]", ReportFooterDate.ToString("dd MMMM yyyy"));

            dataOut.HTML = Content;

            dataOut.JQUERY = model.JQUERY;

            if (string.IsNullOrEmpty(dataOut.JQUERY))
            {
                dataOut.JQUERY = string.Empty;
            }

            if (string.IsNullOrEmpty(model.Header))
            {
                model.Header = string.Empty;
            }

            ViewData["JQUERY"] = new HtmlString(dataOut.JQUERY);
            ViewData["CSS"] = new HtmlString(dataOut.CSS);
            var str = new HtmlString(Content);
            ViewData["HTML"] = str;

            var formatType = Recipe.ChromePdf;
            var FileExt = string.Empty;



            if (model.FileType.Equals("Html"))
            {
                formatType = Recipe.Html;
                FileExt = ".html";
            }

            if (model.FileType.Equals("Excel"))
            {
                formatType = Recipe.HtmlToXlsx;
                FileExt = ".xlsx";
            }

            if (model.FileType.Equals("Pdf"))
            {
                formatType = Recipe.ChromePdf;
                FileExt = ".pdf";
            }

            HttpContext.JsReportFeature()

             .Recipe(formatType)
            .OnAfterRender((r) => HttpContext.Response.Headers["Content-Disposition"] = "attachment; filename=" + model.FileName + FileExt + "")
            .Configure((r) => r.Options = new RenderOptions
            {
                //       Preview = true,

                Timeout = 180000
            })
            .Configure((r) => r.Template.Chrome = new Chrome
            {
                HeaderTemplate = model.Header,
                DisplayHeaderFooter = model.IncludeHeader,
                MarginTop = model.MarginTop,
                MarginBottom = model.MarginBottom,
                MarginLeft = model.MarginLeft,
                MarginRight = model.MarginRight,
                Landscape = model.Landscape,
                Format = model.PageSize,
                Scale = model.Scale,
                PageRanges = model.PageRanges,
                FooterTemplate = "<div class='footer'><p></p></div>",
                //    WaitForJS = true

                //Footer = ""

                // WaitForNetworkIddle = true

            });



            //if (model.RunFromHTML)
            //{
            //    dataOut.HTML = model.HTML;
            //}
            //else
            //{
            //    model.HTML = dataOut.HTML;
            //    await srv.Save(model);
            //}

            //model.HTML = string.Empty;
            //await srv.Save(model);

            return View("ReportOutput", dataOut);

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new ReportDesignerService();
            await srv.Delete(_id);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ReportDesignerModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Reports Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = _id
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "Report");

        }

        [Obsolete]
        private string GetPath(string filename)
        {
            string path = _env.WebRootPath + "/reports/";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + filename;

        }

        [HttpGet]
        [Authorize]
        public async Task<ReportEngineModel> SearchResults(string Id)
        {
            var srv = new QueryService();
            var model = await srv.GetReportByID(Id);

            if (model.Name == null)
            {
                return null;
            }

            ViewBag.Title = model.Name;

            if (model.ProductGroupType.Equals("Long"))
            {
                var tariffSrv = new TariffService();
                var productList = new List<SaveQueryProductsModel>();
                foreach (var product in model.Products)
                {
                    int pID = await tariffSrv.ConvertFromGuidToId(product.Product);
                    var prodModel = new SaveQueryProductsModel { Product = pID.ToString() };
                    productList.Add(prodModel);
                }
                model.Products = productList;
            }


            var dateList = new List<int>();
            foreach (var d in model.TimeIDs) dateList.Add(d.TimeID);

            var scList = new List<string>();
            foreach (var sc in model.SourceCountryGEOs) scList.Add(sc.GeoCode);

            var mcList = new List<string>();
            foreach (var mc in model.MarketCountryGEOs) mcList.Add(mc.GeoCode);

            var prodList = new List<string>();
            foreach (var p in model.Products) prodList.Add(p.Product);

            var portList = new List<int>();
            if (model.IncludePorts) foreach (var p in model.Ports) portList.Add(p.PortID);

            //if (model.ProductGroupType.Equals("Custom"))
            //{
            //    var tariffSrv = new TariffService();
            //    var filterSrv = new ProductCustomFilterService();
            //    var productList = new List<SaveQueryProductsModel>();
            //    foreach (var product in model.Products)
            //    {
            //        var products = await filterSrv.GetByID(product.Product);

            //        foreach (var Item in products.Items)
            //        {
            //            var pID = await tariffSrv.GetTariffByCode(Item.TariffCode);
            //            var prodModel = new SaveQueryProductsModel { Product = pID.TARIFF_ID.ToString() };
            //            productList.Add(prodModel);
            //        }

            //    }
            //    model.Products = productList;

            //    prodList = new List<string>();
            //    foreach (var p in model.Products) prodList.Add(p.Product);
            //}


            if (model.ProductGroupType.Equals("Custom"))
            {
                model.ProductGroupType = "Long";
                var tariffSrv = new TariffService();
                var filterSrv = new ProductCustomFilterService();
                var productList = new List<SaveQueryProductsModel>();

                foreach (var item in model.Products)
                {
                    var product = await filterSrv.GetByID(item.Product);

                    foreach (var tCode in product.Items)
                    {
                        if (tCode.TariffCode.Length == 6)
                        {
                            model.ProductGroupType = "Broad";
                            var tModel = await tariffSrv.GetHSTariffByCode(tCode.TariffCode);
                            if (tModel != null)
                            {
                                //var pID = await tariffSrv.GetHSTariffByCode(tCode.TariffCode);
                                var prodModel = new SaveQueryProductsModel { Product = tCode.TariffCode };
                                productList.Add(prodModel);

                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(product.SearchType))
                                product.SearchType = "CN";

                            var tModel = await tariffSrv.GetTariffByCodeAndRegion(tCode.TariffCode, product.SearchType);
                            if (tModel._id != null)
                            {
                                int pID = await tariffSrv.ConvertFromGuidToId(tModel._id);
                                var prodModel = new SaveQueryProductsModel { Product = pID.ToString() };
                                productList.Add(prodModel);

                            }
                        }
                    }
                }
                model.Products = productList;

                prodList = new List<string>();
                foreach (var p in model.Products) prodList.Add(p.Product);
            }

            //ShowWeightZero CHAS
            var searchModel = new SearchModel
            {
                ShowWeightZero = false,
                Id = model.Id,
                Date = model.Date,
                User = model.User,
                Name = model.Name,
                Description = model.Description,
                Comments = model.Comments,

                SourceCountryGeoCodes = scList,
                MarketCountryGeoCodes = mcList,
                IncludePorts = model.IncludePorts,
                Ports = portList,
                Products = prodList,
                GroupByMonth = model.GroupByMonth,
                GroupByQuarter = model.GroupByQuarter,
                GroupByYear = model.GroupByYear,
                Dates = dateList,

                SourceCountryGroup = model.SourceCountryGroup,
                TradeFlowType = model.TradeFlowType,
                MarketCountryGroup = model.MarketCountryGroup,
                PortGroup = model.PortGroup,
                ProductGroupType = model.ProductGroupType,
                ProductGroup = model.ProductGroup,
                TonnesValuesGroup = model.TonnesValuesGroup
            };


            var res = new ReportEngineService();

            //if(model.PivotTable)
            searchModel.IsPivot = false;
            var factory = await res.GetReport(searchModel);

            //var factory = await res(searchModel);

            var viewModel = new ReportEngineModel { Search = searchModel, Data = (IQueryable<ReportOutModel>)factory.Queryable };
            await viewModel.ConvertToCurrency(model.SelectedCurrency);

            var cB = new Infragistics.Web.Mvc.GridColumnBuilder<ReportOutModel>();
            viewModel.Columns(cB);

            return viewModel;
        }

        private async Task<List<string>> LinierTableBuilder(ReportEngineModel keyData, ReportOutModelJsonModel dataOut, List<string> FieldSort, ReportDesignerModel model, string seperator)
        {
            var dataStr = string.Empty;
            var dataList = new List<string>();
            var reportHelper = new ReportWriterHelper();
            if (model.FileType.Equals("TXT"))
            {
                dataStr = await reportHelper.BuildCSVHeader(keyData, FieldSort, model, seperator);

                dataStr = dataStr.Remove(dataStr.Length - 1);
                dataList.Add(dataStr);
            }
            else
            {
                if (model.TableHeader.Equals("<p><br></p>"))
                {
                    dataStr = await reportHelper.BuildHeader(keyData, FieldSort, model);
                    dataList.Add(dataStr);
                }
                else
                {
                    dataStr = model.TableHeader;
                    dataList.Add(dataStr);
                }
            }
            dataStr = string.Empty;

            if (model.FileType.Equals("TXT"))
            {
                var dataListTable = await reportHelper.TableCSVBuilder(keyData, dataOut, FieldSort, model, seperator);

                foreach (var Item in dataListTable)
                {
                    dataList.Add(Item);
                }
            }
            else
            {
                dataStr += "<tbody>";
                dataList.Add(dataStr);

                var dataListTable = await reportHelper.TableBuilder(keyData, dataOut, FieldSort, model);

                //Modified by CJ duplicates
                foreach (var Item in dataListTable)
                {
                    dataList.Add(Item);
                }

                if (!model.ShowTotals)
                {
                    dataStr = "</tbody>";
                    dataList.Add(dataStr);
                    dataStr = "</table>";
                    dataList.Add(dataStr);
                }
            }
            return dataList;
        }

        private async Task<List<string>> LinierTableBuilderWithSort(ReportEngineModel keyData, ReportOutModelJsonModel dataOut, List<string> FieldSort, ReportDesignerModel model, string seperator)
        {
            var LastFilter = string.Empty;
            var LastYear = string.Empty;
            var LastFlow = string.Empty;
            var dataStr = string.Empty;
            var dataList = new List<string>();
            int TableID = 1;
            var keyService = new ReportKeysService();
            var keyModel = new ReportKeysModel();
            var reportHelper = new ReportWriterHelper();
            var header = string.Empty;
            if (dataOut.IsPivotData)
            {

                var htmlStr = string.Empty;

                if (model.FileType.Equals("TXT"))
                {
                    htmlStr = await reportHelper.BuildCSVHeader(keyData, FieldSort, model, seperator);
                }
                else
                {
                    if (model.TableHeader.Equals("<p><br></p>"))
                    {
                        htmlStr = await reportHelper.BuildHeader(keyData, FieldSort, model);
                        // htmlStr += "</tr></thead>";
                        header = htmlStr;
                    }
                    else
                    {
                        htmlStr = model.TableStyle;
                        //htmlStr += "<thead><tr>";
                        htmlStr += model.TableHeader;
                        // htmlStr += "</tr></thead>";
                    }
                }

                htmlStr = htmlStr.Remove(htmlStr.Length - 1);
                dataList.Add(htmlStr);

                string[] words = dataStr.Split(',');

                var pivotModelList = new List<PivotModel>();
                var pivotModel = new PivotModel();

                //keyData.Data = dataOut;

                pivotModelList = await reportHelper.SourceGrouping(keyData, dataOut, FieldSort, model, seperator);

                IList<PropertyInfo> htmlProperties = typeof(PivotModel).GetProperties().ToList();

                if (model.FileType.Equals("TXT"))
                {
                    foreach (var tBuilder in pivotModelList)
                    {
                        htmlStr = string.Empty;
                        for (int i = 1; i < htmlProperties.Count; i++)
                        {
                            var FieldVal = htmlProperties[i].GetValue(tBuilder);
                            if (FieldVal != null)
                            {
                                if (!string.IsNullOrEmpty(FieldVal.ToString()))
                                {

                                    htmlStr += FieldVal.ToString();
                                }
                            }
                        }
                        htmlStr = htmlStr.Remove(htmlStr.Length - 1);
                        dataList.Add(htmlStr);
                    }
                    return dataList;
                }

                htmlStr = "<tbody>";
                dataList.Add(htmlStr);

                List<PivotModel> FooterList = new List<PivotModel>();
                FooterList.AddRange(pivotModelList);

                foreach (var tBuilder in pivotModelList)
                {
                    if (string.IsNullOrEmpty(tBuilder.PageBreak))
                        tBuilder.PageBreak = string.Empty;

                    if (tBuilder.PageBreak.Equals("PAGE-AFTER"))
                    {
                        tBuilder.PageBreak = null;

                        if (model.ShowTotals)
                        {
                            var ThisFlow = tBuilder.Flow;
                            // LastFilter = tBuilder.Field2;
                            LastYear = tBuilder.Year;

                            if (!LastFilter.Equals(tBuilder.Field2))
                            {
                                if (string.IsNullOrEmpty(LastFlow))
                                {
                                    ThisFlow = tBuilder.Flow;
                                }
                                else
                                {
                                    ThisFlow = LastFlow;
                                }
                            }

                            LastFilter = tBuilder.Field2;

                            LastFlow = tBuilder.Flow;
                            htmlStr = "<tfoot id='table-footer' style='font-size: " + model.FontSize + "'>";
                            dataList.Add(htmlStr);
                            htmlStr = reportHelper.SummaryPivotBuilder(FieldSort, model, tBuilder.Field1, tBuilder.Year, ThisFlow);
                            dataList.Add(htmlStr);
                            htmlStr = "</tfoot>";
                            dataList.Add(htmlStr);
                        }


                        tBuilder.Field1 = string.Empty;
                        tBuilder.Field2 = string.Empty;
                        tBuilder.Year = string.Empty;
                        tBuilder.Flow = string.Empty;
                        htmlStr = "</tbody></table>";
                        dataList.Add(htmlStr);

                        htmlStr = "<div style='page-break-after: always; '></div>";
                        dataList.Add(htmlStr);

                        if (model.TableHeader.Equals("<p><br></p>"))
                        {
                            var th = await reportHelper.BuildHeader(keyData, FieldSort, model);
                            int nID = TableID + 1;
                            th = th.Replace("id='1'", "id='" + nID + "'");
                            TableID++;
                            htmlStr = th;
                            dataList.Add(htmlStr);
                            dataList.Add("<tbody>");
                        }
                        else
                        {
                            htmlStr = model.TableHeader;
                            dataList.Add(htmlStr);
                        }

                        htmlStr = "<tr>";

                        dataList.Add("<tr>");
                        for (int i = 1; i < htmlProperties.Count; i++)
                        {
                            var FieldVal = htmlProperties[i].GetValue(tBuilder);
                            if (FieldVal != null)
                            {
                                if (!string.IsNullOrEmpty(FieldVal.ToString()))
                                {
                                    htmlStr = FieldVal.ToString();
                                    dataList.Add(htmlStr);
                                }
                            }
                        }

                    }
                    else
                    {

                        htmlStr = "<tr>";

                        dataList.Add("<tr>");
                        for (int i = 1; i < htmlProperties.Count; i++)
                        {
                            var FieldVal = htmlProperties[i].GetValue(tBuilder);
                            if (FieldVal != null)
                            {
                                if (!string.IsNullOrEmpty(FieldVal.ToString()))
                                {
                                    htmlStr = FieldVal.ToString();
                                    dataList.Add(htmlStr);
                                }
                            }
                        }

                        htmlStr = "</tr>";
                        dataList.Add(htmlStr);
                    }
                }

                htmlStr = "</tbody>";
                dataList.Add(htmlStr);
                //Build summaries here

                if (model.ShowTotals)
                {
                    htmlStr = "<tfoot id='table-footer' style='font-size: " + model.FontSize + "'>";
                    dataList.Add(htmlStr);
                    htmlStr = reportHelper.SummaryPivotBuilder(FieldSort, model, LastFilter, LastYear, LastFlow);
                    dataList.Add(htmlStr);
                    htmlStr = "</tfoot>";
                    dataList.Add(htmlStr);
                    htmlStr = "</table>";
                    dataList.Add(htmlStr);

                }
                htmlStr = "</table>";
                dataList.Add(htmlStr);
                //dataStr = htmlStr;
                //dataList.Add(dataStr);
            }
            else
            {
                int cnt = keyData.Data.Count();
                var dataListTable = await LinierTableBuilder(keyData, dataOut, FieldSort, model, seperator);

                foreach (var Item in dataListTable)
                {
                    dataList.Add(Item);
                }
            }

            return dataList;
        }

        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {

                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {

                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        private async Task InitKeys()
        {
            //Update report keys
            var rkService = new ReportKeysService();
            var rkModel = new ReportKeysModel
            {
                Key = "SC_GEO",
                Description = "Source Country GEO Code",
                Title = "SC GEO",
                Sort = 1,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "SC_NAME",
                Description = "Source Country Name",
                Title = "SC Description",
                Sort = 2,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "MC_GEO",
                Description = "Market Country GEO Code",
                Title = "MC GEO",
                Sort = 3,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "MC_NAME",
                Description = "Market Country Name",
                Title = "MC Description",
                Sort = 4,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "TARIFF_CODE",
                Description = "Tariff Code",
                Title = "Tariff Code",
                Sort = 5,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "TARIFF_DESCRIPTION",
                Description = "Tariff Description",
                Title = "Tariff Description",
                Sort = 6,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "PORT",
                Description = "Port",
                Title = "Port",
                Sort = 7,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "PORT_NAME",
                Description = "Port Name",
                Title = "Port Name",
                Sort = 8,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "SIDE_OF_TRADE",
                Description = "Side of Trade Imports / Exports",
                Title = "Side of Trade",
                Sort = 9,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "YEAR",
                Description = "Report Year",
                Title = "Year",
                Sort = 10,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "QUARTER",
                Description = "Report Quarter",
                Title = "Quarter",
                Sort = 11,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "MONTH",
                Description = "Report Month",
                Title = "Month",
                Sort = 12,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "WEIGHT",
                Description = "Report Tones",
                Title = "Tonnes",
                Sort = 13,
                Width = "10px"
            };
            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "VALUE",
                Description = "Report in GBP",
                Title = "GBP",
                Sort = 14,
                Width = "10px"
            };

            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "VPT",
                Description = "Value Per Tonne",
                Title = "VPT",
                Sort = 15,
                Width = "10px"
            };

            await rkService.Add(rkModel);
            rkModel = new ReportKeysModel
            {
                Key = "YTD_WEIGHT",
                Description = "Report YTD Tones",
                Title = "Tonnes YTD",
                Sort = 16,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "YTD_VALUE",
                Description = "Report YTD GBP",
                Title = "YTD GBP",
                Sort = 17,
                Width = "10px"
            };
            await rkService.Add(rkModel);


            rkModel = new ReportKeysModel
            {
                Key = "YTD_VPT",
                Description = "Report YTD GBP",
                Title = "YTD GBP",
                Sort = 18,
                Width = "10px"
            };
            await rkService.Add(rkModel);


            rkModel = new ReportKeysModel
            {
                Key = "JAN_WEIGHT",
                Description = "Tonnes for January",
                Title = "Jan Ton's",
                Sort = 19,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "FEB_WEIGHT",
                Description = "Tonnes for February",
                Title = "Feb Ton's",
                Sort = 20,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "MAR_WEIGHT",
                Description = "Tonnes for March",
                Title = "Mar Ton's",
                Sort = 21,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "APR_WEIGHT",
                Description = "Tonnes for April",
                Title = "Apr Ton's",
                Sort = 22,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "MAY_WEIGHT",
                Description = "Tonnes for May",
                Title = "May Ton's",
                Sort = 23,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "JUN_WEIGHT",
                Description = "Tonnes for June",
                Title = "June Ton's",
                Sort = 24,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "JUL_WEIGHT",
                Description = "Tonnes for July",
                Title = "Jul Ton's",
                Sort = 25,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "AUG_WEIGHT",
                Description = "Tonnes for August",
                Title = "Aug Ton's",
                Sort = 26,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "SEP_WEIGHT",
                Description = "Tonnes for September",
                Title = "Sep Ton's",
                Sort = 27,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "OCT_WEIGHT",
                Description = "Tonnes for October",
                Title = "Oct Ton's",
                Sort = 28,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "NOV_WEIGHT",
                Description = "Tonnes for November",
                Title = "Nov Ton's",
                Sort = 29,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "DEC_WEIGHT",
                Description = "Tonnes for December",
                Title = "Dec Ton's",
                Sort = 30,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "JAN_VALUE",
                Description = "GBP for January",
                Title = "Jan GBP",
                Sort = 31,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "FEB_VALUE",
                Description = "GBP for February",
                Title = "Feb GBP",
                Sort = 32,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "MAR_VALUE",
                Description = "GBP for March",
                Title = "Mar GBP",
                Sort = 33,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "APR_VALUE",
                Description = "GBP for April",
                Title = "Apr GBP",
                Sort = 34,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "MAY_VALUE",
                Description = "GBP for May",
                Title = "May GBP",
                Sort = 35,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "JUN_VALUE",
                Description = "GBP for June",
                Title = "June GBP",
                Sort = 36,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "JUL_VALUE",
                Description = "GBP for July",
                Title = "Jul GBP",
                Sort = 37,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "AUG_VALUE",
                Description = "GBP for August",
                Title = "Aug GBP",
                Sort = 38,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "SEP_VALUE",
                Description = "GBP for September",
                Title = "Sep GBP",
                Sort = 39,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "OCT_VALUE",
                Description = "GBP for October",
                Title = "Oct GBP",
                Sort = 40,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "NOV_VALUE",
                Description = "GBP for November",
                Title = "Nov GBP",
                Sort = 41,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "DEC_VALUE",
                Description = "GBP for December",
                Title = "Dec GBP",
                Sort = 42,
                Width = "10px"
            };
            await rkService.Add(rkModel);


            rkModel = new ReportKeysModel
            {
                Key = "JAN_VPT",
                Description = "VPT for January",
                Title = "Jan VPT",
                Sort = 43,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "FEB_VPT",
                Description = "VPT for February",
                Title = "Feb VPT",
                Sort = 44,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "MAR_VPT",
                Description = "VPT for March",
                Title = "Jan VPT",
                Sort = 45,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "APR_VPT",
                Description = "VPT for April",
                Title = "Apr VPT",
                Sort = 46,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "MAY_VPT",
                Description = "VPT for May",
                Title = "May VPT",
                Sort = 47,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "JUN_VPT",
                Description = "VPT for June",
                Title = "June VPT",
                Sort = 48,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "JUL_VPT",
                Description = "VPT for July",
                Title = "Jul VPT",
                Sort = 49,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "AUG_VPT",
                Description = "VPT for August",
                Title = "Aug VPT",
                Sort = 50,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "SEP_VPT",
                Description = "VPT for September",
                Title = "Sep VPT",
                Sort = 51,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "OCT_VPT",
                Description = "VPT for October",
                Title = "Oct VPT",
                Sort = 52,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "NOV_VPT",
                Description = "VPT for November",
                Title = "Nov VPT",
                Sort = 53,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "DEC_VPT",
                Description = "VPT for December",
                Title = "Dec VPT",
                Sort = 54,
                Width = "10px"
            };
            await rkService.Add(rkModel);


            rkModel = new ReportKeysModel
            {
                Key = "TOTAL_WEIGHT",
                Description = "YTD Tonnes total",
                Title = "YTD Ton's",
                Sort = 55,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "TOTAL_VALUE",
                Description = "YTD Value total",
                Title = "YTD GBP",
                Sort = 56,
                Width = "10px"
            };
            await rkService.Add(rkModel);

            rkModel = new ReportKeysModel
            {
                Key = "TOTAL_VPT",
                Description = "YTD Value per Tonnes",
                Title = "YTD VPT",
                Sort = 57,
                Width = "10px"
            };
            await rkService.Add(rkModel);

        }
    }
}