using MongoDB.Bson;

namespace Data.DBModels;

public class SourceCountryMappingDB
{
    public BsonObjectId _id { get; set; }
    public string Source_Country_ID { get; set; }
    public string Source_Country_Value { get; set; }
    public string ISSB_Country_Geo_Code { get; set; }
    public string COUNTRY_NAME { get; set; }
}