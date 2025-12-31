using System;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class LoggerDB
    {
        public BsonObjectId _id { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
