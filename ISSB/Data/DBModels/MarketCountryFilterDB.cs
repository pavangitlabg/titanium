using System.Collections.Generic;
using MongoDB.Bson;
namespace Data.DBModels
{

    public class MarketCountryFilterDB
    {
        public BsonObjectId _id { get; set; }
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
        public bool CanEdit { get; set; }
        public List<MarketCountryFilterListDB> Items { get; set; }

    }

    public class MarketCountryFilterListDB
    {
        public int SortOrder { get; set; }
        public string GeoCode { get; set; }
        public string CountryName { get; set; }
    }

}
