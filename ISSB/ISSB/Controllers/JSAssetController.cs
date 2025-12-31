using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSAssetController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult<AssetJsonModel>> GetData(string UserEmail, string UserPassword)
        {
            var userModel = new UserModel { Email = UserEmail, Password = UserPassword };
            var authSrv = new UserServices();
            var UserModel = await authSrv.LoginUser(userModel.Email, userModel.Password);
            if (!UserModel.IsLoggedIn)
            {
                var rModel = new AssetJsonModel { };
                return rModel;
            }


            var srv = new AssetService();
            var data = await srv.GetAssets();

            var dList = new List<AssetModel>();
            foreach(var Item in data)
            {
                var model = new AssetModel
                {
                    CHECKED_BY = Item.CHECKED_BY,
                    DATE_CHECKED = Item.DATE_CHECKED,
                    DESCRIPTION = Item.DESCRIPTION,
                    ID = Item.ID,
                    LAST_MODIFIED = Item.LAST_MODIFIED,
                    LOCATION = Item.LOCATION,
                    MANUFACTURER = Item.MANUFACTURER,
                    MEMO = Item.MEMO,
                    MODEL_TYPE = Item.MODEL_TYPE,
                    PASSWORD = Item.PASSWORD,
                    PURCHASE_DATE = Item.PURCHASE_DATE,
                    SERIAL_NO = Item.SERIAL_NO,
                    USERNAME = Item.USERNAME,
                    VALUE = Item.VALUE,
                    _id = Item._id

                };
                dList.Add(model);
            }

            var rData = new AssetJsonModel { data = dList };

            return rData;
        }
    }
}
