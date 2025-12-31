using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class OpenExchangeAverageRateModel
    {
        public string _id { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public double Value { get; set; }
        public string Date { get; set; }
        public string ReportDate { get; set; }
        public List<OpenExchangeAverageListModel> Rates { get; set; }
    }

    public class OpenExchangeAverageListModel
    {
        public string Currency { get; set; }
        public string Country { get; set; }
        public double Value { get; set; }
        public string Date { get; set; }
        public string ReportDate { get; set; }
    }
}
