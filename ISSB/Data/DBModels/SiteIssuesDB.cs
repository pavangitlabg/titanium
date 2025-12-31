using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class SiteIssuesDB
    {
        public BsonObjectId _id { get; set; }
        public int RequestID { get; set; }
        public string User { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string Severity { get; set; }
        public string Email { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
