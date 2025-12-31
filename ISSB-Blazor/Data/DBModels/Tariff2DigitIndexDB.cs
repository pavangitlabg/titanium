using MongoDB.Bson;

namespace Data.DBModels;

public class Tariff2DigitIndexDB
{
    public BsonObjectId _id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
}