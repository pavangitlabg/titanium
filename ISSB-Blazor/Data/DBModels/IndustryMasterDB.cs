using MongoDB.Bson;

namespace Data.DBModels;

public class IndustryMasterDB
{
    public BsonObjectId _id { get; set; }
    public int DataType { get; set; }
    public DateTime dateTime { get; set; }
    public string FileName { get; set; }
    public string Subject { get; set; }
}