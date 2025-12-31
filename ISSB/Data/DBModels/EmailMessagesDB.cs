using System;
using MongoDB.Bson;
namespace Data.DBModels
{
    public class EmailMessagesDB
    {
        public BsonObjectId _id { get; set; }
        public int MessageID { get; set; }
        public string Title { get; set; }
        public string HTML { get; set; }
    }
}
