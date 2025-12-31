using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class LookupsService
    {
        public async Task<List<TaxCodesModel>> GetTaxCodes()
        {
            var db = new DBContext();
            var cursor = await db.TaxCodesDB.FindAsync(new BsonDocument());

            IList<TaxCodesDB> results = cursor.ToList();
            var modelList = new List<TaxCodesModel>();

            foreach (var Item in results)
            {
                var model = new TaxCodesModel
                {
                    TaxCode = Item.TaxCode,
                    Description = Item.Description,
                    VatRate = Item.VatRate
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<IEnumerable<SelectListItem>> GetTaxCodesDropdown()
        {
            var db = new DBContext();
            var cursor = await db.TaxCodesDB.FindAsync(new BsonDocument());

            IList<TaxCodesDB> results = cursor.ToList();
            var modelList = new List<TaxCodesDB>();

            List<SelectListItem> taxitems = new List<SelectListItem>();


            foreach (var Item in results)
            {
                var modelLocation = new SelectListItem
                {
                    Value = Item.TaxCode,
                    Text = Item.Description
                };
                taxitems.Add(modelLocation);
            }

            return taxitems;
        }

        public async Task<bool> InsertTaxCode(TaxCodesModel model)
        {
            bool bReturn = false;
         
            var db = new DBContext();

            var modelDB = new TaxCodesDB
            {
                TaxCode = model.TaxCode,
                Description = model.Description,
                VatRate = model.VatRate
            };

            await db.TaxCodesDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<bool> InsertYear(YearsModel model)
        {
            bool bReturn = false;

            var db = new DBContext();
            var mList = new List<MonthDB>();

            foreach(var m in model.Months)
            {
                var mth = new MonthDB { MounthNumber = m.MounthNumber, Month = m.Month };
                mList.Add(mth);
            }

            var modelDB = new YearsDB
            {
                Idx = model.Idx,
                Year = model.Year,
                Months = mList
            };

            await db.YearsDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;
        }

        public async Task<List<YearsModel>> GetYears()
        {
            var db = new DBContext();
            var cursor = await db.YearsDB.FindAsync(new BsonDocument());

            IList<YearsDB> results = cursor.ToList();
            var modelList = new List<YearsModel>();

            foreach (var Item in results)
            {
                var mList = new List<MonthModel>();

                foreach(var m in Item.Months)
                {
                    var mth = new MonthModel { MounthNumber = m.MounthNumber, Month = m.Month };
                    mList.Add(mth);
                }

                var model = new YearsModel
                {
                     Idx = Item.Idx,
                     Year = Item.Year,
                     Months = mList
                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x.Idx).ToList(); 
        }

        public async Task<IEnumerable<SelectListItem>> GetYearsDropdown()
        {
            var db = new DBContext();
            var cursor = await db.YearsDB.FindAsync(new BsonDocument());

            IList<YearsDB> results = cursor.ToList();
            //var modelList = new List<YearsDB>();

            List<SelectListItem> yearItems = new List<SelectListItem>();


            foreach (var Item in results)
            {
                var modelLocation = new SelectListItem
                {
                    Value = Item.Idx.ToString(),
                    Text = Item.Year.ToString()
                };
                yearItems.Add(modelLocation);
            }

            return yearItems.OrderByDescending(x => x.Text).ToList(); 
        }
    }
}
