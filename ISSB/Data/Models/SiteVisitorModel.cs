using System;
using System.ComponentModel.DataAnnotations;
namespace Data.Models
{
    public class SiteVisitorModel
    {
        public string _id { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Country")]
        public string country { get; set; }
        [Display(Name = "Country Code")]
        public string countryCode { get; set; }
        [Display(Name = "Region")]
        public string region { get; set; }
        [Display(Name = "Region Name")]
        public string regionName { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Zip")]
        public string zip { get; set; }
        [Display(Name = "Latitude")]
        public double lat { get; set; }
        [Display(Name = "Longitude")]
        public double lon { get; set; }
        [Display(Name = "Timezone")]
        public string timezone { get; set; }
        [Display(Name = "ISP")]
        public string isp { get; set; }
        [Display(Name = "Organization")]
        public string org { get; set; }
        [Display(Name = "Association")]
        public string @as { get; set; }
        [Display(Name = "Query")]
        public string query { get; set; }
        [Display(Name = "IP Address")]
        public string ipAddress { get; set; }
        [Display(Name = "Date of Log")]
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class JsonSiteVisitorModel
    {
        public string _id { get; set; }
        public SiteVisitorModel siteVisitorModel { get; set; }
    }
}
