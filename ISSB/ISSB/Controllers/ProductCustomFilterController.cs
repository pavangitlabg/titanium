using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;

namespace ISSB.Controllers
{
    public class ProductCustomFilterController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new ProductCustomFilterService();
            var model = await srv.GetCustomProducts();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public IActionResult Add()
        {

            var modelOut = new ProductCustomFilterModel
            {
                Name = string.Empty,
                SortOrder = 0,

            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> AddNew(ProductCustomFilterModel model)
        {
            var srv = new ProductCustomFilterService();

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Filter Details";
               
                var modelOut = new ProductCustomFilterModel
                {
                    Name = string.Empty,
                    SortOrder = 0,
                    Items = new List<ProductCustomFilterItemModel>()
                };

                return View("Add", modelOut);
            }
            model.Items = new List<ProductCustomFilterItemModel>();
            await srv.Add(model);

            return RedirectToAction("Index", "ProductCustomFilter");
        }

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            //var tariffSrv = new TariffService();
            //var tmodel = await tariffSrv.GetTariffsByRegion("028");

            var tmodel = new List<TariffModel>();

            IQueryable data = tmodel.AsQueryable();
            ViewBag.TCList = JsonConvert.SerializeObject(tmodel);

            var srv = new ProductCustomFilterService();

            var model = await srv.GetByID(_id);

            ViewBag.FilterTCList = JsonConvert.SerializeObject(model.Items);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Save(ProductCustomFilterModel model)
        {
            var srv = new ProductCustomFilterService();
            var SaveModel = await srv.GetByID(model._id);

            model.Items = SaveModel.Items;
            SaveModel.SortOrder = model.SortOrder;
            SaveModel.Name = model.Name;
            SaveModel._id = model._id;
            SaveModel.SearchType = model.SearchType;
            await srv.Save(SaveModel);

            return RedirectToAction("Index", "ProductCustomFilter");
        }

        [HttpPost]
        [Authorize]
        public async Task<string> UpdateTariffCodes(string RegionCode)
        {

            var tariffSrv = new TariffService();
            var tmodel = await tariffSrv.GetTariffsByRegion(RegionCode);

            foreach(var Item in tmodel)
            {
                if(string.IsNullOrEmpty(Item.SOURCE_COUNTRY_TARIFF_CODE))
                {
                   // Item.SOURCE_COUNTRY_TARIFF_CODE = Item.HARMONISED_TARIFF_CODE.PadRight(11, '0');
                    Item.SOURCE_COUNTRY_TARIFF_CODE = Item.HARMONISED_TARIFF_CODE;
                    Item.HARMONISED_TARIFF_LONG_LEGEND = Item.HARMONISED_TARIFF_LONG_LEGEND;
                }
                else
                {
                    if(Item.TARIFF_CODE_TABLE_CODE.Equals("HS"))
                        Item.SOURCE_COUNTRY_TARIFF_CODE = Item.HARMONISED_TARIFF_CODE;

                    Item.HARMONISED_TARIFF_LONG_LEGEND = Item.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND;
                }
            }


            return JsonConvert.SerializeObject(tmodel);
            
        }

        [HttpPost]
        [Authorize]
        public async Task<string> PostTariffCodes(string RecordId, string SearchType, string TariffCodes)
        {

            var tariffList = TariffCodes.Split('|').ToList();
            var srv = new ProductCustomFilterService();
            var tariffsrv = new TariffService();

            var model = await srv.GetByID(RecordId);
            model.SearchType = SearchType;

            model.Items = new List<ProductCustomFilterItemModel>();

            int cnt = 1;
            foreach (var Item in tariffList)
            {
                if (Item.Length == 6)
                {
                    var tModel = await tariffsrv.GetHSTariffByCode(Item);
                    var lModel = new ProductCustomFilterItemModel
                    {
                        TariffCode = Item,
                        Name = tModel.HARMONISED_TARIFF_LONG_LEGEND,
                        Sort = cnt
                    };
                    model.Items.Add(lModel);

                    cnt++;
                }
                else
                {
                    var tModel = await tariffsrv.GetTariffByCode(Item);
                    var lModel = new ProductCustomFilterItemModel
                    {
                        TariffCode = Item,
                        Name = tModel.HARMONISED_TARIFF_LONG_LEGEND,
                        Sort = cnt
                        
                    };
                    model.Items.Add(lModel);

                    cnt++;
                }
            }

            await srv.Save(model);

            return JsonConvert.SerializeObject(model.Items);
          
        }

        [HttpPost]
        [Authorize]
        public async Task<string> RemoveTariffCodes(string RecordId, string TariffCodes)
        {
            var tariffList = TariffCodes.Split('|').ToList();
            var srv = new ProductCustomFilterService();
            var tariffsrv = new TariffService();
            var model = await srv.GetByID(RecordId);

            foreach (var Item in tariffList)
            {
                model.Items.RemoveAll((x) => x.TariffCode.Equals(Item));
            }
            await srv.Save(model);
            return JsonConvert.SerializeObject(model.Items);
        }

        [Authorize]
        public async Task<ActionResult> Delete(string _id)
        {
            var srv = new ProductCustomFilterService();

            var model = await srv.GetByID(_id);

            await srv.Delete(model);

            return RedirectToAction("Index", "ProductCustomFilter");
        }
    }
}
