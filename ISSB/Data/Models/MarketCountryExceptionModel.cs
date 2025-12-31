using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class MarketCountryExceptionModel
    {
        public string _id { get; set; }
        [Display(Name = "Market Country Id")]
        public int MARKET_COUNTRY_ID { get; set; }        [Display(Name = "Geo Code")]        [Required(ErrorMessage = "Empty Geo Code")]        [RegularExpression(@"^(\d{3})$", ErrorMessage = "Please enter 3 digit number")]
        public string GEO_CODE { get; set; }        [Display(Name = "Name Old")]        public string NAME_OLD { get; set; }        [Display(Name = "Short Legend")]        public string SHORT_LEGEND { get; set; }        [Display(Name = "Long Legend")]        public string LONG_LEGEND { get; set; }        [Display(Name = "Region Name")]        public string REGION_NAME { get; set; }        [Display(Name = "Source Country Indicator")]        public string SOURCE_COUNTRY_INDICATOR { get; set; }        [Display(Name = "Replaced Geo Code")]        public string REPLACED_GEO_CODE { get; set; }        [DataType(DataType.Date)]        public DateTime START_DATE { get; set; }        [DataType(DataType.Date)]        public DateTime DISCONTINUED_DATE { get; set; }        [Display(Name = "Name")]        public string NAME { get; set; }
        [Display(Name = "Active")]
        [Required(ErrorMessage = "Empty active field")]
        public bool ACTIVE { get; set; }
    }
}
