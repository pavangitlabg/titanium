using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class SalesProfileModel
{
    public string _id { get; set; }
    public int COMPANY_ID { get; set; }
    public string ACCOUNT_REF { get; set; }
    public string NAME { get; set; }
    public string ADDRESS_1 { get; set; }
    public string ADDRESS_2 { get; set; }
    public string ADDRESS_3 { get; set; }
    public string ADDRESS_4 { get; set; }
    public string ADDRESS_5 { get; set; }
    public string C_ADDRESS_1 { get; set; }
    public string C_ADDRESS_2 { get; set; }
    public string C_ADDRESS_3 { get; set; }
    public string C_ADDRESS_4 { get; set; }
    public string C_ADDRESS_5 { get; set; }
    public string CONTACT_NAME { get; set; }
    public string TELEPHONE { get; set; }
    public string TELEPHONE_2 { get; set; }
    public string FAX { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$",
        ErrorMessage = "Please enter a valid email")]
    public string E_MAIL { get; set; }

    public string WEB_ADDRESS { get; set; }
    public string TRADE_CONTACT { get; set; }
    public string DEL_NAME { get; set; }
    public string DEL_ADDRESS_1 { get; set; }
    public string DEL_ADDRESS_2 { get; set; }
    public string DEL_ADDRESS_3 { get; set; }
    public string DEL_ADDRESS_4 { get; set; }
    public string DEL_ADDRESS_5 { get; set; }
    public string DEL_TELEPHONE { get; set; }
    public string DEL_FAX { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$",
        ErrorMessage = "Please enter a valid email")]
    public string DEL_E_MAIL { get; set; }

    public string ANALYSIS_1 { get; set; }
    public string ANALYSIS_2 { get; set; }
    public string ANALYSIS_3 { get; set; }
    public int DEPT_NUMBER { get; set; }
    public string DEPT_NAME { get; set; }
    public int STATUS_NUMBER { get; set; }
    public string STATUS_TEXT { get; set; }
    public string DEF_TAX_CODE { get; set; }
    public string TAX_NAME { get; set; }
    public string DEF_NOM_CODE { get; set; }
    public string NOMINAL_NAME { get; set; }
    public string VAT_REG_NUMBER { get; set; }
    public int CURRENCY_NUMBER { get; set; }
    public string CURRENCY { get; set; }
    public string CURRENCY_NAME { get; set; }
    public string COUNTRY_CODE { get; set; }
    public string COUNTRY_NAME { get; set; }
    public int DISCOUNT_TYPE { get; set; }
    public string DISCOUNT_ADDITIONAL { get; set; }
    public double DISCOUNT_RATE { get; set; }
    public string PRICING_REF { get; set; }
    public double SETTLEMENT_DISC_RATE { get; set; }
    public int SETTLEMENT_DUE_DAYS { get; set; }
    public int PAYMENT_DUE_DAYS { get; set; }
    public double CREDIT_LIMIT { get; set; }
    public int ACCOUNT_ON_HOLD { get; set; }
    public string TERMS { get; set; }
    public string RESTRICT_NOMINAL_CODE { get; set; }
    public string RESTRICT_TAX_CODE { get; set; }
    public string RESTRICT_MAIL { get; set; }
    public string TERMS_AGREED { get; set; }
    public DateTime DATE_AC_OPENED { get; set; }
    public DateTime DATE_ACCOUNT_OPENED { get; set; }
    public DateTime DATE_NEXT_CREDIT { get; set; }
    public DateTime DATE_LAST_CREDIT { get; set; }
    public string CREDIT_POSITION { get; set; }
    public int CREDIT_POS_CODE { get; set; }
    public int CAN_CHARGE { get; set; }
    public int BUREAU_CODE { get; set; }
    public string CREDIT_REF { get; set; }
    public DateTime DATE_CREDIT_APPLIED { get; set; }
    public DateTime DATE_CREDIT_RECEIVED { get; set; }
    public int OVERRIDE_TAX_CODE { get; set; }
    public DateTime LAST_PAYMENT_DATE { get; set; }
    public DateTime FIRST_INV_DATE { get; set; }
    public int AVERAGE_PAY_DAYS { get; set; }
    public DateTime LAST_INV_DATE { get; set; }
    public double BALANCE { get; set; }
    public double TURNOVER_MTD { get; set; }
    public double TURNOVER_YTD { get; set; }
    public double PRIOR_YEAR { get; set; }
    public string BANK_NAME { get; set; }
    public string BANK_ADDRESS_1 { get; set; }
    public string BANK_ADDRESS_2 { get; set; }
    public string BANK_ADDRESS_3 { get; set; }
    public string BANK_ADDRESS_4 { get; set; }
    public string BANK_ADDRESS_5 { get; set; }
    public string BANK_SORT_CODE { get; set; }
    public string BANK_ACCOUNT_NAME { get; set; }
    public string BANK_ACCOUNT_NUMBER { get; set; }
    public string BANK_BAC { get; set; }
    public string BANK_IBAN { get; set; }
    public string BANK_BIC { get; set; }
    public string BANK_ROLLNUMBER { get; set; }
    public string BANK_ADDITIONALREF1 { get; set; }
    public string BANK_ADDITIONALREF2 { get; set; }
    public string BANK_ADDITIONALREF3 { get; set; }
    public string MEMO { get; set; }
    public string MEMO_2 { get; set; }
    public string CREDIT_CHARGE { get; set; }
    public string RESTRICT_MAIL2 { get; set; }
    public string ACCOUNT_STATUS { get; set; }
    public string ACC_ON_HOLD { get; set; }
    public string USER_NAME { get; set; }
    public DateTime LAST_UPDATED { get; set; }
    public string SALES_LEVEL { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$",
        ErrorMessage = "Please enter a valid email")]
    public string TES_EMAIL { get; set; }

    public string CATEGORY { get; set; }
    public string STATUS { get; set; }
    public string UNSUBSCRIBE { get; set; }
    public string PORTAL { get; set; }
    public string PORTAL_EMAIL { get; set; }

    // To be used for TES Configuration
    public List<string> AllowedSourceCountries { get; set; }
    public List<string> AllowedMarketCountries { get; set; }
    public List<string> AllowedTariffCodes { get; set; }
}

public class SalesProfileJsonModel
{
    public List<SalesProfileModel> data { get; set; }
}