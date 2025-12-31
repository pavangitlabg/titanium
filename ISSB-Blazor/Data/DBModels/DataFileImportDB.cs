using MongoDB.Bson;

namespace Data.DBModels;

public class DataFileImportDB
{
    public BsonObjectId _id { get; set; }
    public long Idx { get; set; }
    public DateTime Date { get; set; }
    public string Source { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public string FileLocation { get; set; }
    public string FileName { get; set; }
    public string FileStatus { get; set; }
}