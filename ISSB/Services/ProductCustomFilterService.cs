using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class ProductCustomFilterService
    {

        public async Task<List<ProductCustomFilterModel>> GetCustomProductsbyReport(string _id)
        {
            var db = DBContext.Instance;

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<SavedQueryDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var savedQueryCursor = await db.SavedQueryDB.FindAsync(filter);
            IList<SavedQueryDB> resultssavedQueryResults = savedQueryCursor.ToList();

            if(resultssavedQueryResults.Count == 0)
            {
                return new List<ProductCustomFilterModel>();
            }

            var QRecord = resultssavedQueryResults[0];
            var modelList = new List<ProductCustomFilterModel>();

            foreach (var FilterItem in QRecord.Products)
            {
                BsonObjectId RecordFId = new BsonObjectId(new ObjectId(FilterItem.Product));
                var qbuilder = Builders<ProductCustomFilterDB>.Filter;
                var qfilter = qbuilder.Eq("_id", RecordFId);


                var cursor = await db.ProductCustomFilterDB.FindAsync(qfilter);
                IList<ProductCustomFilterDB> results = cursor.ToList();
               

                foreach (var Item in results)
                {
                    var ItemList = new List<ProductCustomFilterItemModel>();
                    foreach (var Itm in Item.Items)
                    {
                        var modelItm = new ProductCustomFilterItemModel { TariffCode = Itm.TariffCode, Name = Itm.Name, Sort = Itm.Sort };
                        ItemList.Add(modelItm);
                    }

                    var model = new ProductCustomFilterModel
                    {
                        _id = Item._id.ToString(),
                        Name = Item.Name,
                        SortOrder = Item.SortOrder,
                        Items = ItemList

                    };

                    modelList.Add(model);
                }
            }

            return modelList.OrderBy(x => x._id).ToList();
        }




        public async Task<List<ProductCustomFilterModel>> GetCustomProducts()
        {
            var db = DBContext.Instance;
            var cursor = await db.ProductCustomFilterDB.FindAsync(new BsonDocument());

            IList<ProductCustomFilterDB> results = cursor.ToList();
            var modelList = new List<ProductCustomFilterModel>();

            foreach (var Item in results)
            {
                var ItemList = new List<ProductCustomFilterItemModel>();
                foreach (var Itm in Item.Items)
                {
                    var modelItm = new ProductCustomFilterItemModel { TariffCode = Itm.TariffCode, Name = Itm.Name, Sort = Itm.Sort };
                    ItemList.Add(modelItm);
                }

                var model = new ProductCustomFilterModel
                {
                    _id = Item._id.ToString(),
                    Name = Item.Name,
                    SortOrder = Item.SortOrder,
                    Items = ItemList

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<bool> Add(ProductCustomFilterModel model)
        {
            var db = DBContext.Instance;
            var ItemList = new List<ProductCustomFilterItemDB>();

            foreach (var Item in model.Items)
            {
                var modelItm = new ProductCustomFilterItemDB { TariffCode = Item.TariffCode, Name = Item.Name, Sort = Item.Sort };
                ItemList.Add(modelItm);
            }

            var modelDB = new ProductCustomFilterDB
            {
                  SortOrder = model.SortOrder,
                  Name = model.Name,
                  Items = ItemList

            };

            await db.ProductCustomFilterDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<ProductCustomFilterModel> GetByID(string _id)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<ProductCustomFilterDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = DBContext.Instance;
            var cursor = await db.ProductCustomFilterDB.FindAsync(filter);

            IList<ProductCustomFilterDB> results = cursor.ToList();

            var Item = results[0];

            var ItemList = new List<ProductCustomFilterItemModel>();

            foreach (var Itm in Item.Items)
            {
                var modelItm = new ProductCustomFilterItemModel { TariffCode = Itm.TariffCode, Name = Itm.Name, Sort = Itm.Sort };
                ItemList.Add(modelItm);
            }

            var model = new ProductCustomFilterModel
            {
                _id = Item._id.ToString(),
                Name = Item.Name,
                SortOrder = Item.SortOrder,
                SearchType = Item.SearchType,
                Items = ItemList
            };

            return model;
        }

        public async Task<ProductCustomFilterDB> Save(ProductCustomFilterModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<ProductCustomFilterDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = DBContext.Instance;

            var cursor = await db.ProductCustomFilterDB.FindAsync(filter);
            IList<ProductCustomFilterDB> results = cursor.ToList();

            var Item = results[0];

            var tList = new List<ProductCustomFilterItemDB>();

            foreach(var tItem in model.Items)
            {
                var tModel = new ProductCustomFilterItemDB
                {
                    TariffCode = tItem.TariffCode,
                    Name = tItem.Name,

                    Sort = tItem.Sort
                };
                tList.Add(tModel);
            }

            var modelDB = new ProductCustomFilterDB
            {
                _id = Item._id,
                SortOrder = model.SortOrder,
                Name = model.Name,
                SearchType = model.SearchType,
                Items = tList
            };

            var returnModel = await db.ProductCustomFilterDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<bool> Delete(ProductCustomFilterModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = DBContext.Instance;

            var builder = Builders<ProductCustomFilterDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.ProductCustomFilterDB.DeleteOneAsync(filter);

            return true;
        }
    }
}
