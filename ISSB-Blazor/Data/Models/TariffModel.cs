using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class TariffModel
{
    public string _id { get; set; }

    [Display(Name = "Tariff ID")] public int TARIFF_ID { get; set; }

    [Display(Name = "World Trade Organisation Code")]
    public string WTO_CODE { get; set; }

    [Display(Name = "World Trade Organisation Code Legend")]
    public string WTO_CODE_LEGEND { get; set; }

    [Display(Name = "World Trade Organisation Alloy Code")]
    public string WTO_ALLOY_CODE { get; set; }

    [Display(Name = "World Trade Organisation Legend")]
    public string WTO_ALLOY_LEGEND { get; set; }

    [Display(Name = "ISSB Stored Code")] public string ISSB_STORED_CODE { get; set; }

    [Display(Name = "ISSB Stored Legend")] public string ISSB_STORED_LEGEND { get; set; }

    [Display(Name = "Harmonised Tariff Code")]
    [Required(ErrorMessage = "Harmonised Tariff Code required.")]
    public string HARMONISED_TARIFF_CODE { get; set; }

    [Display(Name = "Harmonised Tariff Short Legend Backup")]
    public string HARMONISED_TARIFF_SHORT_LEGEND_BACKUP { get; set; }

    [Display(Name = "Harmonised Tariff Long Legend")]
    public string HARMONISED_TARIFF_LONG_LEGEND { get; set; }

    [Display(Name = "Source Country Tariff Code")]
    public string SOURCE_COUNTRY_TARIFF_CODE { get; set; }

    [Display(Name = "Source Country Tariff Short Legend")]
    public string SOURCE_COUNTRY_TARIFF_SHORT_LEGEND { get; set; }

    [Display(Name = "Source Country Tariff Long Legend")]
    public string SOURCE_COUNTRY_TARIFF_LONG_LEGEND { get; set; }

    [Display(Name = "Lower VPT")] public string LOWER_VPT { get; set; }

    [Display(Name = "Upper VPT")] public string UPPER_VPT { get; set; }

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime START_DATE { get; set; }

    [Display(Name = "Discontinued Date")]
    [DataType(DataType.Date)]
    public DateTime DISCONTINUED_DATE { get; set; }

    [Display(Name = "Side of Trade")] public string SIDE_OF_TRADE { get; set; }

    [Display(Name = "Region Code")]
    [Required(ErrorMessage = "Region Code required.")]
    public string TARIFF_CODE_TABLE_CODE { get; set; }

    [Display(Name = "Harmonised Tariff Short Legend")]
    public string HARMONISED_TARIFF_SHORT_LEGEND { get; set; }
}

public class TariffShortModel
{
    public string _id { get; set; }
    public string SOURCE_COUNTRY_TARIFF_CODE { get; set; }
    public string SOURCE_COUNTRY_TARIFF_SHORT_LEGEND { get; set; }
    public string WTO_CODE { get; set; }
}