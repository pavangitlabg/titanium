using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class CurrencyCodesModel
    {
        public string _id { get; set; }
        [Display(Name = "Currency Code")]
        public string CURRENCY_CODE { get; set; }
        [Display(Name = "Currency Name")]
        public string CURRENCY_NAME { get; set; }
        [Display(Name = "Currency Symbol")]
        public string CURRENCY_SYMBOL { get; set; }
    }
}
