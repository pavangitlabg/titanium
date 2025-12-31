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
    public class SourceCountryFilterService
    {
        public async Task<List<SourceCountryFilterModel>> GetFilters()
        {
            var db = new DBContext();
            var cursor = await db.SourceCountryFilterDB.FindAsync(new BsonDocument());

            IList<SourceCountryFilterDB> results = cursor.ToList();
            var modelList = new List<SourceCountryFilterModel>();

            foreach (var Item in results)
            {

                var Items = new List<SourceCountryFilterListModel>();

                foreach (var i in Item.Items)
                {
                    var lModel = new SourceCountryFilterListModel
                    {
                        SortOrder = i.SortOrder,
                        GeoCode = i.GeoCode,
                        CountryName = i.CountryName

                    };
                    Items.Add(lModel);
                }

                var model = new SourceCountryFilterModel
                {
                    _id = Item._id.ToString(),
                    Active = Item.Active,
                    CanEdit = Item.CanEdit,
                    Notes = Item.Notes,
                    Name = Item.Name,
                    SortOrder = Item.SortOrder
                };
                model.Items = Items;
                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<SourceCountryFilterModel> GetFilterByName(string name)
        {

            var builder = Builders<SourceCountryFilterDB>.Filter;
            var filter = builder.Eq("Name", name);

            var db = new DBContext();
            var cursor = await db.SourceCountryFilterDB.FindAsync(filter);
            IList<SourceCountryFilterDB> results = cursor.ToList();

            var Item = results[0];

            var model = new SourceCountryFilterModel
            {
                _id = Item._id.ToString(),
                Active = Item.Active,
                CanEdit = Item.CanEdit,
                Notes = Item.Notes,
                Name = Item.Name,
                SortOrder = Item.SortOrder
            };

            var Items = new List<SourceCountryFilterListModel>();

            foreach (var m in Item.Items)
            {
                var lModel = new SourceCountryFilterListModel
                {
                    SortOrder = m.SortOrder,
                    GeoCode = m.GeoCode,
                    CountryName = m.CountryName
                    
                };
                Items.Add(lModel);
               
            }
            model.Items = Items;
            return model;
        }

        public async Task AddFilter(SourceCountryFilterModel model)
        {
            var db = new DBContext();

            var newModel = new SourceCountryFilterDB
            {

                Active = model.Active,
                CanEdit = model.CanEdit,
                Name = model.Name,
                Notes = model.Notes,
                SortOrder = 1

            };

            var Items = new List<SourceCountryFilterListDB>();

            foreach (var Item in model.Items)
            {
                var lModel = new SourceCountryFilterListDB { GeoCode = Item.GeoCode, CountryName = Item.CountryName, SortOrder = Item.SortOrder };
                Items.Add(lModel);
            }
            newModel.Items = Items;

            await db.SourceCountryFilterDB.InsertOneAsync(newModel);

        }

        public async Task SaveFilter(SourceCountryFilterModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var newModel = new SourceCountryFilterDB
            {
                _id = RecordId,
                Active = model.Active,
                CanEdit = model.CanEdit,
                Name = model.Name,
                Notes = model.Notes,
                SortOrder = 1

            };

            var Items = new List<SourceCountryFilterListDB>();

            foreach(var Item in model.Items)
            {
                var lModel = new SourceCountryFilterListDB { GeoCode = Item.GeoCode, CountryName = Item.CountryName, SortOrder = Item.SortOrder };
                Items.Add(lModel);
            }
            newModel.Items = Items;

            var builder = Builders<SourceCountryFilterDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            await db.SourceCountryFilterDB.FindOneAndReplaceAsync(filter, newModel);

        }

        public async Task<bool> Delete(string objId)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(objId));
            bool bReturn = false;
            var builder = Builders<SourceCountryFilterDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            await db.SourceCountryFilterDB.DeleteOneAsync(filter);
            return bReturn;
        }

        public async Task<SourceCountryFilterModel> GetFilterById(string objId)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(objId));
            var builder = Builders<SourceCountryFilterDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.SourceCountryFilterDB.FindAsync(filter);
            IList<SourceCountryFilterDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                return new SourceCountryFilterModel();
            }

            var Item = results[0];

            var model = new SourceCountryFilterModel
            {
                _id = Item._id.ToString(),

                Active = Item.Active,
                CanEdit = Item.CanEdit,
                Notes = Item.Notes,
                Name = Item.Name,
                SortOrder = Item.SortOrder
            };

            var Items = new List<SourceCountryFilterListModel>();

            foreach (var m in Item.Items)
            {
                var lModel = new SourceCountryFilterListModel
                {
                    SortOrder = m.SortOrder,
                    GeoCode = m.GeoCode,
                    CountryName = m.CountryName

                };
                Items.Add(lModel);

            }
            model.Items = Items;
            return model;
        }

        public async Task<List<SourceCountryFilterListRegionModel>> GetRegionCodes()
        {
           

            var db = new DBContext();
            var cursor = await db.SourceCountryFilterDB.FindAsync(new BsonDocument());
            IList<SourceCountryFilterDB> results = cursor.ToList();

            var OutList = new List<SourceCountryFilterListRegionModel>();
            var Items = results;
            foreach (var code in Items)
            {
                var model = new SourceCountryFilterModel
                {
                    _id = code._id.ToString(),
                    Active = code.Active,
                    CanEdit = code.CanEdit,
                    Notes = code.Notes,
                    Name = code.Name,
                    SortOrder = code.SortOrder
                };

                foreach (var m in code.Items)
                {
                    var rModel = new SourceCountryFilterListRegionModel
                    {
                        SortOrder = m.SortOrder,
                        GeoCode = m.GeoCode,
                        CountryName = m.CountryName,
                        RegionName = code.Name

                    };
                    if(!string.IsNullOrEmpty(rModel.GeoCode))
                       OutList.Add(rModel);
                }
            }
           
            return OutList;
        }
     }
        
}
