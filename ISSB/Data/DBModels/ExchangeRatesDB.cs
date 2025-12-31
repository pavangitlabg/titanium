using System.Collections.Generic;
using MongoDB.Bson;

namespace Data.DBModels
{
    public class ExchangeRatesDB
    {
        public BsonObjectId _id { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public double Value { get; set; }
        public string Date { get; set; }
        public string ReportDate { get; set; }
        public List<ExchangeListDB> Rates { get; set; }
    }

    public class ExchangeListDB
    {
        public string Currency { get; set; }
        public string Country { get; set; }
        public double Value { get; set; }
        public string Date { get; set; }
        public string ReportDate { get; set; }
    }
}
