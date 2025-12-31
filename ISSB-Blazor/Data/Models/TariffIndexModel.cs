using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class TariffIndexModel
{
    public string _id { get; set; }

    [Display(Name = "Code")]
    [Required(ErrorMessage = "Code is required")]
    public string Code { get; set; }

    [Display(Name = "Name")]
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Display(Name = "Description")] public string Description { get; set; }

    [Display(Name = "Active / Not Active")]
    public bool Active { get; set; }
}