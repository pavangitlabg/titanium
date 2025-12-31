using MongoDB.Bson;

namespace Data.DBModels;

public class ChinaUnitsofMeasurementDB
{
    public BsonObjectId _id { get; set; }
    public int Codes { get; set; }
    public string NameOfUnits { get; set; }
}