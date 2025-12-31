using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Syncfusion.Blazor.Notifications;


namespace ISSB.Import.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        public LoginModel() { }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet()
        {
            ReturnUrl = Url.Content("~/");

            Input = new InputModel
            {
                Email = "",
                Password = "",
                OldPassword = string.Empty,
                ConfirmPassword = string.Empty,
                NewPassword = string.Empty,
                IsChange = "N"
            };

        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("~/");

            if (ModelState.IsValid)
            {

                var srv = new UserServices();
                var UserModel = await srv.LoginUser(Input.Email.ToLower(), Input.Password);

                if (UserModel.Password == null)
                {
                    ViewData["Message"] = "Invalid user name or password, Please contact your Administrator.";
                    return Page();
                }

                int dResult = DateTime.Compare(UserModel.ExpiryDate, DateTime.Now);

                if (dResult < 0)
                {
                    ViewData["Message"] = "Your account has expired, Please contact your Administrator.";
                    return Page();
                }

                if (UserModel.AccountStatus.Equals("Closed"))
                {
                    ViewData["Message"] = "Your account has been Closed, Please contact your Administrator.";
                    return Page();
                }

                if (!UserModel.IsLoggedIn)
                {
                    ViewData["Message"] = "Invalid Username or Password";
                    return Page();
                }
                else
                {
                    string Actor = string.Empty;
                    string AdvancedUser = string.Empty;
                    bool IsSet = false;

                    if (UserModel.IsAdministrator)
                    {
                        Actor = "Administrator";
                        IsSet = true;
                    }

                    if (!IsSet)
                    {
                        if (UserModel.IsSystemUser)
                        {
                            Actor = "SystemUser";
                            IsSet = true;
                        }
                    }

                    if (!IsSet)
                    {
                        if (UserModel.IsTradeInquiry)
                        {
                            Actor = "Tes";
                            IsSet = true;
                        }
                    }

                    if (UserModel.IsAdvancedUser)
                    {
                        AdvancedUser = "AdvancedUser";
                        IsSet = true;
                    }

                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, UserModel.UserName),
                        new Claim(ClaimTypes.Email, UserModel.Email),
                        new Claim(ClaimTypes.Sid, UserModel._id),
                        new Claim(ClaimTypes.Actor, Actor),
                        new Claim(ClaimTypes.GroupSid, AdvancedUser)

                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToPage("/_Host");

                }
            }
            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            public string OldPassword { get; set; }


            [DataType(DataType.Password)]
            public string NewPassword { get; set; }


            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }

            public string IsChange { get; set; } = "N";

            public bool ResetPassword { get; set; }
        }
    }
}
