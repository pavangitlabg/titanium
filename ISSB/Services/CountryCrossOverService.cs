
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class CountryCrossOverService
    {
        public async Task<List<CountryCrossOverModel>> GetCountries()
        {
            var db = new DBContext();
            var cursor = await db.CountryCrossOverDB.FindAsync(new BsonDocument());

            IList<CountryCrossOverDB> results = cursor.ToList();
            var modelList = new List<CountryCrossOverModel>();

           

            foreach (var Item in results)
            {
                var modelListChild = new List<CountryCrossOverChildModel>();
                foreach (var ItemChild in Item.Countries)
                {
                    var modelChild = new CountryCrossOverChildModel
                    {
                        _id = ItemChild._id.ToString(),
                        NAME = ItemChild.COUNTRY_NAME,
                        GEO_CODE = ItemChild.ISSB_Country_Geo_Code,
                        Source_Country_ID = ItemChild.Source_Country_ID,
                        Source_Country_Value = ItemChild.Source_Country_Value
                    };

                    modelListChild.Add(modelChild);
                }

                var model = new CountryCrossOverModel
                {
                    _id = Item._id.ToString(),
                    Active = Item.Active,
                    BITS_Processing_Indicator = Item.BITS_Processing_Indicator,
                    Country_ID = Item.Country_ID ?? 0,
                    Currency_ID = Item.Currency_ID ?? 0,
                    Currency_Units = Item.Currency_Units ?? 0,
                    Data_Format = Item.Data_Format,
                    GEO_CODE = Item.Geo_Code,
                    Geo_Code_Discontinued_Date = Item.Geo_Code_Discontinued_Date,
                    Long_Legend = Item.Long_Legend,
                    Market_Country_Indicator = Item.Market_Country_Indicator,
                    NAME = Item.Name,
                    Old_Geo_Code = Item.Old_Geo_Code,
                    RedBrick_Market_Country_ID = Item.RedBrick_Market_Country_ID,
                    RedBrick_Source_Country_ID = Item.RedBrick_Source_Country_ID,
                    Region_ID = Item.Region_ID,
                    Short_Legend = Item.Short_Legend,
                    Side_Of_Trade = Item.Side_Of_Trade,
                    Source_Country_First_Date = Item.Source_Country_First_Date,
                    Source_Country_Indicator = Item.Source_Country_Indicator,
                    Start_Date = Item.Start_Date,
                    Tariff_Code_Table_Code = Item.Tariff_Code_Table_Code,
                    Countries = modelListChild
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<List<CountryCrossOverChildModel>> GetChildCountries(int Source_Country_ID)
        {
            var builder = Builders<CountryCrossOverChildDB>.Filter;
            var filter = builder.Eq("Source_Country_ID", Source_Country_ID);

            var db = new DBContext();
            var cursor = await db.CountryCrossOverChildDB.FindAsync(filter);

            IList<CountryCrossOverChildDB> results = cursor.ToList();
            var modelList = new List<CountryCrossOverChildModel>();

            foreach (var Item in results)
            {

                var model = new CountryCrossOverChildModel
                {
                    _id = Item._id.ToString(),
                    NAME = Item.COUNTRY_NAME,
                    GEO_CODE = Item.ISSB_Country_Geo_Code,
                    Source_Country_ID = Item.Source_Country_ID,
                    Source_Country_Value = Item.Source_Country_Value
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public async Task<CountryCrossOverChildModel> GetChildCountrySC_GEO(string SC_GEO, string MC_GEO)
        {
            var builder = Builders<CountryCrossOverDB>.Filter;
            var filter = builder.Eq("Geo_Code", SC_GEO) & builder.Eq("Active", true);

            var db = new DBContext();
            var cursor = await db.CountryCrossOverDB.FindAsync(filter);

            IList<CountryCrossOverDB> results = cursor.ToList();

            if (results.Count == 0)
                return new CountryCrossOverChildModel();

            var modelList = new List<CountryCrossOverChildModel>();
            foreach (var Item in results[0].Countries)
            {
                var model = new CountryCrossOverChildModel
                {
                    _id = Item._id.ToString(),
                    NAME = Item.COUNTRY_NAME,
                    GEO_CODE = Item.ISSB_Country_Geo_Code,
                    Source_Country_ID = Item.Source_Country_ID,
                    Source_Country_Value = Item.Source_Country_Value
                };

                modelList.Add(model);
            }
            var outModel = modelList.Find(x => x.Source_Country_Value.Equals(MC_GEO));

            return outModel;
        }

        public async Task<List<CountryCrossOverChildModel>> GetChildCountryList(string SC_GEO)
        {
            var builder = Builders<CountryCrossOverDB>.Filter;
            var filter = builder.Eq("Geo_Code", SC_GEO) & builder.Eq("Active", true);

            var db = new DBContext();
            var cursor = await db.CountryCrossOverDB.FindAsync(filter);

            IList<CountryCrossOverDB> results = cursor.ToList();

            if (results.Count == 0)
                return new List<CountryCrossOverChildModel>();

            var modelList = new List<CountryCrossOverChildModel>();
            foreach (var Item in results[0].Countries)
            {
                var model = new CountryCrossOverChildModel
                {
                    _id = Item._id.ToString(),
                    NAME = Item.COUNTRY_NAME,
                    GEO_CODE = Item.ISSB_Country_Geo_Code,
                    Source_Country_ID = Item.Source_Country_ID,
                    Source_Country_Value = Item.Source_Country_Value
                };

                modelList.Add(model);
            }
            //var outModel = modelList.Find(x => x.Source_Country_Value.Equals(MC_GEO));

            return modelList;
        }


        public async Task<bool> Save(CountryCrossOverModel model)
        {
            var ChildList = new List<CountryCrossOverChildDB>();

            foreach(var cItem in model.Countries)
            {
                BsonObjectId RecordChildId = new BsonObjectId(new ObjectId(cItem._id));

                var cModel = new CountryCrossOverChildDB
                {
                    _id = RecordChildId,
                    COUNTRY_NAME = cItem.NAME,
                    ISSB_Country_Geo_Code = cItem.GEO_CODE,
                    Source_Country_ID = cItem.Source_Country_ID,
                    Source_Country_Value = cItem.Source_Country_Value
                };
                ChildList.Add(cModel);
            }


            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<CountryCrossOverDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.CountryCrossOverDB.FindAsync(filter);
            IList<CountryCrossOverDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new CountryCrossOverDB
            {
                _id = RecordId,
                Active = Item.Active,
                BITS_Processing_Indicator = Item.BITS_Processing_Indicator,
                Country_ID = Item.Country_ID ?? 0,
                Currency_ID = Item.Currency_ID ?? 0,
                Currency_Units = Item.Currency_Units ?? 0,
                Data_Format = Item.Data_Format,
                Geo_Code = Item.Geo_Code,
                Geo_Code_Discontinued_Date = Item.Geo_Code_Discontinued_Date,
                Long_Legend = Item.Long_Legend,
                Market_Country_Indicator = Item.Market_Country_Indicator,
                Name = Item.Name,
                Old_Geo_Code = Item.Old_Geo_Code,
                RedBrick_Market_Country_ID = Item.RedBrick_Market_Country_ID,
                RedBrick_Source_Country_ID = Item.RedBrick_Source_Country_ID,
                Region_ID = Item.Region_ID,
                Short_Legend = Item.Short_Legend,
                Side_Of_Trade = Item.Side_Of_Trade,
                Source_Country_First_Date = Item.Source_Country_First_Date,
                Source_Country_Indicator = Item.Source_Country_Indicator,
                Start_Date = Item.Start_Date,
                Tariff_Code_Table_Code = Item.Tariff_Code_Table_Code,
                Countries = ChildList
            };

            await db.CountryCrossOverDB.FindOneAndReplaceAsync(filter, modelDB);

            return true;

        }
    }
}
