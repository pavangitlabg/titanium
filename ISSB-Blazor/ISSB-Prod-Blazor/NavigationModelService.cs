using Data.Models;

namespace ISSB_Prod_Blazor;

public class NavigationModelService
{
    public UserModel CurrentUserModel { get; set; }
    public GridIndexModel CurrentGridModel { get; set; }
}