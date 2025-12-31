using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{

    public class ResultLine
    {
        public int MONTH { get; set; }
        public string IMPORT_TARIFF { get; set; }
        public string GEO { get; set; }
        public double TONNES { get; set; }
        public double VPT { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class JSUKPlateDataController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<ActionResult<UKPLateJsonModel>> GetUKPlate(string UserEmail,string UserPassword,int year)
        {
            var userModel = new UserModel { Email = UserEmail , Password = UserPassword };
            var authSrv = new UserServices();
            var UserModel = await authSrv.LoginUser(userModel.Email, userModel.Password);
            if (!UserModel.IsLoggedIn)
            {
                var rModel = new UKPLateJsonModel { };
                return rModel;
            }

            var srv = new GeneralReporstService();
            var data = await srv.GetUKPlateReportData(year);

            var DataList = new List<TradeDataModel>();
            var DataListResult = new List<TradeDataModel>();

            foreach (var d in data.TradeData)
            {
                DataList.Add(d);
            }


            var result = DataList
                .OrderBy(x => x.IMPORT_TARIFF).ThenBy(s => s.SC_GEO).ThenBy(s => s.MONTH)
                .GroupBy(x => new { x.YEAR, x.MONTH, x.IMPORT_TARIFF, x.SC_GEO })
               
                .Select(cl => new ResultLine
                {
                    MONTH = cl.FirstOrDefault().MONTH,
                    IMPORT_TARIFF = cl.FirstOrDefault().IMPORT_TARIFF,
                    GEO = cl.FirstOrDefault().SC_GEO,
                    TONNES = cl.Sum(c => c.WEIGHT / 10000),
                    VPT = cl.Sum(c => c.MONETARY_VALUE / 10000)
                }).ToList();


            var sourceCountrySrv = new SourceCountryService();
            var tariffCodeSrv = new TariffService();
            var tariffList = await tariffCodeSrv.GetTariffs();
            var tList = tariffList.OrderByDescending(x => x.START_DATE);
            var SourceCountryList = await sourceCountrySrv.GetSourceCountries();
            var ItemList = new List<UKPlateModel>();

            foreach(var Item in result)
            {

                var cName = SourceCountryList.FirstOrDefault(x => x.GEO_CODE.Equals(Item.GEO));
                var tName = tList.FirstOrDefault(x => x.SOURCE_COUNTRY_TARIFF_CODE.Equals(Item.IMPORT_TARIFF));
                var model = new UKPlateModel {  Code = Item.IMPORT_TARIFF,  GEO = Item.GEO, Country = cName.NAME, ProductName = tName.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND };
                var searchItem = ItemList.Find(r => r.Code == Item.IMPORT_TARIFF && r.GEO == Item.GEO);
                

                if (searchItem == null)
                {
                    if(Item.MONTH == 1)
                    {
                        model.Tonnes1 = Item.TONNES;
                        model.Weight1 = Item.VPT;
                    }

                    if (Item.MONTH == 2)
                    {
                        model.Tonnes2 = Item.TONNES;
                        model.Weight2 = Item.VPT;
                    }
                    if (Item.MONTH == 3)
                    {
                        model.Tonnes3 = Item.TONNES;
                        model.Weight3 = Item.VPT;
                    }
                    if (Item.MONTH == 4)
                    {
                        model.Tonnes4 = Item.TONNES;
                        model.Weight4 = Item.VPT;
                    }
                    if (Item.MONTH == 5)
                    {
                        model.Tonnes5 = Item.TONNES;
                        model.Weight5 = Item.VPT;
                    }
                    if (Item.MONTH == 6)
                    {
                        model.Tonnes6 = Item.TONNES;
                        model.Weight6 = Item.VPT;
                    }
                    if (Item.MONTH == 7)
                    {
                        model.Tonnes7 = Item.TONNES;
                        model.Weight7 = Item.VPT;
                    }
                    if (Item.MONTH == 8)
                    {
                        model.Tonnes8 = Item.TONNES;
                        model.Weight8 = Item.VPT;
                    }
                    if (Item.MONTH == 9)
                    {
                        model.Tonnes9 = Item.TONNES;
                        model.Weight9 = Item.VPT;
                    }
                    if (Item.MONTH == 10)
                    {
                        model.Tonnes10 = Item.TONNES;
                        model.Weight10 = Item.VPT;
                    }
                    if (Item.MONTH == 11)
                    {
                        model.Tonnes11 = Item.TONNES;
                        model.Weight11 = Item.VPT;
                    }
                    if (Item.MONTH == 12)
                    {
                        model.Tonnes12 = Item.TONNES;
                        model.Weight12 = Item.VPT;
                    }

                    model.TonnesYTD = model.Tonnes1 + model.Tonnes2 + model.Tonnes3 + model.Tonnes4 + model.Tonnes5
                       + model.Tonnes6 + model.Tonnes7 + model.Tonnes8 + model.Tonnes9 + model.Tonnes10 + model.Tonnes11
                       + model.Tonnes12;

                    model.WeightYTD = model.Weight1 + model.Weight2 + model.Weight3 + model.Weight4 + model.Weight5
                      + model.Weight6 + model.Weight7 + model.Weight8 + model.Weight9 + model.Weight10 + model.Weight11
                      + model.Weight12;

                    ItemList.Add(model);
                }
                else
                {
                    if (Item.MONTH == 1)
                    {
                        searchItem.Tonnes1 = Item.TONNES;
                        searchItem.Weight1 = Item.VPT;
                    }

                    if (Item.MONTH == 2)
                    {
                        searchItem.Tonnes2 = Item.TONNES;
                        searchItem.Weight2 = Item.VPT;
                    }
                    if (Item.MONTH == 3)
                    {
                        searchItem.Tonnes3 = Item.TONNES;
                        searchItem.Weight3 = Item.VPT;
                    }
                    if (Item.MONTH == 4)
                    {
                        searchItem.Tonnes4 = Item.TONNES;
                        searchItem.Weight4 = Item.VPT;
                    }
                    if (Item.MONTH == 5)
                    {
                        searchItem.Tonnes5 = Item.TONNES;
                        searchItem.Weight5 = Item.VPT;
                    }
                    if (Item.MONTH == 6)
                    {
                        searchItem.Tonnes6 = Item.TONNES;
                        searchItem.Weight6 = Item.VPT;
                    }
                    if (Item.MONTH == 7)
                    {
                        searchItem.Tonnes7 = Item.TONNES;
                        searchItem.Weight7 = Item.VPT;
                    }
                    if (Item.MONTH == 8)
                    {
                        searchItem.Tonnes8 = Item.TONNES;
                        searchItem.Weight8 = Item.VPT;
                    }
                    if (Item.MONTH == 9)
                    {
                        searchItem.Tonnes9 = Item.TONNES;
                        searchItem.Weight9 = Item.VPT;
                    }
                    if (Item.MONTH == 10)
                    {
                        searchItem.Tonnes10 = Item.TONNES;
                        searchItem.Weight10 = Item.VPT;
                    }
                    if (Item.MONTH == 11)
                    {
                        searchItem.Tonnes11 = Item.TONNES;
                        searchItem.Weight11 = Item.VPT;
                    }
                    if (Item.MONTH == 12)
                    {
                        searchItem.Tonnes12 = Item.TONNES;
                        searchItem.Weight12 = Item.VPT;
                    }

                    searchItem.TonnesYTD = searchItem.Tonnes1 + searchItem.Tonnes2 + searchItem.Tonnes3 + searchItem.Tonnes4 + searchItem.Tonnes5
                        + searchItem.Tonnes6 + searchItem.Tonnes7 + searchItem.Tonnes8 + searchItem.Tonnes9 + searchItem.Tonnes10 + searchItem.Tonnes11
                        + searchItem.Tonnes12;

                    searchItem.WeightYTD = searchItem.Weight1 + searchItem.Weight2 + searchItem.Weight3 + searchItem.Weight4 + searchItem.Weight5
                    + searchItem.Weight6 + searchItem.Weight7 + searchItem.Weight8 + searchItem.Weight9 + searchItem.Weight10 + searchItem.Weight11
                    + searchItem.Weight12;

                }
               
            }

            


            var dataList = new UKPLateJsonModel { data = ItemList };


            return dataList;
        }
    }
}
