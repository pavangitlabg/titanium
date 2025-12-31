using System;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class ErrorDB
    {
        public BsonObjectId _id { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Class { get; set; }
        public string ErrorMessage { get; set; }
    }
}
