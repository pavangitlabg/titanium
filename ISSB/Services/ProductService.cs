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
    public class ProductService
    {

        public async Task<List<ProductAllFilterModel>> GetAllProducts()
        {
            var db = new DBContext();
            var cursor = await db.ProductAllFilterDB.FindAsync(new BsonDocument());

            IList<ProductAllFilterDB> results = cursor.ToList();
            var modelList = new List<ProductAllFilterModel>();

            foreach (var Item in results)
            {

                var model = new ProductAllFilterModel
                {
                    _id = Item._id.ToString(),
                    Idx = Item.Idx,
                    Name = Item.Name,
                    SortOrder = Item.SortOrder,
                    TariffCode = Item.TariffCode,
                    Cost = Item.Cost,

                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.Idx).ToList();
        }

        public async Task<List<ProductAllFilterModel>> GetAllProductsForTree()
        {
            var db = new DBContext();
            var cursor = await db.ProductAllFilterDB.FindAsync(new BsonDocument());

            IList<ProductAllFilterDB> results = cursor.ToList();
            var modelList = new List<ProductAllFilterModel>();

            foreach (var Item in results)
            {
                var LongName = Item.TariffCode + " - " + Item.Name;
                var model = new ProductAllFilterModel
                {
                    Idx = Item.Idx,
                    Name = LongName,
                    SortOrder = Item.SortOrder,
                    TariffCode = Item.TariffCode,
                    Cost = Item.Cost
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x.Idx).ToList();
        }

        //public async Task<ProductAllFilterModel> GetProductsById(int Id)
        //{
        //    var builder = Builders<ProductAllFilterDB>.Filter;
        //    var filter = builder.Eq("Idx", Id);

        //    var db = new DBContext();
        //    var cursor = await db.ProductAllFilterDB.FindAsync(filter);

        //    IList<ProductAllFilterDB> results = cursor.ToList();

        //    var Item = results[0];

        //    var model = new ProductAllFilterModel { Idx = Item.Idx, SortOrder = Item.SortOrder, TariffCode = Item.TariffCode, Name = Item.Name, Cost = Item.Cost };
        //    return model;
        //}

        public async Task<ProductAllFilterModel> GetProductsById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<ProductAllFilterDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.ProductAllFilterDB.FindAsync(filter);

            IList<ProductAllFilterDB> results = cursor.ToList();

            var Item = results[0];

            var model = new ProductAllFilterModel
            {
                _id = Item._id.ToString(),
                Cost = Item.Cost,
                Idx = Item.Idx,
                Name = Item.Name,
                SortOrder = Item.SortOrder,
                TariffCode = Item.TariffCode

            };

            return model;
        }

        public async Task<ProductAllFilterModel> GetProductsByTariffCode(string code)
        {
            var builder = Builders<ProductAllFilterDB>.Filter;
            var filter = builder.Eq("TariffCode", code);

            var db = new DBContext();
            var cursor = await db.ProductAllFilterDB.FindAsync(filter);

            IList<ProductAllFilterDB> results = cursor.ToList();

            var Item = results[0];

            var model = new ProductAllFilterModel { _id = Item._id.ToString(), Idx = Item.Idx, SortOrder = Item.SortOrder, TariffCode = Item.TariffCode, Name = Item.Name, Cost = Item.Cost };
            return model;
        }

        public async Task<List<ProductBroadFilterModel>> GetAllBroadProducts()
        {
            var db = new DBContext();
            var cursor = await db.ProductBroadFilterDB.FindAsync(new BsonDocument());

            IList<ProductBroadFilterDB> results = cursor.ToList();
            var modelList = new List<ProductBroadFilterModel>();

            foreach (var Item in results)
            {
                var iList = new List<ProductBroadFilterItemModel>();
                foreach (var It in Item.Items)
                {
                     var iModel = new ProductBroadFilterItemModel { Name = It.Name, Sort = It.Sort, TariffCode = It.TariffCode, Description = It.TariffCode + " - " + It.Name };
                   //  var iModel = new ProductBroadFilterItemModel { Name = It.Name, Sort = It.Sort, TariffCode = It.TariffCode, Description = It.Name };

                    iList.Add(iModel);
                }

                var model = new ProductBroadFilterModel
                {
                    Idx = Item.Idx,
                    Name = Item.Name,
                    SortOrder = Item.SortOrder,
                    Items = iList,

                };

                modelList.Add(model);
            }

            //var modelDeleted = new ProductBroadFilterModel
            //{
            //    Idx = modelList.Count + 1,
            //    Name = "Deleted",
            //    SortOrder = 99,
            //    Items = new List<ProductBroadFilterItemModel>()

            //};
            //modelList.Add(modelDeleted);

            return modelList.OrderBy(x => x.SortOrder).ToList();
        }

        public async Task<List<ProductBroadFilterItemModel>> GetBroadProducts()
        {
            var db = new DBContext();
            var cursor = await db.ProductAllFilterDB.FindAsync(new BsonDocument());

            IList<ProductAllFilterDB> results = cursor.ToList();
            var modelList = new List<ProductBroadFilterModel>();
            var iList = new List<ProductBroadFilterItemModel>();
            foreach (var Item in results)
            {
                var model = new ProductBroadFilterItemModel
                {
                    Description = Item.Name,
                    TariffCode = Item.TariffCode,
                    Name = Item.Name,
                    Sort = Item.SortOrder
                };

                iList.Add(model);

            }

            return iList.OrderBy(x => x.TariffCode).ToList();
        }

        public async Task<List<string>> GetAllSteel()
        {
            var db = new DBContext();
            var cursor = await db.ProductBroadFilterDB.FindAsync(new BsonDocument());

            IList<ProductBroadFilterDB> results = cursor.ToList();

            var Item = results[0];

            var iList = new List<string>();
            foreach (var It in Item.Items)
            {
                iList.Add(It.TariffCode);
            }
            
            return iList;
        }

        public async Task<List<ProductMedumFilterModel>> GetAllMedumProducts()
        {
            var db = new DBContext();
            var cursor = await db.ProductMedumFilterDB.FindAsync(new BsonDocument());

            IList<ProductMedumFilterDB> results = cursor.ToList();
            var modelList = new List<ProductMedumFilterModel>();

            foreach (var Item in results)
            {
                var iList = new List<ProductMedumFilterItemModel>();
                foreach (var It in Item.Items)
                {
                    var iModel = new ProductMedumFilterItemModel { Name = It.Name, Sort = It.Sort, TariffCode = It.TariffCode, Description = It.TariffCode + " - " + It.Name };
                    iList.Add(iModel);
                }

                var model = new ProductMedumFilterModel
                {
                    Idx = Item.Idx,
                    Name = Item.Name,
                    SortOrder = Item.SortOrder,
                    Code = Item.Code,
                    Description = "[" + Item.Code + "] " + Item.Name,
                    Items = iList

                };

                modelList.Add(model);
            }


            return modelList.OrderBy(x => x.Code).ToList();
        }

        public async Task<List<ProductMedumFilterModel>> GetAllMedumProductsForTree()
        {
            var db = new DBContext();
            var cursor = await db.ProductMedumFilterDB.FindAsync(new BsonDocument());

            IList<ProductMedumFilterDB> results = cursor.ToList();
            var modelList = new List<ProductMedumFilterModel>();

            foreach (var Item in results)
            {
                var iList = new List<ProductMedumFilterItemModel>();
                foreach (var It in Item.Items)
                {
                    var iModel = new ProductMedumFilterItemModel { Name = It.Name, Sort = It.Sort, TariffCode = It.TariffCode, Description = It.TariffCode + " - " + It.Name };
                    iList.Add(iModel);
                }

                var model = new ProductMedumFilterModel
                {
                    Idx = Item.Idx,
                    Description = Item.Name,
                    SortOrder = Item.SortOrder,
                    Code = Item.Code,
                    Name = "[" + Item.Code + "] " + Item.Name,
                    Items = iList

                };

                modelList.Add(model);
            }


            return modelList.OrderBy(x => x.Code).ToList();
        }

        public async Task<List<TariffShortModel>> GetLongTarrif()
        {
            var db = new DBContext();

            // var cursor =  db.TariffDB.Find(new BsonDocument()).Limit(50);
            var cursor = await db.TariffDB.FindAsync(new BsonDocument());

            IList<TariffDB> results = cursor.ToList();
            var modelList = new List<TariffShortModel>();

            foreach (var Item in results)
            {
                if (!string.IsNullOrEmpty(Item.SOURCE_COUNTRY_TARIFF_CODE))
                {
                    var model = new TariffShortModel
                    {
                        _id = Item._id.ToString(),
                        SOURCE_COUNTRY_TARIFF_CODE = Item.SOURCE_COUNTRY_TARIFF_CODE,
                        SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = Item.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND

                    };

                    modelList.Add(model);
                }
            }


            return modelList.OrderBy(x => x.SOURCE_COUNTRY_TARIFF_CODE).ToList();
        }

        public async Task<List<TariffShortModel>> GetLongTarrifByCode(string Code)
        {
            var db = new DBContext();

            var builder = Builders<TariffDB>.Filter;
            var filter = builder.Eq("TARIFF_CODE_TABLE_CODE", Code);
            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();
            var modelList = new List<TariffShortModel>();

            foreach (var Item in results)
            {
                if (!string.IsNullOrEmpty(Item.SOURCE_COUNTRY_TARIFF_CODE))
                {
                    var model = new TariffShortModel
                    {
                        _id = Item._id.ToString(),
                        SOURCE_COUNTRY_TARIFF_CODE = Item.SOURCE_COUNTRY_TARIFF_CODE,
                        SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = Item.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND,
                        WTO_CODE = Item.WTO_CODE

                    };

                    modelList.Add(model);
                }
            }


            return modelList.OrderBy(x => x.SOURCE_COUNTRY_TARIFF_CODE).ToList();
        }

        public async Task<List<TariffShortModel>> GetLongTarrifBy2DigitCode(string Code, string Code2)
        {
            var db = new DBContext();

            var builder = Builders<TariffDB>.Filter;
            var filter = builder.Eq("TARIFF_CODE_TABLE_CODE", Code) & builder.Eq("WTO_CODE", Code2);

            var cursor = await db.TariffDB.FindAsync(filter);

            IList<TariffDB> results = cursor.ToList();
            var modelList = new List<TariffShortModel>();

            foreach (var Item in results)
            {
                if (!string.IsNullOrEmpty(Item.SOURCE_COUNTRY_TARIFF_CODE))
                {
                    var model = new TariffShortModel
                    {
                        _id = Item._id.ToString(),
                        SOURCE_COUNTRY_TARIFF_CODE = Item.SOURCE_COUNTRY_TARIFF_CODE,
                        SOURCE_COUNTRY_TARIFF_SHORT_LEGEND = Item.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND,
                        WTO_CODE = Item.WTO_CODE

                    };

                    modelList.Add(model);
                }
            }


            return modelList.OrderBy(x => x.SOURCE_COUNTRY_TARIFF_CODE).ToList();
        }

        public async Task<ProductAllFilterDB> Save(ProductAllFilterModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<ProductAllFilterDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.ProductAllFilterDB.FindAsync(filter);
            IList<ProductAllFilterDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new ProductAllFilterDB
            {
                _id = Item._id,
                Idx = model.Idx,
                TariffCode = model.TariffCode,
                Name = model.Name,
                Cost = model.Cost,
                SortOrder = model.SortOrder
            };

            var returnModel = await db.ProductAllFilterDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;

        }

        public async Task<bool> SaveBroad(ProductBroadFilterModel model)
        {
            bool bReturn = false;
            var builder = Builders<ProductBroadFilterDB>.Filter;
            var filter = builder.Eq("Idx", model.Idx);
            var db = new DBContext();

            var cursor = await db.ProductBroadFilterDB.FindAsync(filter);
            IList<ProductBroadFilterDB> results = cursor.ToList();

            var Item = results[0];

            var iList = new List<ProductBroadFilterItemDB>();

            foreach (var modelItem in model.Items)
            {
                var iModel = new ProductBroadFilterItemDB { TariffCode = modelItem.TariffCode, Name = modelItem.Name, Sort = modelItem.Sort };
                iList.Add(iModel);
            }

            var modelDB = new ProductBroadFilterDB
            {
                _id = Item._id,
                Idx = model.Idx,
                Name = model.Name,
                SortOrder = model.SortOrder,
                Items = iList

            };

            await db.ProductBroadFilterDB.FindOneAndReplaceAsync(filter, modelDB);
            bReturn = true;


            return bReturn;
        }

        public async Task<ProductMedumFilterDB> SaveMedum(ProductMedumFilterModel model)
        {

            var builder = Builders<ProductMedumFilterDB>.Filter;
            var filter = builder.Eq("Idx", model.Idx);
            var db = new DBContext();

            var cursor = await db.ProductMedumFilterDB.FindAsync(filter);
            IList<ProductMedumFilterDB> results = cursor.ToList();

            var Item = results[0];

            var iList = new List<ProductMedumFilterItemDB>();

            foreach (var modelItem in model.Items)
            {
                var iModel = new ProductMedumFilterItemDB { TariffCode = modelItem.TariffCode, Name = modelItem.Name, Sort = modelItem.Sort };
                iList.Add(iModel);
            }

            var modelDB = new ProductMedumFilterDB
            {
                _id = Item._id,
                Idx = model.Idx,
                Name = model.Name,
                SortOrder = model.SortOrder,
                Code = model.Code,
                Items = iList

            };

            var returnModel = await db.ProductMedumFilterDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;
        }

        public async Task<ProductBroadFilterDB> Delete(ProductBroadFilterModel model)
        {

            var builder = Builders<ProductBroadFilterDB>.Filter;
            var filter = builder.Eq("Idx", model.Idx);
            var db = new DBContext();

            var cursor = await db.ProductBroadFilterDB.FindAsync(filter);
            IList<ProductBroadFilterDB> results = cursor.ToList();

            var Item = results[0];

            var pList = new List<ProductBroadFilterItemDB>();
            foreach (var mItem in model.Items)
            {
                var pmodel = new ProductBroadFilterItemDB { TariffCode = mItem.TariffCode, Name = mItem.Name, Sort = mItem.Sort };
                pList.Add(pmodel);
            }


            var modelDB = new ProductBroadFilterDB
            {
                _id = Item._id,
                Idx = model.Idx,
                Name = model.Name,
                SortOrder = model.SortOrder,
                Items = pList
            };
            var upsert = new UpdateOptions()
            {
                IsUpsert = false
            };

            var mod = await db.ProductBroadFilterDB.FindOneAndReplaceAsync(filter, modelDB);

            return mod;
        }

        public async Task<ProductMedumFilterDB> DeleteMedum(ProductMedumFilterModel model)
        {

            var builder = Builders<ProductMedumFilterDB>.Filter;
            var filter = builder.Eq("Idx", model.Idx);
            var db = new DBContext();

            var cursor = await db.ProductMedumFilterDB.FindAsync(filter);
            IList<ProductMedumFilterDB> results = cursor.ToList();

            var Item = results[0];

            var pList = new List<ProductMedumFilterItemDB>();
            foreach (var mItem in model.Items)
            {
                var pmodel = new ProductMedumFilterItemDB { TariffCode = mItem.TariffCode, Name = mItem.Name, Sort = mItem.Sort };
                pList.Add(pmodel);
            }


            var modelDB = new ProductMedumFilterDB
            {
                _id = Item._id,
                Idx = model.Idx,
                Name = model.Name,
                SortOrder = model.SortOrder,
                Code = model.Code,
                Items = pList
            };
            var upsert = new UpdateOptions()
            {
                IsUpsert = false
            };

            var mod = await db.ProductMedumFilterDB.FindOneAndReplaceAsync(filter, modelDB);

            return mod;
        }

        public async Task<bool> ResyncHS(List<ProductAllFilterModel> modelList)
        {
            var builder = Builders<ProductAllFilterDB>.Filter;

            var filter = builder.Eq("Cost", 0);
            var db = new DBContext();

            await db.ProductAllFilterDB.DeleteManyAsync(new BsonDocument());

            var updateList = new List<ProductAllFilterDB>();

            foreach(var Item in modelList)
            {
                var model = new ProductAllFilterDB
                {
                    Cost = Item.Cost,
                    Idx = Item.Idx,
                    Name = Item.Name,
                    SortOrder = Item.SortOrder,
                    TariffCode = Item.TariffCode

                };
                updateList.Add(model);

            }
            await db.ProductAllFilterDB.InsertManyAsync(updateList);

            return true;
        }
    }
}
