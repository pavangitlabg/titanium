using System.Collections.Generic;

namespace Data.Models
{
    public class ExchangeModel
    {
        public string _id { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public double Value { get; set; }
        public string Date { get; set; }
        public string ReportDate { get; set; }
        public List<ExchangeListModel> Rates { get; set; }
    }

    public class ExchangeListModel
    {
        public string Currency { get; set; }
        public string Country { get; set; }
        public double Value { get; set; }
        public string Date { get; set; }
        public string ReportDate { get; set; }
    }
}
