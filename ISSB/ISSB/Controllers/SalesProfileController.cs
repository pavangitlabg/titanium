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
using System.Security.Claims;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class SalesProfileController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
        [Obsolete]
        public SalesProfileController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var exRates = new ExchnageRateService();
            //var rates = await exRates.GetLatestRates();

            var srv = new SalesProfileService();

           // await srv.ConvertData();


            var model = await srv.GetSalesProfiles();
            IQueryable data = model.AsQueryable();
            return View(data);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new SalesProfileService();
            var model = await srv.GetSalesProfileById(_id);

            var lookupSrv = new LookupsService();
            ViewBag.TaxCodes = await lookupSrv.GetTaxCodesDropdown();

            return View(model);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(SalesProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Details";
                return View("Edit", model);
            }

            var srv = new SalesProfileService();
            await srv.SaveSalesProfile(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SalesProfileModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Sales Profile Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SalesProfile");

        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new SalesProfileService();
            var modelOut = await srv.GetSalesProfileById(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.SalesProfileModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Sales Profile Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "SalesProfile");

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new SalesProfileService();
            var nRec = await ser.GetCount();
            var modelOut = new SalesProfileModel
            {
                ACCOUNT_ON_HOLD = 0,
                ACCOUNT_REF = "",
                ACCOUNT_STATUS = "",
                ACC_ON_HOLD = "",
                ADDRESS_1 = "",
                ADDRESS_2 = "",
                ADDRESS_3 = "",
                ADDRESS_4 = "",
                ADDRESS_5 = "",
                ANALYSIS_1 = "",
                ANALYSIS_2 = "",
                ANALYSIS_3 = "",
                AVERAGE_PAY_DAYS = 0,
                BALANCE = 0,
                BANK_ACCOUNT_NAME = "",
                BANK_ACCOUNT_NUMBER = "",
                BANK_ADDITIONALREF1 = "",
                BANK_ADDITIONALREF2 = "",
                BANK_ADDITIONALREF3 = "",
                BANK_ADDRESS_1 = "",
                BANK_ADDRESS_2 = "",
                BANK_ADDRESS_3 = "",
                BANK_ADDRESS_4 = "",
                BANK_ADDRESS_5 = "",
                BANK_BAC = "",
                BANK_BIC = "",
                BANK_IBAN = "",
                BANK_NAME = "",
                BANK_ROLLNUMBER = "",
                BANK_SORT_CODE = "",
                BUREAU_CODE = 0,
                CAN_CHARGE = 0,
                CATEGORY = "",
                COMPANY_ID = 0,
                CONTACT_NAME = "",
                COUNTRY_CODE = "",
                COUNTRY_NAME = "",
                CREDIT_CHARGE = "",
                CREDIT_LIMIT = 0,
                CREDIT_POSITION = "",
                CREDIT_POS_CODE = 0,
                CREDIT_REF = "",
                CURRENCY = "",
                CURRENCY_NAME = "",
                CURRENCY_NUMBER = 0,
                C_ADDRESS_1 = "",
                C_ADDRESS_2 = "",
                C_ADDRESS_3 = "",
                C_ADDRESS_4 = "",
                C_ADDRESS_5 = "",
                DATE_ACCOUNT_OPENED = new DateTime(1980, 1, 1),
                DATE_AC_OPENED = new DateTime(1980, 1, 1),
                DATE_CREDIT_APPLIED = new DateTime(1980, 1, 1),
                DATE_CREDIT_RECEIVED = new DateTime(1980, 1, 1),
                DATE_LAST_CREDIT = new DateTime(1980, 1, 1),
                DATE_NEXT_CREDIT = new DateTime(1980, 1, 1),
                DEF_NOM_CODE = "",
                DEF_TAX_CODE = "",
                DEL_ADDRESS_1 = "",
                DEL_ADDRESS_2 = "",
                DEL_ADDRESS_3 = "",
                DEL_ADDRESS_4 = "",
                DEL_ADDRESS_5 = "",
                DEL_E_MAIL = "",
                DEL_FAX = "",
                DEL_NAME = "",
                DEL_TELEPHONE = "",
                DEPT_NAME = "",
                DEPT_NUMBER = 0,
                DISCOUNT_ADDITIONAL = "",
                DISCOUNT_RATE = 0,
                DISCOUNT_TYPE = 0,
                E_MAIL = "",
                FAX = "",
                FIRST_INV_DATE = new DateTime(1980, 1, 1),
                PRICING_REF = "",
                PAYMENT_DUE_DAYS = 0,
                PORTAL = "",
                LAST_INV_DATE = new DateTime(1980, 1, 1),
                LAST_PAYMENT_DATE = new DateTime(1980, 1, 1),
                LAST_UPDATED = new DateTime(1980, 1, 1),
                MEMO = "",
                MEMO_2 = "",
                NAME = "",
                NOMINAL_NAME = "",
                PORTAL_EMAIL = "",
                OVERRIDE_TAX_CODE = 0,
                PRIOR_YEAR = 0,
                TELEPHONE = "",
                RESTRICT_MAIL = "",
                RESTRICT_MAIL2 = "",
                RESTRICT_NOMINAL_CODE = "",
                RESTRICT_TAX_CODE = "",
                SALES_LEVEL = "",
                SETTLEMENT_DISC_RATE = 0,
                SETTLEMENT_DUE_DAYS = 0,
                STATUS = "",
                STATUS_NUMBER = 0,
                STATUS_TEXT = "",
                TAX_NAME = "",
                TELEPHONE_2 = "",
                TERMS = "",
                TERMS_AGREED = "",
                TES_EMAIL = "",
                TRADE_CONTACT = "",

                TURNOVER_MTD = 0,
                TURNOVER_YTD = 0,
                UNSUBSCRIBE = "",
                USER_NAME = "",
                VAT_REG_NUMBER = "",
                WEB_ADDRESS = ""
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(SalesProfileModel model)
        {
            var srv = new SalesProfileService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Customer Details";
                var ser = new SalesProfileService();
                var nRec = await ser.GetCount();
                var modelOut = new SalesProfileModel
                {
                    ACCOUNT_ON_HOLD = 0,
                    ACCOUNT_REF = "",
                    ACCOUNT_STATUS = "",
                    ACC_ON_HOLD = "",
                    ADDRESS_1 = "",
                    ADDRESS_2 = "",
                    ADDRESS_3 = "",
                    ADDRESS_4 = "",
                    ADDRESS_5 = "",
                    ANALYSIS_1 = "",
                    ANALYSIS_2 = "",
                    ANALYSIS_3 = "",
                    AVERAGE_PAY_DAYS = 0,
                    BALANCE = 0,
                    BANK_ACCOUNT_NAME = "",
                    BANK_ACCOUNT_NUMBER = "",
                    BANK_ADDITIONALREF1 = "",
                    BANK_ADDITIONALREF2 = "",
                    BANK_ADDITIONALREF3 = "",
                    BANK_ADDRESS_1 = "",
                    BANK_ADDRESS_2 = "",
                    BANK_ADDRESS_3 = "",
                    BANK_ADDRESS_4 = "",
                    BANK_ADDRESS_5 = "",
                    BANK_BAC = "",
                    BANK_BIC = "",
                    BANK_IBAN = "",
                    BANK_NAME = "",
                    BANK_ROLLNUMBER = "",
                    BANK_SORT_CODE = "",
                    BUREAU_CODE = 0,
                    CAN_CHARGE = 0,
                    CATEGORY = "",
                    COMPANY_ID = 0,
                    CONTACT_NAME = "",
                    COUNTRY_CODE = "",
                    COUNTRY_NAME = "",
                    CREDIT_CHARGE = "",
                    CREDIT_LIMIT = 0,
                    CREDIT_POSITION = "",
                    CREDIT_POS_CODE = 0,
                    CREDIT_REF = "",
                    CURRENCY = "",
                    CURRENCY_NAME = "",
                    CURRENCY_NUMBER = 0,
                    C_ADDRESS_1 = "",
                    C_ADDRESS_2 = "",
                    C_ADDRESS_3 = "",
                    C_ADDRESS_4 = "",
                    C_ADDRESS_5 = "",
                    DATE_ACCOUNT_OPENED = new DateTime(1980, 1, 1),
                    DATE_AC_OPENED = new DateTime(1980, 1, 1),
                    DATE_CREDIT_APPLIED = new DateTime(1980, 1, 1),
                    DATE_CREDIT_RECEIVED = new DateTime(1980, 1, 1),
                    DATE_LAST_CREDIT = new DateTime(1980, 1, 1),
                    DATE_NEXT_CREDIT = new DateTime(1980, 1, 1),
                    DEF_NOM_CODE = "",
                    DEF_TAX_CODE = "",
                    DEL_ADDRESS_1 = "",
                    DEL_ADDRESS_2 = "",
                    DEL_ADDRESS_3 = "",
                    DEL_ADDRESS_4 = "",
                    DEL_ADDRESS_5 = "",
                    DEL_E_MAIL = "",
                    DEL_FAX = "",
                    DEL_NAME = "",
                    DEL_TELEPHONE = "",
                    DEPT_NAME = "",
                    DEPT_NUMBER = 0,
                    DISCOUNT_ADDITIONAL = "",
                    DISCOUNT_RATE = 0,
                    DISCOUNT_TYPE = 0,
                    E_MAIL = "",
                    FAX = "",
                    FIRST_INV_DATE = new DateTime(1980, 1, 1),
                    PRICING_REF = "",
                    PAYMENT_DUE_DAYS = 0,
                    PORTAL = "",
                    LAST_INV_DATE = new DateTime(1980, 1, 1),
                    LAST_PAYMENT_DATE = new DateTime(1980, 1, 1),
                    LAST_UPDATED = new DateTime(1980, 1, 1),
                    MEMO = "",
                    MEMO_2 = "",
                    NAME = "",
                    NOMINAL_NAME = "",
                    PORTAL_EMAIL = "",
                    OVERRIDE_TAX_CODE = 0,
                    PRIOR_YEAR = 0,
                    TELEPHONE = "",
                    RESTRICT_MAIL = "",
                    RESTRICT_MAIL2 = "",
                    RESTRICT_NOMINAL_CODE = "",
                    RESTRICT_TAX_CODE = "",
                    SALES_LEVEL = "",
                    SETTLEMENT_DISC_RATE = 0,
                    SETTLEMENT_DUE_DAYS = 0,
                    STATUS = "",
                    STATUS_NUMBER = 0,
                    STATUS_TEXT = "",
                    TAX_NAME = "",
                    TELEPHONE_2 = "",
                    TERMS = "",
                    TERMS_AGREED = "",
                    TES_EMAIL = "",
                    TRADE_CONTACT = "",

                    TURNOVER_MTD = 0,
                    TURNOVER_YTD = 0,
                    UNSUBSCRIBE = "",
                    USER_NAME = "",
                    VAT_REG_NUMBER = "",
                    WEB_ADDRESS = ""

                };

                return View("Add", modelOut);
            }
            await srv.Add(model);

            return RedirectToAction("Index", "SalesProfile");

        }
    }
}
