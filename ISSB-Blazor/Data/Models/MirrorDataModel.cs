using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class MirrorDataModel
{
    [Display(Name = "Source Country GEO")] public string GeoCode { get; set; }

    [Display(Name = "Year")] public int Year { get; set; }

    [Display(Name = "Month")] public int Month { get; set; }

    [Display(Name = "Tariff Codes")] public string TariffCodes { get; set; }
}