using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class PortsModel
{
    public string _id { get; set; }

    [Display(Name = "Port ID")] public int PortID { get; set; }

    [Display(Name = "Alpha Code")] public string AlphaCode { get; set; }

    [Display(Name = "Port Name")]
    [Required(ErrorMessage = "Empty Port name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Country GEO Code")]
    public string GeoCode { get; set; }
}

public class PortJsonModel
{
    public List<PortsModel> data { get; set; }
}