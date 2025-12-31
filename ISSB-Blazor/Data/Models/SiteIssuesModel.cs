using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class SiteIssuesModel
{
    public string _id { get; set; }

    [Display(Name = "Request ID")] public int RequestID { get; set; }

    [Display(Name = "User")] public string User { get; set; }

    [Display(Name = "Subject")] public string Subject { get; set; }

    [Display(Name = "Description")] public string Description { get; set; }

    [Display(Name = "Response")] public string Comment { get; set; }

    [Display(Name = "Status")] public string Status { get; set; }

    [Display(Name = "Severity")] public string Severity { get; set; }

    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$",
        ErrorMessage = "Please enter a valid email")]
    public string Email { get; set; }

    [Display(Name = "Timestamp")]
    [DataType(DataType.Date)]
    public DateTime Timestamp { get; set; }
}