using MongoDB.Bson;

namespace Data.DBModels;

public class EmailCredentialsDB
{
    public BsonObjectId _id { get; set; }
    public string EmailAddress { get; set; }
    public string EmailPassword { get; set; }
    public string FilePath { get; set; }
    public string DownloadFilePath { get; set; }
}