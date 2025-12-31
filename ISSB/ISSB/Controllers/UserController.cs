using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using System.Collections.Generic;
using Newtonsoft.Json;
using Data.Enums;
using ISSB.Helpers;
using ISSB.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;

namespace ISSB.Controllers
{
    public class UserController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _env;
          [Obsolete]
        public UserController(IHostingEnvironment env)
        {
            _env = env;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
         
            var srv = new UserServices();
            var model = await srv.GetUsers();
            IQueryable data = model.AsQueryable();
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var srv = new UserServices();
            var UserId = User.FindFirst(ClaimTypes.Sid).Value;
            var model = await srv.GetUsersById(UserId);

            var ProfileModel = new UserProfileModel
            {
                _id = model._id,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
                Address4 = model.Address4,
                CompanyName = model.CompanyName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Mobile = model.Mobile,
                PostalCode = model.PostalCode,
                Telephone = model.Telephone,
                UserName = model.UserName,
                EmailNotification = model.EmailNotification,
                ImageURL = model.ImageURL
            };
            return View(ProfileModel);
        }

        [Authorize]
        public async Task<IActionResult> Password()
        {
            var srv = new UserServices();
            var UserId = User.FindFirst(ClaimTypes.Sid).Value;
            var model = await srv.GetUsersById(UserId);

            var ProfileModel = new UserPasswordModel
            {
                _id = model._id,
                NewPassword = string.Empty,
                OldPassword = string.Empty,
                VerifyPassword = string.Empty
            };
            return View(ProfileModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var scSrv = new SourceCountryService();
         
            var srv = new UserServices();
            var model = await srv.GetUsersById(_id);
            model.SCList = await scSrv.GetActiveSourceCountries();

            var tariffSrv = new ProductService();
            model.TCList = await tariffSrv.GetAllProducts();


            ViewBag.TCList = JsonConvert.SerializeObject(model.AllowedTariffCodes);
            ViewBag.UserCostomFilters = JsonConvert.SerializeObject(model.AllowedCustomFilters);

            var cFilterSrv = new ProductCustomFilterService();
            var CoustomFiltersList = await cFilterSrv.GetCustomProducts();

            ViewBag.CostomFilters = JsonConvert.SerializeObject(CoustomFiltersList);

            return View(model);
        }

       
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(UserModel model)
        {
            var scSrv = new SourceCountryService();
            var srv = new UserServices();
            var SCAllowed = await srv.GetUsersById(model._id);
            model.ImageURL = SCAllowed.ImageURL;
            model.AllowedSourceCountries = SCAllowed.AllowedSourceCountries;
            model.AllowedTariffCodes = SCAllowed.AllowedTariffCodes;
            model.AllowedCustomFilters = SCAllowed.AllowedCustomFilters;
            model.SCList = await scSrv.GetActiveSourceCountries();
            var tariffSrv = new ProductService();
            model.TCList = await tariffSrv.GetAllProducts();
            

            ViewBag.TCList = JsonConvert.SerializeObject(model.AllowedTariffCodes);


            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid User Details";
                return View("Edit", model);
            }           

            model.Password = EncryptionHelper.Encrypt(model.Password);
            await srv.UpdateUser(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.UserModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "User Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "User");

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserColours(string RecordId)                                               
        {
            var srv = new UserServices();
            var ColourModel = await srv.GetUserColours(RecordId);
            if (string.IsNullOrEmpty(ColourModel.NavBar))
                ColourModel.NavBar = "main-header navbar navbar-expand border-bottom navbar-light";
            if (string.IsNullOrEmpty(ColourModel.BrandLink))
                ColourModel.BrandLink = "brand-link active navbar-primary";
            if (string.IsNullOrEmpty(ColourModel.SideBar))
                ColourModel.SideBar = "main-sidebar sidebar-dark-primary elevation-4";

            return Json(new { success = true , navbar = ColourModel.NavBar , brandlink = ColourModel.BrandLink , sidebar = ColourModel.SideBar });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateNavBarColour(string RecordId,
                                                            string NavBar)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            modelUpdate.NavBar = NavBar;
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);

            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateBrandLinkColour(string RecordId,
                                                           string BrandLink)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            modelUpdate.BrandLink = BrandLink;
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);

            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateMainSideBarColour(string RecordId,
                                                          string SideBar)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            modelUpdate.SideBar = SideBar;
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);

