using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class HomeModel
{
    public string _id { get; set; }

    [Display(Name = "First Name*")]
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }

    [Display(Name = "Last Name*")]
    [Required(ErrorMessage = "Last Name is required")]
    public string Lastname { get; set; }

    [Display(Name = "Company")] public string Company { get; set; }

    [Display(Name = "How did you hear about us?*")]
    [Required(ErrorMessage = "This is required")]
    public string Info { get; set; }

    [Display(Name = "Address 1")] public string Address1 { get; set; }

    [Display(Name = "Address 2")] public string Address2 { get; set; }

    [Display(Name = "City")] public string City { get; set; }

    [Display(Name = "State")] public string State { get; set; }

    [Display(Name = "Zip")] public string Zip { get; set; }

    [Display(Name = "Phone")] public string Phone { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$",
        ErrorMessage = "Please enter a valid email")]
    [Display(Name = "Email*")]
    public string Email { get; set; }

    [Display(Name = "Comment*")]
    [Required(ErrorMessage = "This is required")]
    public string Comment { get; set; }

    [Display(Name = "Contact Date")]
    [DataType(DataType.Date)]
    public DateTime ContactDate { get; set; }

    [Display(Name = "Contact Status")] public bool Status { get; set; }
}