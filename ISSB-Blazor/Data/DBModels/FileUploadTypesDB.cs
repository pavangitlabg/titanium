using MongoDB.Bson;

namespace Data.DBModels;

public class FileUploadTypesDB
{
    public BsonObjectId _id { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
}