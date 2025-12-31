using System;
using System.ComponentModel.DataAnnotations;
namespace Data.Models
{
    public class SourceCountryExceptionModel
    {
        public string _id { get; set; }
        [Display(Name = "Source Country ID")]
        public int SOURCE_COUNTRY_ID { get; set; }        [Display(Name = "Geo Code")]        public string GEO_CODE { get; set; }        [Display(Name = "Name Old")]        public string NAME_OLD { get; set; }        [Display(Name = "Short Legend")]        public string SHORT_LEGEND { get; set; }        [Display(Name = "Long Legend")]        public string LONG_LEGEND { get; set; }        [Display(Name = "Region Name")]        public string REGION_NAME { get; set; }        [Display(Name = "Source Country Indicator")]        public string SOURCE_COUNTRY_INDICATOR { get; set; }        [Display(Name = "Replaced Geo Code")]        public string REPLACED_GEO_CODE { get; set; }        [Display(Name = "Start Date")]        [DataType(DataType.Date)]        public DateTime START_DATE { get; set; }        [Display(Name = "Discontinued Date")]        [DataType(DataType.Date)]        public DateTime DISCONTINUED_DATE { get; set; }        [Display(Name = "Name")]        public string NAME { get; set; }
        [Display(Name = "Active")]
        public bool ACTIVE { get; set; }
    }
}
