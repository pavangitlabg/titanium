using System;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class ImportDataDB
    {
        public BsonObjectId _id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public int time_id { get; set; }        public int source_country_id { get; set; }        public int market_country_id { get; set; }        public int tariff_id { get; set; }        public string side_of_trade { get; set; }        public int port_id { get; set; }        public double weight { get; set; }        public double monetary_value { get; set; }        public double ytd_weight { get; set; }        public double ytd_monetary_value { get; set; }        public string estimated { get; set; }        public string coo_geo_code { get; set; }        public string cwc_geo_code { get; set; }
    }
}
