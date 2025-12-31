using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class SourceCountryMappingModel
{
    public string _id { get; set; }

    [Display(Name = "Source Country ID")] public string Source_Country_ID { get; set; }

    [Display(Name = "Source Country Value")]
    public string Source_Country_Value { get; set; }

    [Display(Name = "ISSB Country Geo Code")]
    public string ISSB_Country_Geo_Code { get; set; }

    [Display(Name = "Country Name")] public string COUNTRY_NAME { get; set; }
}