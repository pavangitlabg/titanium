using System;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class CountryCrossOverChildDB
    {
        public BsonObjectId _id { get; set; }
        public int Source_Country_ID { get; set; }
        public string Source_Country_Value { get; set; }
        public string ISSB_Country_Geo_Code { get; set; }
        public string COUNTRY_NAME { get; set; }
    }
}
