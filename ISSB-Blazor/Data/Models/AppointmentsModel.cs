using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class AppointmentsModel
{
    public string _id { get; set; }

    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    [Display(Name = "Title")]
    [Required(ErrorMessage = "Title Required")]
    public string Title { get; set; }

    [Display(Name = "Description")]
    [Required(ErrorMessage = "Description Required")]
    public string Description { get; set; }

    [Display(Name = "Appointment Notes")] public string Notes { get; set; }

    [Display(Name = "Publish on Web Site")]
    public bool IsPublished { get; set; }
}