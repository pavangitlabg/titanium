using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Enums;
using Data.Models;
using ISSB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{
    public class ProductFilterController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new ProductService();
            var model = await srv.GetAllProducts();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> UpdateHS()
        {
            var prodSrv = new ProductService();
            var tariffSrv = new TariffService();
            var hsTariffList = await tariffSrv.GetTariffsByRegion("HS");

            var prodFilterModel = new List<ProductAllFilterModel>();
            int cnt = 1;
            foreach (var Item in hsTariffList)
            {
                var model = new ProductAllFilterModel
                {
                    Idx = cnt,
                    SortOrder = cnt,
                    TariffCode = Item.HARMONISED_TARIFF_CODE,
                    Cost = 0,
                    Name = Item.HARMONISED_TARIFF_SHORT_LEGEND
                };
                prodFilterModel.Add(model);
                cnt++;
            }

            await prodSrv.ResyncHS(prodFilterModel);

            return RedirectToAction("Index", "ProductFilter");

        }

        [Authorize]
        public async Task<IActionResult> MedumFilter()
        {
            var srv = new ProductService();
            var treeModel = await srv.GetAllMedumProducts();
            var model = await srv.GetAllProducts();

            IQueryable treeData = treeModel.AsQueryable();
            ViewBag.TreeData = treeData;

            IQueryable data = model.AsQueryable();

            return View(data);
           
        }

        [Authorize]
        public async Task<IActionResult> BroadFilter()
        {
            var srv = new ProductService();
            var treeModel = await srv.GetAllBroadProducts();
            var model = await srv.GetAllProducts();

            IQueryable treeData = treeModel.AsQueryable();
            ViewBag.TreeData = treeData;

            IQueryable data = model.AsQueryable();

            return View(data);
        }
    

        [Authorize]
        public async Task<IActionResult> Edit(string _id)
        {
            var srv = new ProductService();
            var modelOut = await srv.GetProductsById(_id);

            return View(modelOut);

        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Delete(string nodePath)
        {

            string[] idx = nodePath.Split('.');

            int mIndex = int.Parse(idx[0]) - 1;
            int iIndex = int.Parse(idx[1]);

            var srv = new ProductService();
            var treeModel = await srv.GetAllBroadProducts();

            var postModel = treeModel[mIndex];
            var rItem = postModel.Items[iIndex];
            postModel.Items.Remove(rItem);

            var delModel = await srv.Delete(postModel);
           
            treeModel = await srv.GetAllBroadProducts();
            IQueryable treeData = treeModel.AsQueryable();
            ViewBag.TreeData = treeData;

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ProductAllFilterModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Product Filter Deleted, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = delModel
            };
            new EventLog(EventModel);

            return Json(new { success = true });
           // return RedirectToAction("BroadFilter", "ProductFilter");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> DeleteMedum(string nodePath)
        {

            string[] idx = nodePath.Split('.');

            int mIndex = int.Parse(idx[0]) - 1;
            int iIndex = int.Parse(idx[1]);

            var srv = new ProductService();
            var treeModel = await srv.GetAllMedumProducts();

            var postModel = treeModel[mIndex];
            var rItem = postModel.Items[iIndex];
            postModel.Items.Remove(rItem);

            var delModel = await srv.DeleteMedum(postModel);

            treeModel = await srv.GetAllMedumProducts();
            IQueryable treeData = treeModel.AsQueryable();
            ViewBag.TreeData = treeData;
            return Json(new { success = true });
           
        }

        [Authorize]
        public async Task<IActionResult> SaveNode(string nodeText,string nodePath)
        {

            string[] words = nodeText.Split('-');

            string key = words[0].TrimEnd(); 

            var srv = new ProductService();
            var treeModel = await srv.GetAllBroadProducts();
            var itemModel = await srv.GetProductsByTariffCode(key);
            var model = await srv.GetAllProducts();

            var updateModel = treeModel[int.Parse(nodePath)-1];
            var itemListModel = new ProductBroadFilterItemModel {  TariffCode = itemModel.TariffCode, Name = itemModel.Name , Description = string.Empty, Sort = 0 };
            updateModel.Items.Add(itemListModel);
            await srv.SaveBroad(updateModel);

            treeModel = await srv.GetAllBroadProducts();

            IQueryable treeData = treeModel.AsQueryable();
            ViewBag.TreeData = treeData;

            // IQueryable data = model.AsQueryable();
            return Json(new { success = true });
            //return RedirectToAction("BroadFilter", treeModel);

        }

        [Authorize]
        public async Task<IActionResult> SaveMedumNode(string nodeText, string nodePath)
        {

            string[] words = nodeText.Split('-');

            string key = words[0].TrimEnd();

            var srv = new ProductService();
            var treeModel = await srv.GetAllMedumProducts();
            var itemModel = await srv.GetProductsByTariffCode(key);
            var model = await srv.GetAllProducts();

            var updateModel = treeModel[int.Parse(nodePath) - 1];
            var itemListModel = new ProductMedumFilterItemModel { TariffCode = itemModel.TariffCode, Name = itemModel.Name, Description = string.Empty, Sort = 0 };
            updateModel.Items.Add(itemListModel);
            await srv.SaveMedum(updateModel);

            treeModel = await srv.GetAllMedumProducts();

            IQueryable treeData = treeModel.AsQueryable();
            ViewBag.TreeData = treeData;

            // IQueryable data = model.AsQueryable();
            return Json(new { success = true });
            //return RedirectToAction("BroadFilter", treeModel);

        }

        [Authorize]
        public async Task<IActionResult> Save(ProductAllFilterModel model)
        {
            var srv = new ProductService();
           
            await srv.Save(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.ProductAllFilterModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Product Filter Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "ProductFilter");

        }
    }
}
