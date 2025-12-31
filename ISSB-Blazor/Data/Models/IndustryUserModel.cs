namespace Data.Models;

public class IndustryUserModel
{
    public string _id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsSystemUser { get; set; }
    public bool IsAdvancedUser { get; set; }
    public bool IsAdministrator { get; set; }
    public bool IsTradeInquiry { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime DateJoined { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyName { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string Address3 { get; set; }
    public string Address4 { get; set; }
    public string PostalCode { get; set; }
    public string Telephone { get; set; }
    public string Mobile { get; set; }
    public double Costs { get; set; }
    public string AccountStatus { get; set; }
    public bool TwoFactorAuth { get; set; }
    public bool EmailNotification { get; set; }
    public string ImageURL { get; set; }
    public bool IsLoggedIn { get; set; }
    public string NavBar { get; set; }
    public string BrandLink { get; set; }
    public string SideBar { get; set; }
}