            return Json(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public async Task<string> UpdateCustoFilters(string RecordId, string Filters)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            var cfSrv = new ProductCustomFilterService();

            var cfList = new List<ProductCustomFilterModel>();

            string[] words = Filters.Split('|');
            foreach (string word in words)
            {
                var scModel = await cfSrv.GetByID(word);
                cfList.Add(scModel);
            }
            modelUpdate.AllowedCustomFilters = cfList;
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);
            cfList = cfList.OrderBy(x => x.SortOrder).ToList();
            var jString = JsonConvert.SerializeObject(cfList);
            return jString;

        }

        [HttpPost]
        [Authorize]
        public async Task<string> RemoveCustomFilters(string RecordId, string Filters)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            //var tariffSrv = new ProductService();

            var cfList = new List<ProductCustomFilterModel>();

            string[] words = Filters.Split('|');
            foreach (string word in words)
            {
           
                modelUpdate.AllowedCustomFilters.RemoveAll((x) => x._id == word);
    
            }
            cfList = modelUpdate.AllowedCustomFilters.OrderBy(x => x._id).ToList();
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);

            return JsonConvert.SerializeObject(cfList);

        }

        [HttpPost]
        [Authorize]
        public async Task<string> UpdateSourceCountries(string RecordId,string Countries)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            var scSrv = new SourceCountryService();

            var scList = new List<SourceCountryModel>();

            string[] words = Countries.Split('|');
            foreach (string word in words)
            {
                var scModel = await scSrv.GetSourceCountryByGeoCode(word);
                scList.Add(scModel);
            }
            modelUpdate.AllowedSourceCountries = scList;
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);
            scList = scList.OrderBy(x => x.GEO_CODE).ToList();
            var jString = JsonConvert.SerializeObject(scList);
            return jString;

        }

        [HttpPost]
        [Authorize]
        public async Task<string> RemoveSourceCountries(string RecordId, string Countries)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            var scSrv = new SourceCountryService();

            var scList = new List<SourceCountryModel>();

            string[] words = Countries.Split('|');
            foreach (string word in words)
            {
                //var scModel = await scSrv.GetSourceCountryByGeoCode(word);
                modelUpdate.AllowedSourceCountries.RemoveAll((x) => x.GEO_CODE == word);
               // modelUpdate.AllowedSourceCountries.Remove(scModel);
            }
            scList = modelUpdate.AllowedSourceCountries.OrderBy(x => x.GEO_CODE).ToList(); 
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);
            
            return JsonConvert.SerializeObject(scList);

        }

        [HttpPost]
        [Authorize]
        public async Task<string> UpdateTariffCodes(string RecordId, string TariffCodes)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            var tariffSrv = new ProductService();

            var tcList = new List<ProductAllFilterModel>();

            string[] words = TariffCodes.Split('|');
            foreach (string word in words)
            {
                var scModel = await tariffSrv.GetProductsByTariffCode(word);
                tcList.Add(scModel);
            }
            modelUpdate.AllowedTariffCodes = tcList;
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);
            tcList = tcList.OrderBy(x => x.TariffCode).ToList();

           // ViewBag.TCList = JsonConvert.SerializeObject(model.AllowedTariffCodes);
            return JsonConvert.SerializeObject(tcList);
        }

        [HttpPost]
        [Authorize]
        public async Task<string> RemoveTariffCodes(string RecordId, string TariffCodes)
        {
            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(RecordId);
            var tariffSrv = new ProductService();

            var scList = new List<ProductAllFilterModel>();

            string[] words = TariffCodes.Split('|');
            foreach (string word in words)
            {
                //var scModel = await scSrv.GetSourceCountryByGeoCode(word);
                modelUpdate.AllowedTariffCodes.RemoveAll((x) => x.TariffCode == word);
                // modelUpdate.AllowedSourceCountries.Remove(scModel);
            }
            scList = modelUpdate.AllowedTariffCodes.OrderBy(x => x.TariffCode).ToList();
            modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            await srv.UpdateUser(modelUpdate);

            return JsonConvert.SerializeObject(scList);

        }

        [HttpPost]
        [Authorize]
        public async Task<string> AllSteelCodes()
        {
          
            var tariffSrv = new ProductService();
            var scList = await tariffSrv.GetAllSteel();
            return JsonConvert.SerializeObject(scList);

        }

        [HttpPost]
        [Authorize]
        //[System.Runtime.InteropServices.ComVisible(false)]
        [RequestSizeLimit(100_000_000)]
        public async Task<ActionResult> Update(UserProfileModel model)
        {
          
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid User Details";
                return View("Profile", model);
            }
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            string filePath = string.Empty;

            filePath = _env.WebRootPath + "/userimages/" + model._id + ".png"; 

            if (model.File != null)
            {
                if (model.File.FileName.Contains(".png"))
                {
                    try { 
                       
                        if(System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        using (var output = new FileStream(filePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(output);
                        }
                    }
                    catch (IOException e)
                    {
                        var eSrv = new ErrorService();
                        var eModel = new ErrorModel { Code = "IMG", Class = "UserController Uploading Image", ErrorMessage = e.Message };
                        await eSrv.UpdateError(eModel);
                    }

                }
            }


            var ImgURL = string.Empty;
            if(model.File == null)
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                ImgURL = "../userimages/default.png";
            }
            else
            {
                ImgURL = "../userimages/" + model._id + ".png";
            }


            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(model._id);
            modelUpdate.UserName = model.UserName;
            modelUpdate.FirstName = model.FirstName;
            modelUpdate.LastName = model.LastName;
            modelUpdate.CompanyName = model.CompanyName;
            modelUpdate.Address1 = model.Address1;
            modelUpdate.Address2 = model.Address2;
            modelUpdate.Address3 = model.Address3;
            modelUpdate.Address4 = model.Address4;
            modelUpdate.PostalCode = model.PostalCode;
            modelUpdate.Telephone = model.Telephone;
            modelUpdate.Mobile = model.Mobile;
            modelUpdate.Email = model.Email;
            modelUpdate.EmailNotification = model.EmailNotification;
            modelUpdate.ImageURL = ImgURL;
            modelUpdate.Password = modelUpdate.Password = EncryptionHelper.Encrypt(modelUpdate.Password);
            

            await srv.UpdateUser(modelUpdate);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePassword(UserPasswordModel model)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Password Details";
                return View("Password", model);
            }

            var srv = new UserServices();
            var modelUpdate = await srv.GetUsersById(model._id);

            if(!modelUpdate.Password.Equals(model.OldPassword))
            {
                ViewData["Message"] = "Old Password does not match";
                return View("Password", model);
            }

            if (!model.NewPassword.Equals(model.VerifyPassword))
            {
                ViewData["Message1"] = "New Password is not verified";
                return View("Password", model);
            }

            modelUpdate.Password = EncryptionHelper.Encrypt(model.NewPassword);

            var emailSrv = new EmailService();
            string body = "From: ISSB System<br/>";
            body += "Email: <br/>";
            body += "Message: <br/>Your System Account password has been changed<br/>";

            await emailSrv.SendEmailTo(modelUpdate.Email, "ISSB System Password Changed", body);
            await srv.UpdateUser(modelUpdate);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new UserServices();
            var modelOut = await srv.GetUsersById(_id);
            await srv.Delete(modelOut);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.UserModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "User Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = modelOut
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "User");

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var ser = new UserServices();
            var nRec = await ser.GetCount();
            var modelOut = new UserModel
            {
                DateJoined = DateTime.Now,
                Email = "",
                IsAdministrator = false,
                IsSystemUser = false,
                //Permissions = true,
                Password = "",
                UserName = "",
                IsTradeInquiry = false,
                StartDate = DateTime.Now,
                ExpiryDate = new DateTime(2021, 1, 1),
                FirstName = "",
                LastName = "",
                CompanyName = "",                
                Address1 = "",
                Address2 = "",
                Address3 = "",
                Address4 = "",
                PostalCode = "",
                Telephone = "",
                Mobile = "",
                Costs = 0,
                AccountStatus = "",
                TwoFactorAuth = false
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(UserModel model)
        {
            var srv = new UserServices();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid User Details";
                var ser = new UserServices();
               
                var modelOut = new UserModel
                {
                    DateJoined = DateTime.Now,
                    Email = "",
                    IsAdministrator = false,
                    IsSystemUser = false,
                    //Permissions = true,
                    Password = "",
                    UserName = "",
                    IsTradeInquiry = false,
                    StartDate = DateTime.Now,
                    ExpiryDate = new DateTime(2021, 1, 1),
                    FirstName = "",
                    LastName = "",
                    CompanyName = "",                    
                    Address1 = "",
                    Address2 = "",
                    Address3 = "",
                    Address4 = "",
                    PostalCode = "",
                    Telephone = "",
                    Mobile = "",
                    Costs = 0,
                    AccountStatus = "",
                    TwoFactorAuth = false

                };

                return View("Add", modelOut);
            }

            bool bResult = await srv.DoesEmailExist(model.Email);

            if (bResult)
            {

                ViewData["Message"] = "User Email already exists";
                return View("Add", model);

            }

            await srv.AddUser(model);

            return RedirectToAction("Index", "User");

        }
    }
}
