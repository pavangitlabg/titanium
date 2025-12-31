using System;
namespace Data.Models
{
    public class ReportCSVModel
    {
        public int ID { get; set; }
        public int TIME_ID { get; set; }
        public string SC_GEO { get; set; }
        public string SC_NAME { get; set; }
        public string MC_GEO { get; set; }
        public string MC_NAME { get; set; }
        public string TARIFF_CODE { get; set; }
        public string TARIFF_LEGEND { get; set; }
        public string SIDE_OF_TRADE { get; set; }
        public int PORT_ID { get; set; }
        public string PORT_NAME { get; set; }
        public double WEIGHT { get; set; }
        public double MONETARY_VALUE { get; set; }
        public double YTD_WEIGHT { get; set; }
        public double YTD_MONETARY_VALUE { get; set; }
        public int YEAR { get; set; }
        public int QUARTER { get; set; }
        public int MONTH { get; set; }
        public string DateQuarter { get { return QUARTER.ToString("0") + "/" + YEAR.ToString("0000"); } }
        public string DateMonth { get { return MONTH.ToString("00") + "/" + YEAR.ToString("0000"); } }
    }
}
