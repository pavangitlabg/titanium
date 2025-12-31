using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class SalesProfileService
{
    public async Task<List<SalesProfileModel>> GetSalesProfiles()
    {
        var db = new DbContext();
        var cursor = await db.SalesProfileDb.FindAsync(new BsonDocument());

        IList<SalesProfileDB> results = cursor.ToList();
        var modelList = new List<SalesProfileModel>();

        foreach (var Item in results)
        {
            var model = new SalesProfileModel
            {
                _id = Item._id.ToString(),
                ACCOUNT_ON_HOLD = Item.ACCOUNT_ON_HOLD,
                ACCOUNT_REF = Item.ACCOUNT_REF,
                ACCOUNT_STATUS = Item.ACCOUNT_STATUS,
                ACC_ON_HOLD = Item.ACC_ON_HOLD,
                ADDRESS_1 = Item.ADDRESS_1,
                ADDRESS_2 = Item.ADDRESS_2,
                ADDRESS_3 = Item.ADDRESS_3,
                ADDRESS_4 = Item.ADDRESS_4,
                ADDRESS_5 = Item.ADDRESS_5,
                ANALYSIS_1 = Item.ANALYSIS_1,
                ANALYSIS_2 = Item.ANALYSIS_2,
                ANALYSIS_3 = Item.ANALYSIS_3,
                AVERAGE_PAY_DAYS = Item.AVERAGE_PAY_DAYS,
                BALANCE = Item.BALANCE,
                BANK_ACCOUNT_NAME = Item.BANK_ACCOUNT_NAME,
                BANK_ACCOUNT_NUMBER = Item.BANK_ACCOUNT_NUMBER,
                BANK_ADDITIONALREF1 = Item.BANK_ADDITIONALREF1,
                BANK_ADDITIONALREF2 = Item.BANK_ADDITIONALREF2,
                BANK_ADDITIONALREF3 = Item.BANK_ADDITIONALREF3,
                BANK_ADDRESS_1 = Item.BANK_ADDRESS_1,
                BANK_ADDRESS_2 = Item.BANK_ADDRESS_2,
                BANK_ADDRESS_3 = Item.BANK_ADDRESS_3,
                BANK_ADDRESS_4 = Item.BANK_ADDRESS_4,
                BANK_ADDRESS_5 = Item.BANK_ADDRESS_5,
                BANK_BAC = Item.BANK_BAC,
                BANK_BIC = Item.BANK_BIC,
                BANK_IBAN = Item.BANK_IBAN,
                BANK_NAME = Item.BANK_NAME,
                BANK_ROLLNUMBER = Item.BANK_ROLLNUMBER,
                BANK_SORT_CODE = Item.BANK_SORT_CODE,
                BUREAU_CODE = Item.BUREAU_CODE,
                CAN_CHARGE = Item.CAN_CHARGE,
                CATEGORY = Item.CATEGORY,
                COMPANY_ID = Item.COMPANY_ID,
                CONTACT_NAME = Item.CONTACT_NAME,
                COUNTRY_CODE = Item.COUNTRY_CODE,
                COUNTRY_NAME = Item.COUNTRY_NAME,
                CREDIT_CHARGE = Item.CREDIT_CHARGE,
                CREDIT_LIMIT = Item.CREDIT_LIMIT,
                CREDIT_POSITION = Item.CREDIT_POSITION,
                CREDIT_POS_CODE = Item.CREDIT_POS_CODE,
                CREDIT_REF = Item.CREDIT_REF,
                CURRENCY = Item.CURRENCY,
                CURRENCY_NAME = Item.CURRENCY_NAME,
                CURRENCY_NUMBER = Item.CURRENCY_NUMBER,
                C_ADDRESS_1 = Item.C_ADDRESS_1,
                C_ADDRESS_2 = Item.C_ADDRESS_2,
                C_ADDRESS_3 = Item.C_ADDRESS_3,
                C_ADDRESS_4 = Item.C_ADDRESS_4,
                C_ADDRESS_5 = Item.C_ADDRESS_5,
                DATE_ACCOUNT_OPENED = (DateTime)Item.DATE_ACCOUNT_OPENED,
                DATE_AC_OPENED = (DateTime)Item.DATE_AC_OPENED,
                DATE_CREDIT_APPLIED = (DateTime)Item.DATE_CREDIT_APPLIED,
                DATE_CREDIT_RECEIVED = (DateTime)Item.DATE_CREDIT_RECEIVED,
                DATE_LAST_CREDIT = (DateTime)Item.DATE_LAST_CREDIT,
                DATE_NEXT_CREDIT = (DateTime)Item.DATE_NEXT_CREDIT,
                DEF_NOM_CODE = Item.DEF_NOM_CODE,
                DEF_TAX_CODE = Item.DEF_TAX_CODE,
                DEL_ADDRESS_1 = Item.DEL_ADDRESS_1,
                DEL_ADDRESS_2 = Item.DEL_ADDRESS_2,
                DEL_ADDRESS_3 = Item.DEL_ADDRESS_3,
                DEL_ADDRESS_4 = Item.DEL_ADDRESS_4,
                DEL_ADDRESS_5 = Item.DEL_ADDRESS_5,
                DEL_E_MAIL = Item.DEL_E_MAIL,
                DEL_FAX = Item.DEL_FAX,
                DEL_NAME = Item.DEL_NAME,
                DEL_TELEPHONE = Item.DEL_TELEPHONE,
                DEPT_NAME = Item.DEPT_NAME,
                DEPT_NUMBER = Item.DEPT_NUMBER,
                DISCOUNT_ADDITIONAL = Item.DISCOUNT_ADDITIONAL,
                DISCOUNT_RATE = Item.DISCOUNT_RATE,
                DISCOUNT_TYPE = Item.DISCOUNT_TYPE,
                E_MAIL = Item.E_MAIL,
                FAX = Item.FAX,
                FIRST_INV_DATE = (DateTime)Item.FIRST_INV_DATE,
                PRICING_REF = Item.PRICING_REF,
                PAYMENT_DUE_DAYS = Item.PAYMENT_DUE_DAYS,
                PORTAL = Item.PORTAL,
                LAST_INV_DATE = (DateTime)Item.LAST_INV_DATE,
                LAST_PAYMENT_DATE = (DateTime)Item.LAST_PAYMENT_DATE,
                LAST_UPDATED = (DateTime)Item.LAST_UPDATED,
                MEMO = Item.MEMO,
                MEMO_2 = Item.MEMO_2,
                NAME = Item.NAME,
                NOMINAL_NAME = Item.NOMINAL_NAME,
                PORTAL_EMAIL = Item.PORTAL_EMAIL,
                OVERRIDE_TAX_CODE = Item.OVERRIDE_TAX_CODE,
                PRIOR_YEAR = Item.PRIOR_YEAR,
                TELEPHONE = Item.TELEPHONE,
                RESTRICT_MAIL = Item.RESTRICT_MAIL,
                RESTRICT_MAIL2 = Item.RESTRICT_MAIL2,
                RESTRICT_NOMINAL_CODE = Item.RESTRICT_NOMINAL_CODE,
                RESTRICT_TAX_CODE = Item.RESTRICT_TAX_CODE,
                SALES_LEVEL = Item.SALES_LEVEL,
                SETTLEMENT_DISC_RATE = Item.SETTLEMENT_DISC_RATE,
                SETTLEMENT_DUE_DAYS = Item.SETTLEMENT_DUE_DAYS,
                STATUS = Item.STATUS,
                STATUS_NUMBER = Item.STATUS_NUMBER,
                STATUS_TEXT = Item.STATUS_TEXT,
                TAX_NAME = Item.TAX_NAME,
                TELEPHONE_2 = Item.TELEPHONE_2,
                TERMS = Item.TERMS,
                TERMS_AGREED = Item.TERMS_AGREED,
                TES_EMAIL = Item.TES_EMAIL,
                TRADE_CONTACT = Item.TRADE_CONTACT,
                TURNOVER_MTD = Item.TURNOVER_MTD,
                TURNOVER_YTD = Item.TURNOVER_YTD,
                UNSUBSCRIBE = Item.UNSUBSCRIBE,
                USER_NAME = Item.USER_NAME,
                VAT_REG_NUMBER = Item.VAT_REG_NUMBER,
                WEB_ADDRESS = Item.WEB_ADDRESS
            };

            modelList.Add(model);
        }

        return modelList;
    }

    public async Task<SalesProfileModel> GetSalesProfileById(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<SalesProfileDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();

        var cursor = await db.SalesProfileDb.FindAsync(filter);

        IList<SalesProfileDB> results = cursor.ToList();

        var Item = results[0];


        var model = new SalesProfileModel
        {
            _id = Item._id.ToString(),
            ACCOUNT_ON_HOLD = Item.ACCOUNT_ON_HOLD,
            ACCOUNT_REF = Item.ACCOUNT_REF,
            ACCOUNT_STATUS = Item.ACCOUNT_STATUS,
            ACC_ON_HOLD = Item.ACC_ON_HOLD,
            ADDRESS_1 = Item.ADDRESS_1,
            ADDRESS_2 = Item.ADDRESS_2,
            ADDRESS_3 = Item.ADDRESS_3,
            ADDRESS_4 = Item.ADDRESS_4,
            ADDRESS_5 = Item.ADDRESS_5,
            ANALYSIS_1 = Item.ANALYSIS_1,
            ANALYSIS_2 = Item.ANALYSIS_2,
            ANALYSIS_3 = Item.ANALYSIS_3,
            AVERAGE_PAY_DAYS = Item.AVERAGE_PAY_DAYS,
            BALANCE = Item.BALANCE,
            BANK_ACCOUNT_NAME = Item.BANK_ACCOUNT_NAME,
            BANK_ACCOUNT_NUMBER = Item.BANK_ACCOUNT_NUMBER,
            BANK_ADDITIONALREF1 = Item.BANK_ADDITIONALREF1,
            BANK_ADDITIONALREF2 = Item.BANK_ADDITIONALREF2,
            BANK_ADDITIONALREF3 = Item.BANK_ADDITIONALREF3,
            BANK_ADDRESS_1 = Item.BANK_ADDRESS_1,
            BANK_ADDRESS_2 = Item.BANK_ADDRESS_2,
            BANK_ADDRESS_3 = Item.BANK_ADDRESS_3,
            BANK_ADDRESS_4 = Item.BANK_ADDRESS_4,
            BANK_ADDRESS_5 = Item.BANK_ADDRESS_5,
            BANK_BAC = Item.BANK_BAC,
            BANK_BIC = Item.BANK_BIC,
            BANK_IBAN = Item.BANK_IBAN,
            BANK_NAME = Item.BANK_NAME,
            BANK_ROLLNUMBER = Item.BANK_ROLLNUMBER,
            BANK_SORT_CODE = Item.BANK_SORT_CODE,
            BUREAU_CODE = Item.BUREAU_CODE,
            CAN_CHARGE = Item.CAN_CHARGE,
            CATEGORY = Item.CATEGORY,
            COMPANY_ID = Item.COMPANY_ID,
            CONTACT_NAME = Item.CONTACT_NAME,
            COUNTRY_CODE = Item.COUNTRY_CODE,
            COUNTRY_NAME = Item.COUNTRY_NAME,
            CREDIT_CHARGE = Item.CREDIT_CHARGE,
            CREDIT_LIMIT = Item.CREDIT_LIMIT,
            CREDIT_POSITION = Item.CREDIT_POSITION,
            CREDIT_POS_CODE = Item.CREDIT_POS_CODE,
            CREDIT_REF = Item.CREDIT_REF,
            CURRENCY = Item.CURRENCY,
            CURRENCY_NAME = Item.CURRENCY_NAME,
            CURRENCY_NUMBER = Item.CURRENCY_NUMBER,
            C_ADDRESS_1 = Item.C_ADDRESS_1,
            C_ADDRESS_2 = Item.C_ADDRESS_2,
            C_ADDRESS_3 = Item.C_ADDRESS_3,
            C_ADDRESS_4 = Item.C_ADDRESS_4,
            C_ADDRESS_5 = Item.C_ADDRESS_5,
            DATE_ACCOUNT_OPENED = (DateTime)Item.DATE_ACCOUNT_OPENED,
            DATE_AC_OPENED = (DateTime)Item.DATE_AC_OPENED,
            DATE_CREDIT_APPLIED = (DateTime)Item.DATE_CREDIT_APPLIED,
            DATE_CREDIT_RECEIVED = (DateTime)Item.DATE_CREDIT_RECEIVED,
            DATE_LAST_CREDIT = (DateTime)Item.DATE_LAST_CREDIT,
            DATE_NEXT_CREDIT = (DateTime)Item.DATE_NEXT_CREDIT,
            DEF_NOM_CODE = Item.DEF_NOM_CODE,
            DEF_TAX_CODE = Item.DEF_TAX_CODE,
            DEL_ADDRESS_1 = Item.DEL_ADDRESS_1,
            DEL_ADDRESS_2 = Item.DEL_ADDRESS_2,
            DEL_ADDRESS_3 = Item.DEL_ADDRESS_3,
            DEL_ADDRESS_4 = Item.DEL_ADDRESS_4,
            DEL_ADDRESS_5 = Item.DEL_ADDRESS_5,
            DEL_E_MAIL = Item.DEL_E_MAIL,
            DEL_FAX = Item.DEL_FAX,
            DEL_NAME = Item.DEL_NAME,
            DEL_TELEPHONE = Item.DEL_TELEPHONE,
            DEPT_NAME = Item.DEPT_NAME,
            DEPT_NUMBER = Item.DEPT_NUMBER,
            DISCOUNT_ADDITIONAL = Item.DISCOUNT_ADDITIONAL,
            DISCOUNT_RATE = Item.DISCOUNT_RATE,
            DISCOUNT_TYPE = Item.DISCOUNT_TYPE,
            E_MAIL = Item.E_MAIL,
            FAX = Item.FAX,
            FIRST_INV_DATE = (DateTime)Item.FIRST_INV_DATE,
            PRICING_REF = Item.PRICING_REF,
            PAYMENT_DUE_DAYS = Item.PAYMENT_DUE_DAYS,
            PORTAL = Item.PORTAL,
            LAST_INV_DATE = (DateTime)Item.LAST_INV_DATE,
            LAST_PAYMENT_DATE = (DateTime)Item.LAST_PAYMENT_DATE,
            LAST_UPDATED = (DateTime)Item.LAST_UPDATED,
            MEMO = Item.MEMO,
            MEMO_2 = Item.MEMO_2,
            NAME = Item.NAME,
            NOMINAL_NAME = Item.NOMINAL_NAME,
            PORTAL_EMAIL = Item.PORTAL_EMAIL,
            OVERRIDE_TAX_CODE = Item.OVERRIDE_TAX_CODE,
            PRIOR_YEAR = Item.PRIOR_YEAR,
            TELEPHONE = Item.TELEPHONE,
            RESTRICT_MAIL = Item.RESTRICT_MAIL,
            RESTRICT_MAIL2 = Item.RESTRICT_MAIL2,
            RESTRICT_NOMINAL_CODE = Item.RESTRICT_NOMINAL_CODE,
            RESTRICT_TAX_CODE = Item.RESTRICT_TAX_CODE,
            SALES_LEVEL = Item.SALES_LEVEL,
            SETTLEMENT_DISC_RATE = Item.SETTLEMENT_DISC_RATE,
            SETTLEMENT_DUE_DAYS = Item.SETTLEMENT_DUE_DAYS,
            STATUS = Item.STATUS,
            STATUS_NUMBER = Item.STATUS_NUMBER,
            STATUS_TEXT = Item.STATUS_TEXT,
            TAX_NAME = Item.TAX_NAME,
            TELEPHONE_2 = Item.TELEPHONE_2,
            TERMS = Item.TERMS,
            TERMS_AGREED = Item.TERMS_AGREED,
            TES_EMAIL = Item.TES_EMAIL,
            TRADE_CONTACT = Item.TRADE_CONTACT,
            TURNOVER_MTD = Item.TURNOVER_MTD,
            TURNOVER_YTD = Item.TURNOVER_YTD,
            UNSUBSCRIBE = Item.UNSUBSCRIBE,
            USER_NAME = Item.USER_NAME,
            VAT_REG_NUMBER = Item.VAT_REG_NUMBER,
            WEB_ADDRESS = Item.WEB_ADDRESS
        };

        return model;
    }

    public async Task<bool> DoesExist(SalesProfileModel model)
    {
        var bReturn = false;
        var builder = Builders<SalesProfileDB>.Filter;
        var filter = builder.Eq("ACCOUNT_REF", model.ACCOUNT_REF);

        var db = new DbContext();

        var cursor = await db.SalesProfileDb.FindAsync(filter);
        IList<SalesProfileDB> results = cursor.ToList();
        if (results.Count > 0)
            bReturn = true;

        return bReturn;
    }

    public async Task<bool> Delete(SalesProfileModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<SalesProfileDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        var returnModel = await db.SalesProfileDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<bool> SaveSalesProfile(SalesProfileModel model)
    {
        var RecordId = new BsonObjectId(new ObjectId(model._id));

        var builder = Builders<SalesProfileDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.SalesProfileDb.FindAsync(filter);
        IList<SalesProfileDB> results = cursor.ToList();


        var modelDB = new SalesProfileDB
        {
            _id = RecordId,
            ACCOUNT_ON_HOLD = model.ACCOUNT_ON_HOLD,
            ACCOUNT_REF = model.ACCOUNT_REF,
            ACCOUNT_STATUS = model.ACCOUNT_STATUS,
            ACC_ON_HOLD = model.ACC_ON_HOLD,
            ADDRESS_1 = model.ADDRESS_1,
            ADDRESS_2 = model.ADDRESS_2,
            ADDRESS_3 = model.ADDRESS_3,
            ADDRESS_4 = model.ADDRESS_4,
            ADDRESS_5 = model.ADDRESS_5,
            ANALYSIS_1 = model.ANALYSIS_1,
            ANALYSIS_2 = model.ANALYSIS_2,
            ANALYSIS_3 = model.ANALYSIS_3,
            AVERAGE_PAY_DAYS = model.AVERAGE_PAY_DAYS,
            BALANCE = model.BALANCE,
            BANK_ACCOUNT_NAME = model.BANK_ACCOUNT_NAME,
            BANK_ACCOUNT_NUMBER = model.BANK_ACCOUNT_NUMBER,
            BANK_ADDITIONALREF1 = model.BANK_ADDITIONALREF1,
            BANK_ADDITIONALREF2 = model.BANK_ADDITIONALREF2,
            BANK_ADDITIONALREF3 = model.BANK_ADDITIONALREF3,
            BANK_ADDRESS_1 = model.BANK_ADDRESS_1,
            BANK_ADDRESS_2 = model.BANK_ADDRESS_2,
            BANK_ADDRESS_3 = model.BANK_ADDRESS_3,
            BANK_ADDRESS_4 = model.BANK_ADDRESS_4,
            BANK_ADDRESS_5 = model.BANK_ADDRESS_5,
            BANK_BAC = model.BANK_BAC,
            BANK_BIC = model.BANK_BIC,
            BANK_IBAN = model.BANK_IBAN,
            BANK_NAME = model.BANK_NAME,
            BANK_ROLLNUMBER = model.BANK_ROLLNUMBER,
            BANK_SORT_CODE = model.BANK_SORT_CODE,
            BUREAU_CODE = model.BUREAU_CODE,
            CAN_CHARGE = model.CAN_CHARGE,
            CATEGORY = model.CATEGORY,
            COMPANY_ID = model.COMPANY_ID,
            CONTACT_NAME = model.CONTACT_NAME,
            COUNTRY_CODE = model.COUNTRY_CODE,
            COUNTRY_NAME = model.COUNTRY_NAME,
            CREDIT_CHARGE = model.CREDIT_CHARGE,
            CREDIT_LIMIT = model.CREDIT_LIMIT,
            CREDIT_POSITION = model.CREDIT_POSITION,
            CREDIT_POS_CODE = model.CREDIT_POS_CODE,
            CREDIT_REF = model.CREDIT_REF,
            CURRENCY = model.CURRENCY,
            CURRENCY_NAME = model.CURRENCY_NAME,
            CURRENCY_NUMBER = model.CURRENCY_NUMBER,
            C_ADDRESS_1 = model.C_ADDRESS_1,
            C_ADDRESS_2 = model.C_ADDRESS_2,
            C_ADDRESS_3 = model.C_ADDRESS_3,
            C_ADDRESS_4 = model.C_ADDRESS_4,
            C_ADDRESS_5 = model.C_ADDRESS_5,
            DATE_ACCOUNT_OPENED = model.DATE_ACCOUNT_OPENED,
            DATE_AC_OPENED = model.DATE_AC_OPENED,
            DATE_CREDIT_APPLIED = model.DATE_CREDIT_APPLIED,
            DATE_CREDIT_RECEIVED = model.DATE_CREDIT_RECEIVED,
            DATE_LAST_CREDIT = model.DATE_LAST_CREDIT,
            DATE_NEXT_CREDIT = model.DATE_NEXT_CREDIT,
            DEF_NOM_CODE = model.DEF_NOM_CODE,
            DEF_TAX_CODE = model.DEF_TAX_CODE,
            DEL_ADDRESS_1 = model.DEL_ADDRESS_1,
            DEL_ADDRESS_2 = model.DEL_ADDRESS_2,
            DEL_ADDRESS_3 = model.DEL_ADDRESS_3,
            DEL_ADDRESS_4 = model.DEL_ADDRESS_4,
            DEL_ADDRESS_5 = model.DEL_ADDRESS_5,
            DEL_E_MAIL = model.DEL_E_MAIL,
            DEL_FAX = model.DEL_FAX,
            DEL_NAME = model.DEL_NAME,
            DEL_TELEPHONE = model.DEL_TELEPHONE,
            DEPT_NAME = model.DEPT_NAME,
            DEPT_NUMBER = model.DEPT_NUMBER,
            DISCOUNT_ADDITIONAL = model.DISCOUNT_ADDITIONAL,
            DISCOUNT_RATE = model.DISCOUNT_RATE,
            DISCOUNT_TYPE = model.DISCOUNT_TYPE,
            E_MAIL = model.E_MAIL,
            FAX = model.FAX,
            FIRST_INV_DATE = model.FIRST_INV_DATE,
            PRICING_REF = model.PRICING_REF,
            PAYMENT_DUE_DAYS = model.PAYMENT_DUE_DAYS,
            PORTAL = model.PORTAL,
            LAST_INV_DATE = model.LAST_INV_DATE,
            LAST_PAYMENT_DATE = model.LAST_PAYMENT_DATE,
            LAST_UPDATED = model.LAST_UPDATED,
            MEMO = model.MEMO,
            MEMO_2 = model.MEMO_2,
            NAME = model.NAME,
            NOMINAL_NAME = model.NOMINAL_NAME,
            PORTAL_EMAIL = model.PORTAL_EMAIL,
            OVERRIDE_TAX_CODE = model.OVERRIDE_TAX_CODE,
            PRIOR_YEAR = model.PRIOR_YEAR,
            TELEPHONE = model.TELEPHONE,
            RESTRICT_MAIL = model.RESTRICT_MAIL,
            RESTRICT_MAIL2 = model.RESTRICT_MAIL2,
            RESTRICT_NOMINAL_CODE = model.RESTRICT_NOMINAL_CODE,
            RESTRICT_TAX_CODE = model.RESTRICT_TAX_CODE,
            SALES_LEVEL = model.SALES_LEVEL,
            SETTLEMENT_DISC_RATE = model.SETTLEMENT_DISC_RATE,
            SETTLEMENT_DUE_DAYS = model.SETTLEMENT_DUE_DAYS,
            STATUS = model.STATUS,
            STATUS_NUMBER = model.STATUS_NUMBER,
            STATUS_TEXT = model.STATUS_TEXT,
            TAX_NAME = model.TAX_NAME,
            TELEPHONE_2 = model.TELEPHONE_2,
            TERMS = model.TERMS,
            TERMS_AGREED = model.TERMS_AGREED,
            TES_EMAIL = model.TES_EMAIL,
            TRADE_CONTACT = model.TRADE_CONTACT,

            TURNOVER_MTD = model.TURNOVER_MTD,
            TURNOVER_YTD = model.TURNOVER_YTD,
            UNSUBSCRIBE = model.UNSUBSCRIBE,
            USER_NAME = model.USER_NAME,
            VAT_REG_NUMBER = model.VAT_REG_NUMBER,
            WEB_ADDRESS = model.WEB_ADDRESS
        };

        await db.SalesProfileDb.FindOneAndReplaceAsync(filter, modelDB);

        return true;
    }

    public async Task<bool> Add(SalesProfileModel model)
    {
        var db = new DbContext();

        var modelDB = new SalesProfileDB
        {
            ACCOUNT_ON_HOLD = model.ACCOUNT_ON_HOLD,
            ACCOUNT_REF = model.ACCOUNT_REF,
            ACCOUNT_STATUS = model.ACCOUNT_STATUS,
            ACC_ON_HOLD = model.ACC_ON_HOLD,
            ADDRESS_1 = model.ADDRESS_1,
            ADDRESS_2 = model.ADDRESS_2,
            ADDRESS_3 = model.ADDRESS_3,
            ADDRESS_4 = model.ADDRESS_4,
            ADDRESS_5 = model.ADDRESS_5,
            ANALYSIS_1 = model.ANALYSIS_1,
            ANALYSIS_2 = model.ANALYSIS_2,
            ANALYSIS_3 = model.ANALYSIS_3,
            AVERAGE_PAY_DAYS = model.AVERAGE_PAY_DAYS,
            BALANCE = model.BALANCE,
            BANK_ACCOUNT_NAME = model.BANK_ACCOUNT_NAME,
            BANK_ACCOUNT_NUMBER = model.BANK_ACCOUNT_NUMBER,
            BANK_ADDITIONALREF1 = model.BANK_ADDITIONALREF1,
            BANK_ADDITIONALREF2 = model.BANK_ADDITIONALREF2,
            BANK_ADDITIONALREF3 = model.BANK_ADDITIONALREF3,
            BANK_ADDRESS_1 = model.BANK_ADDRESS_1,
            BANK_ADDRESS_2 = model.BANK_ADDRESS_2,
            BANK_ADDRESS_3 = model.BANK_ADDRESS_3,
            BANK_ADDRESS_4 = model.BANK_ADDRESS_4,
            BANK_ADDRESS_5 = model.BANK_ADDRESS_5,
            BANK_BAC = model.BANK_BAC,
            BANK_BIC = model.BANK_BIC,
            BANK_IBAN = model.BANK_IBAN,
            BANK_NAME = model.BANK_NAME,
            BANK_ROLLNUMBER = model.BANK_ROLLNUMBER,
            BANK_SORT_CODE = model.BANK_SORT_CODE,
            BUREAU_CODE = model.BUREAU_CODE,
            CAN_CHARGE = model.CAN_CHARGE,
            CATEGORY = model.CATEGORY,
            COMPANY_ID = model.COMPANY_ID,
            CONTACT_NAME = model.CONTACT_NAME,
            COUNTRY_CODE = model.COUNTRY_CODE,
            COUNTRY_NAME = model.COUNTRY_NAME,
            CREDIT_CHARGE = model.CREDIT_CHARGE,
            CREDIT_LIMIT = model.CREDIT_LIMIT,
            CREDIT_POSITION = model.CREDIT_POSITION,
            CREDIT_POS_CODE = model.CREDIT_POS_CODE,
            CREDIT_REF = model.CREDIT_REF,
            CURRENCY = model.CURRENCY,
            CURRENCY_NAME = model.CURRENCY_NAME,
            CURRENCY_NUMBER = model.CURRENCY_NUMBER,
            C_ADDRESS_1 = model.C_ADDRESS_1,
            C_ADDRESS_2 = model.C_ADDRESS_2,
            C_ADDRESS_3 = model.C_ADDRESS_3,
            C_ADDRESS_4 = model.C_ADDRESS_4,
            C_ADDRESS_5 = model.C_ADDRESS_5,
            DATE_ACCOUNT_OPENED = model.DATE_ACCOUNT_OPENED,
            DATE_AC_OPENED = model.DATE_AC_OPENED,
            DATE_CREDIT_APPLIED = model.DATE_CREDIT_APPLIED,
            DATE_CREDIT_RECEIVED = model.DATE_CREDIT_RECEIVED,
            DATE_LAST_CREDIT = model.DATE_LAST_CREDIT,
            DATE_NEXT_CREDIT = model.DATE_NEXT_CREDIT,
            DEF_NOM_CODE = model.DEF_NOM_CODE,
            DEF_TAX_CODE = model.DEF_TAX_CODE,
            DEL_ADDRESS_1 = model.DEL_ADDRESS_1,
            DEL_ADDRESS_2 = model.DEL_ADDRESS_2,
            DEL_ADDRESS_3 = model.DEL_ADDRESS_3,
            DEL_ADDRESS_4 = model.DEL_ADDRESS_4,
            DEL_ADDRESS_5 = model.DEL_ADDRESS_5,
            DEL_E_MAIL = model.DEL_E_MAIL,
            DEL_FAX = model.DEL_FAX,
            DEL_NAME = model.DEL_NAME,
            DEL_TELEPHONE = model.DEL_TELEPHONE,
            DEPT_NAME = model.DEPT_NAME,
            DEPT_NUMBER = model.DEPT_NUMBER,
            DISCOUNT_ADDITIONAL = model.DISCOUNT_ADDITIONAL,
            DISCOUNT_RATE = model.DISCOUNT_RATE,
            DISCOUNT_TYPE = model.DISCOUNT_TYPE,
            E_MAIL = model.E_MAIL,
            FAX = model.FAX,
            FIRST_INV_DATE = model.FIRST_INV_DATE,
            PRICING_REF = model.PRICING_REF,
            PAYMENT_DUE_DAYS = model.PAYMENT_DUE_DAYS,
            PORTAL = model.PORTAL,
            LAST_INV_DATE = model.LAST_INV_DATE,
            LAST_PAYMENT_DATE = model.LAST_PAYMENT_DATE,
            LAST_UPDATED = model.LAST_UPDATED,
            MEMO = model.MEMO,
            MEMO_2 = model.MEMO_2,
            NAME = model.NAME,
            NOMINAL_NAME = model.NOMINAL_NAME,
            PORTAL_EMAIL = model.PORTAL_EMAIL,
            OVERRIDE_TAX_CODE = model.OVERRIDE_TAX_CODE,
            PRIOR_YEAR = model.PRIOR_YEAR,
            TELEPHONE = model.TELEPHONE,
            RESTRICT_MAIL = model.RESTRICT_MAIL,
            RESTRICT_MAIL2 = model.RESTRICT_MAIL2,
            RESTRICT_NOMINAL_CODE = model.RESTRICT_NOMINAL_CODE,
            RESTRICT_TAX_CODE = model.RESTRICT_TAX_CODE,
            SALES_LEVEL = model.SALES_LEVEL,
            SETTLEMENT_DISC_RATE = model.SETTLEMENT_DISC_RATE,
            SETTLEMENT_DUE_DAYS = model.SETTLEMENT_DUE_DAYS,
            STATUS = model.STATUS,
            STATUS_NUMBER = model.STATUS_NUMBER,
            STATUS_TEXT = model.STATUS_TEXT,
            TAX_NAME = model.TAX_NAME,
            TELEPHONE_2 = model.TELEPHONE_2,
            TERMS = model.TERMS,
            TERMS_AGREED = model.TERMS_AGREED,
            TES_EMAIL = model.TES_EMAIL,
            TRADE_CONTACT = model.TRADE_CONTACT,

            TURNOVER_MTD = model.TURNOVER_MTD,
            TURNOVER_YTD = model.TURNOVER_YTD,
            UNSUBSCRIBE = model.UNSUBSCRIBE,
            USER_NAME = model.USER_NAME,
            VAT_REG_NUMBER = model.VAT_REG_NUMBER,
            WEB_ADDRESS = model.WEB_ADDRESS
        };

        await db.SalesProfileDb.InsertOneAsync(modelDB);

        return true;
    }


    public async Task<int> GetCount()
    {
        var db = new DbContext();
        var cursor = await db.CustomerProfileDb.FindAsync(new BsonDocument());

        IList<CustomerProfileDB> results = cursor.ToList();

        return results.Count;
    }

    public async Task<bool> ConvertData()
    {
        var db = new DbContext();
        var cursor = await db.CustomerProfileDb.FindAsync(new BsonDocument());

        IList<CustomerProfileDB> results = cursor.ToList();


        foreach (var model in results)
        {
            var newModel = new SalesProfileDB
            {
                //_id = RecordId,
                ACCOUNT_ON_HOLD = model.ACCOUNT_ON_HOLD,
                ACCOUNT_REF = model.ACCOUNT_REF,
                ACCOUNT_STATUS = model.ACCOUNT_STATUS,
                ACC_ON_HOLD = model.ACC_ON_HOLD,
                ADDRESS_1 = model.ADDRESS_1,
                ADDRESS_2 = model.ADDRESS_2,
                ADDRESS_3 = model.ADDRESS_3,
                ADDRESS_4 = model.ADDRESS_4,
                ADDRESS_5 = model.ADDRESS_5,
                ANALYSIS_1 = model.ANALYSIS_1,
                ANALYSIS_2 = model.ANALYSIS_2,
                ANALYSIS_3 = model.ANALYSIS_3,
                AVERAGE_PAY_DAYS = model.AVERAGE_PAY_DAYS,
                BALANCE = model.BALANCE,
                BANK_ACCOUNT_NAME = model.BANK_ACCOUNT_NAME,
                BANK_ACCOUNT_NUMBER = model.BANK_ACCOUNT_NUMBER,
                BANK_ADDITIONALREF1 = model.BANK_ADDITIONALREF1,
                BANK_ADDITIONALREF2 = model.BANK_ADDITIONALREF2,
                BANK_ADDITIONALREF3 = model.BANK_ADDITIONALREF3,
                BANK_ADDRESS_1 = model.BANK_ADDRESS_1,
                BANK_ADDRESS_2 = model.BANK_ADDRESS_2,
                BANK_ADDRESS_3 = model.BANK_ADDRESS_3,
                BANK_ADDRESS_4 = model.BANK_ADDRESS_4,
                BANK_ADDRESS_5 = model.BANK_ADDRESS_5,
                BANK_BAC = model.BANK_BAC,
                BANK_BIC = model.BANK_BIC,
                BANK_IBAN = model.BANK_IBAN,
                BANK_NAME = model.BANK_NAME,
                BANK_ROLLNUMBER = model.BANK_ROLLNUMBER,
                BANK_SORT_CODE = model.BANK_SORT_CODE,
                BUREAU_CODE = model.BUREAU_CODE,
                CAN_CHARGE = model.CAN_CHARGE,
                CATEGORY = model.CATEGORY,
                COMPANY_ID = model.COMPANY_ID,
                CONTACT_NAME = model.CONTACT_NAME,
                COUNTRY_CODE = model.COUNTRY_CODE,
                COUNTRY_NAME = model.COUNTRY_NAME,
                CREDIT_CHARGE = model.CREDIT_CHARGE,
                CREDIT_LIMIT = model.CREDIT_LIMIT,
                CREDIT_POSITION = model.CREDIT_POSITION,
                CREDIT_POS_CODE = model.CREDIT_POS_CODE,
                CREDIT_REF = model.CREDIT_REF,
                CURRENCY = model.CURRENCY,
                CURRENCY_NAME = model.CURRENCY_NAME,
                CURRENCY_NUMBER = model.CURRENCY_NUMBER,
                C_ADDRESS_1 = model.C_ADDRESS_1,
                C_ADDRESS_2 = model.C_ADDRESS_2,
                C_ADDRESS_3 = model.C_ADDRESS_3,
                C_ADDRESS_4 = model.C_ADDRESS_4,
                C_ADDRESS_5 = model.C_ADDRESS_5,

                DEF_NOM_CODE = model.DEF_NOM_CODE,
                DEF_TAX_CODE = model.DEF_TAX_CODE,
                DEL_ADDRESS_1 = model.DEL_ADDRESS_1,
                DEL_ADDRESS_2 = model.DEL_ADDRESS_2,
                DEL_ADDRESS_3 = model.DEL_ADDRESS_3,
                DEL_ADDRESS_4 = model.DEL_ADDRESS_4,
                DEL_ADDRESS_5 = model.DEL_ADDRESS_5,
                DEL_E_MAIL = model.DEL_E_MAIL,
                DEL_FAX = model.DEL_FAX,
                DEL_NAME = model.DEL_NAME,
                DEL_TELEPHONE = model.DEL_TELEPHONE,
                DEPT_NAME = model.DEPT_NAME,
                DEPT_NUMBER = model.DEPT_NUMBER,
                DISCOUNT_ADDITIONAL = model.DISCOUNT_ADDITIONAL,
                DISCOUNT_RATE = model.DISCOUNT_RATE,
                DISCOUNT_TYPE = model.DISCOUNT_TYPE,
                E_MAIL = model.E_MAIL,
                FAX = model.FAX,

                PRICING_REF = model.PRICING_REF,
                PAYMENT_DUE_DAYS = model.PAYMENT_DUE_DAYS,
                PORTAL = model.PORTAL,

                MEMO = model.MEMO,
                MEMO_2 = model.MEMO_2,
                NAME = model.NAME,
                NOMINAL_NAME = model.NOMINAL_NAME,
                PORTAL_EMAIL = model.PORTAL_EMAIL,
                OVERRIDE_TAX_CODE = model.OVERRIDE_TAX_CODE,
                PRIOR_YEAR = model.PRIOR_YEAR,
                TELEPHONE = model.TELEPHONE,
                RESTRICT_MAIL = model.RESTRICT_MAIL,
                RESTRICT_MAIL2 = model.RESTRICT_MAIL2,
                RESTRICT_NOMINAL_CODE = model.RESTRICT_NOMINAL_CODE,
                RESTRICT_TAX_CODE = model.RESTRICT_TAX_CODE,
                SALES_LEVEL = model.SALES_LEVEL,
                SETTLEMENT_DISC_RATE = model.SETTLEMENT_DISC_RATE,
                SETTLEMENT_DUE_DAYS = model.SETTLEMENT_DUE_DAYS,
                STATUS = model.STATUS,
                STATUS_NUMBER = model.STATUS_NUMBER,
                STATUS_TEXT = model.STATUS_TEXT,
                TAX_NAME = model.TAX_NAME,
                TELEPHONE_2 = model.TELEPHONE_2,
                TERMS = model.TERMS,
                TERMS_AGREED = model.TERMS_AGREED,
                TES_EMAIL = model.TES_EMAIL,
                TRADE_CONTACT = model.TRADE_CONTACT,

                TURNOVER_MTD = model.TURNOVER_MTD,
                TURNOVER_YTD = model.TURNOVER_YTD,
                UNSUBSCRIBE = model.UNSUBSCRIBE,
                USER_NAME = model.USER_NAME,
                VAT_REG_NUMBER = model.VAT_REG_NUMBER,
                WEB_ADDRESS = model.WEB_ADDRESS
            };

            if (model.ACCOUNT_REF.Equals("NATHAN"))
            {
            }

            var dTime = new DateTime(1980, 1, 1);
            if (!string.IsNullOrEmpty(model.DATE_ACCOUNT_OPENED))
                newModel.DATE_ACCOUNT_OPENED = ConvertToDate(model.DATE_ACCOUNT_OPENED);
            else
                newModel.DATE_ACCOUNT_OPENED = dTime;

            if (!string.IsNullOrEmpty(model.DATE_AC_OPENED))
                newModel.DATE_AC_OPENED = ConvertToDate(model.DATE_AC_OPENED);
            else
                newModel.DATE_AC_OPENED = dTime;

            if (!string.IsNullOrEmpty(model.DATE_CREDIT_APPLIED))
                newModel.DATE_CREDIT_APPLIED = ConvertToDate(model.DATE_CREDIT_APPLIED);
            else
                newModel.DATE_CREDIT_APPLIED = dTime;

            if (!string.IsNullOrEmpty(model.DATE_CREDIT_RECEIVED))
                newModel.DATE_CREDIT_RECEIVED = ConvertToDate(model.DATE_CREDIT_RECEIVED);
            else
                newModel.DATE_CREDIT_RECEIVED = dTime;

            if (!string.IsNullOrEmpty(model.DATE_LAST_CREDIT))
                newModel.DATE_LAST_CREDIT = ConvertToDate(model.DATE_LAST_CREDIT);
            else
                newModel.DATE_LAST_CREDIT = dTime;

            if (!string.IsNullOrEmpty(model.DATE_NEXT_CREDIT))
                newModel.DATE_NEXT_CREDIT = ConvertToDate(model.DATE_NEXT_CREDIT);
            else
                newModel.DATE_NEXT_CREDIT = dTime;

            if (!string.IsNullOrEmpty(model.FIRST_INV_DATE))
                newModel.FIRST_INV_DATE = ConvertToDate(model.FIRST_INV_DATE);
            else
                newModel.FIRST_INV_DATE = dTime;

            if (!string.IsNullOrEmpty(model.LAST_INV_DATE))
                newModel.LAST_INV_DATE = ConvertToDate(model.LAST_INV_DATE);
            else
                newModel.LAST_INV_DATE = dTime;

            if (!string.IsNullOrEmpty(model.LAST_PAYMENT_DATE))
                newModel.LAST_PAYMENT_DATE = ConvertToDate(model.LAST_PAYMENT_DATE);
            else
                newModel.LAST_PAYMENT_DATE = dTime;


            if (!string.IsNullOrEmpty(model.LAST_UPDATED))
                newModel.LAST_UPDATED = ConvertToDate(model.LAST_UPDATED);
            else
                newModel.LAST_UPDATED = dTime;


            newModel.AllowedSourceCountries = new List<string>();
            newModel.AllowedMarketCountries = new List<string>();
            newModel.AllowedTariffCodes = new List<string>();
            await db.SalesProfileDb.InsertOneAsync(newModel);
        }

        return true;
    }

    private DateTime ConvertToDate(string szDate)
    {
        var words = szDate.Split('/');

        var rDate = DateTime.Now;

        try
        {
            var Day = int.Parse(words[0]);
            var Month = int.Parse(words[1]);
            var Year = int.Parse(words[2].Substring(0, 4));
            rDate = new DateTime(Year, Month, Day);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }


        return rDate;
    }
}