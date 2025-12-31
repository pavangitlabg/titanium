using MongoDB.Bson;

namespace Data.DBModels;

public class ProductAllFilterDB
{
    public BsonObjectId _id { get; set; }
    public int Idx { get; set; }
    public int SortOrder { get; set; }
    public string TariffCode { get; set; }
    public string Name { get; set; }
    public double Cost { get; set; }
}