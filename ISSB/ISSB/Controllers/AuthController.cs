using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Services;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using Data.Enums;
using ISSB.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Twilio.TwiML.Voice;

namespace ISSB.Controllers
{
    public class AuthController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Login";
                return View("Login", loginModel);
            }

            var srv = new UserServices();
            var UserModel = await srv.LoginUser(loginModel.Email.ToLower(), loginModel.Password);

            if (UserModel.Password == null)
            {
                ViewData["Message"] = "Invalid user name or password, Please contact your Administrator.";
                return View("Login", loginModel);
            }

            int dResult = DateTime.Compare(UserModel.ExpiryDate, DateTime.Now);

            if (dResult < 0)
            {
                ViewData["Message"] = "Your account has expired, Please contact your Administrator.";
                return View("Login", loginModel);
            }

            if (UserModel.AccountStatus.Equals("Closed"))
            {
                ViewData["Message"] = "Your account has been Closed, Please contact your Administrator.";
                return View("Login", loginModel);
            }

            if (!UserModel.IsLoggedIn)
            {
                ViewData["Message"] = "Invalid Username or Password";
                return View("Login", loginModel);
            }
            else
            {
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
                    return View("AuthCode", loginModel);
                }

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

                UserModel = await srv.GetUsersById(UserModel._id);
                UserModel.Password = EncryptionHelper.Encrypt(UserModel.Password);

                var EventModel = new SystemEventLogsModel
                {
                    DataType = DataTypeEnums.UserModel,
                    TransactionType = TransactionTypeEnums.Edit,
                    MESSAGE = "System Login, [" + UserModel.Email + "]",
                    DATE = DateTime.Now,
                    USER = UserModel.Email,
                    Model = UserModel
                };
                new EventLog(EventModel);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

        }

        public IActionResult AuthCode(LoginModel model)
        {
            ViewData["Message"] = "Invalid Verification Code";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Verify(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.ValidationCode))
            {
                model.ValidationCode = string.Empty;
            }

            if (!model.ValidationCode.Equals(model.SMSCode))
            {
                ViewData["Message"] = "Invalid Login";

                return RedirectToAction("AuthCode", "Auth", model);
                // return View("AuthCode", model);
            }

            var srv = new UserServices();
            var UserModel = await srv.LoginUser(model.Email, model.Password);

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
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout(UserModel userModel)
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var UserSrv = new UserServices();
            var UserModel = await UserSrv.GetUser(UserId);
            UserModel.Password = EncryptionHelper.Encrypt(UserModel.Password);
            UserModel = await UserSrv.GetUsersById(UserModel._id);
            UserModel.Password = EncryptionHelper.Encrypt(UserModel.Password);

            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.UserModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "System Logout, [" + UserModel.Email + "]",
                DATE = DateTime.Now,
                USER = UserModel.Email,
                Model = UserModel
            };

            new EventLog(EventModel);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
