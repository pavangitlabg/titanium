using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;
using System.Security.Claims;

namespace ISSB.Controllers
{
    public class AddYearsController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var srv = new UserServices();
            var model = await srv.GetUsers();
            IQueryable Data = model.AsQueryable();
            return View(Data);
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var srv = new UserServices();
            var model = await srv.GetUsers();

            foreach(var Item in model)
            {
                if(string.IsNullOrEmpty(Item.AllowedYears))
                {
                    Item.AllowedYears = string.Empty;
                }

                if (Item.AllowedYears.Contains("2020"))
                {
                    //if (Item.Email.Equals("charles.jardine@nathan-software.com"))
                    //{
                        Item.AllowedYears = "2021," + Item.AllowedYears;
                        var editModel = await srv.GetUsersById(Item._id);
                        editModel.AllowedYears = Item.AllowedYears;
                        editModel.Password = Item.Password;

                        await srv.UpdateUser(editModel);
                   // }
                }
            }


            IQueryable Data = model.AsQueryable();
            return View(Data);
        }
    }
}
