using System.Security.Claims;
using System.Text.RegularExpressions;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace ISSB_Prod_Blazor.Areas.Identity.Pages.Account;

public class UserLoginModel : PageModel
{
    [BindProperty] public LoginModel loginModel { get; set; }

    public string ReturnUrl { get; set; }

    public void OnGet()
    {
        ReturnUrl = Url.Content("~/");

        loginModel = new LoginModel();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var srv = new UserServices();
        var UserModel = await srv.LoginUser(loginModel.Email.ToLower(), loginModel.Password);

        if (UserModel.Password == null)
        {
            ViewData["Message"] = "Invalid user name or password, Please contact your Administrator.";
            //return View("Login", loginModel);
            return Page();
        }

        var dResult = DateTime.Compare(UserModel.ExpiryDate, DateTime.Now);

        if (dResult < 0)
        {
            ViewData["Message"] = "Your account has expired, Please contact your Administrator.";
            // return View("Login", loginModel);
            return Page();
        }

        if (UserModel.AccountStatus.Equals("Closed"))
        {
            ViewData["Message"] = "Your account has been Closed, Please contact your Administrator.";
            //return View("Login", loginModel);
            return Page();
        }

        if (!UserModel.IsLoggedIn)
        {
            ViewData["Message"] = "Invalid Username or Password";
            // return View("Login", loginModel);
            return Page();
        }

        if (UserModel.TwoFactorAuth)
        {
            var code = RandomNumber(1000, 9999);
            var controlService = new SystemControlService();
            var cntModel = await controlService.GetSystemControl();
            var smsService = new SMSService();

            var msgSrv = new SMSService();
            var messages = new List<SMSMessage>();
            var smsText = new SMSMessage
            {
                from = "ISSB",
                to = UserModel.Mobile.Trim(),
                source = "ISSB",
                body = cntModel.SMSMessage.Trim() + " " + code
            };
            messages.Add(smsText);

            var message = new SMSRequestModel
            {
                messages = messages
            };
            var responseMsg = await msgSrv.SendSMS(message);

            // smsService.SendSMS(UserModel.Mobile.Trim(), cntModel.SMSNumber, cntModel.SMSMessage.Trim() + " " + code);

            loginModel.SMSCode = code.ToString();
            //return View("AuthCode", loginModel);
            return Page();
        }

        var Actor = string.Empty;
        var AdvancedUser = string.Empty;
        var IsSet = false;

        if (UserModel.IsAdministrator)
        {
            Actor = "Administrator";
            IsSet = true;
        }

        if (!IsSet)
            if (UserModel.IsSystemUser)
            {
                Actor = "SystemUser";
                IsSet = true;
            }

        if (!IsSet)
            if (UserModel.IsTradeInquiry)
            {
                Actor = "Tes";
                IsSet = true;
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

        UserModel = await srv.GetUsersById(UserModel._id);
        UserModel.Password = EncryptionHelper.Encrypt(UserModel.Password);

        // var EventModel = new SystemEventLogsModel
        // {
        //     DataType = DataTypeEnums.UserModel,
        //     TransactionType = TransactionTypeEnums.Edit,
        //     MESSAGE = "System Login, [" + UserModel.Email + "]",
        //     DATE = DateTime.Now,
        //     USER = UserModel.Email,
        //     Model = UserModel
        // };
        // new EventLog(EventModel);

        //return RedirectToPage("https://localhost:17838/");
        return RedirectToPage(ReturnUrl);
    }

    private int RandomNumber(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max);
    }

    public static bool IsValidPassword(string plainText)
    {
        var regex = new Regex(@"^(.{0,7}|[^0-9]*|[^A-Z])$");
        var match = regex.Match(plainText);
        return match.Success;
    }
}