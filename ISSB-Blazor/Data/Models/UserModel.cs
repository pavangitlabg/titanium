using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.Models;

public class UserModel
{
    public string _id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Display(Name = "User Name")]
    [Required(ErrorMessage = "Username is required.")]
    [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
    public string UserName { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$",
        ErrorMessage = "Please enter a valid email")]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    //[Required(ErrorMessage = "Please enter password"), MaxLength(30)]
    [StringLength(30, ErrorMessage = "Must be between 8 and 30 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{8,30}$",
        ErrorMessage =
            "Password must contain at least one letter, at least one number, and be longer than eight charaters")] //Password must contain at least one letter, at least one number, and be longer than eight charaters.
    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public bool IsLoggedIn { get; set; }

    [Display(Name = "System User")] public bool IsSystemUser { get; set; }

    [Display(Name = "Advanced User")] public bool IsAdvancedUser { get; set; }

    [Display(Name = "Administrator")] public bool IsAdministrator { get; set; }

    [Display(Name = "Trade Inquiry")] public bool IsTradeInquiry { get; set; }

    [Display(Name = "Date Joined")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime DateJoined { get; set; }

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime StartDate { get; set; }

    [Display(Name = "Expiry Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime ExpiryDate { get; set; }

    [Display(Name = "First Name")] public string FirstName { get; set; }

    [Display(Name = "Last Name")] public string LastName { get; set; }

    [Display(Name = "Company Name")] public string CompanyName { get; set; }

    [Display(Name = "Address 1")] public string Address1 { get; set; }

    [Display(Name = "Address 2")] public string Address2 { get; set; }

    [Display(Name = "Address 3")] public string Address3 { get; set; }

    [Display(Name = "Address 4")] public string Address4 { get; set; }

    [Display(Name = "Postal Code")] public string PostalCode { get; set; }

    [Display(Name = "Telephone Number")] public string Telephone { get; set; }

    [Display(Name = "Mobile Number")]
    [Required(ErrorMessage = "Mobile Number is required")]
    public string Mobile { get; set; }

    [Display(Name = "Costs")] public double Costs { get; set; }

    [Display(Name = "Status")] public string AccountStatus { get; set; }

    [Display(Name = "Use Two Factor Authentication")]
    public bool TwoFactorAuth { get; set; }

    [Display(Name = "Email Notifications")]
    public bool EmailNotification { get; set; }

    public string ImageURL { get; set; }
    public string NavBar { get; set; }
    public string BrandLink { get; set; }
    public string SideBar { get; set; }

    [Display(Name = "API Enabled")] public bool IsAPI { get; set; }

    [Display(Name = "Allowed Years")] public string AllowedYears { get; set; }

    public List<ProductCustomFilterModel> AllowedCustomFilters { get; set; }
    public List<object> Permissions { get; set; }
    public List<SourceCountryModel> AllowedSourceCountries { get; set; }
    public List<string> AllowedMarketCountries { get; set; }
    public List<ProductAllFilterModel> AllowedTariffCodes { get; set; }
    public List<SourceCountryModel> SCList { get; set; }
    public List<ProductAllFilterModel> TCList { get; set; }
}

public class UserPasswordModel
{
    public string _id { get; set; }

    [Required(ErrorMessage = "Old Password is required")]
    [Display(Name = "Old Password")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{8,30}$",
        ErrorMessage =
            "Password must contain at least one letter, at least one number, and be longer than eight charaters")]
    public string OldPassword { get; set; }

    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{8,30}$",
        ErrorMessage =
            "Password must contain at least one letter, at least one number, and be longer than eight charaters")]
    [Display(Name = "New Password")]
    [Required(ErrorMessage = "New Password is required")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{8,30}$",
        ErrorMessage =
            "Password must contain at least one letter, at least one number, and be longer than eight charaters")]
    [Display(Name = "Verify")]
    [Required(ErrorMessage = "Verify Password is required")]
    public string VerifyPassword { get; set; }
}