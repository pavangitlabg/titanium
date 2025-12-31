using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class CRMService
    {
        public async Task<List<MarketCountryModel>> GetUsers()
        {
            var db = new DBContext();
            var cursor = await db.MarketCountryDB.FindAsync(new BsonDocument());

            IList<MarketCountryDB> results = cursor.ToList();
            var modelList = new List<MarketCountryModel>();

            foreach (var Item in results)
            {

                var model = new MarketCountryModel
                {
                    DISCONTINUED_DATE = (DateTime)Item.DISCONTINUED_DATE,
                    GEO_CODE = Item.GEO_CODE,
                    REGION_NAME = Item.REGION_NAME,
                    LONG_LEGEND = Item.LONG_LEGEND,
                    MARKET_COUNTRY_ID = Item.MARKET_COUNTRY_ID,
                    NAME = Item.NAME,
                    NAME_OLD = Item.NAME_OLD,
                    REPLACED_GEO_CODE = Item.REPLACED_GEO_CODE,
                    SHORT_LEGEND = Item.SHORT_LEGEND,
                    SOURCE_COUNTRY_INDICATOR = Item.SOURCE_COUNTRY_INDICATOR,
                    START_DATE = (DateTime)Item.START_DATE
                };

                modelList.Add(model);
            }

            return modelList;
        }
    }
}
