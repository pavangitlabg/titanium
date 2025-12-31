using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class LoginModel
{
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email address is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    public string ValidationCode { get; set; }
    public string SMSCode { get; set; }
    public string Message { get; set; }
}