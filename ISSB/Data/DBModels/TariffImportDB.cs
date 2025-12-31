using System;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class TariffImportDB
    {
        public BsonObjectId _id { get; set; }
        public string TID { get; set; }
        public string HS { get; set; }
        public string TARIFF { get; set; }
        public string DESCRIPTION { get; set; }
        public string REGION_CODE { get; set; }
        public string SIDE_OF_TRADE { get; set; }
        
    }
}
