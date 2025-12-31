using System;
namespace ISSB.Models
{
    public class ReportOutHelperModel
    {
        /// <summary>sequential ID</summary>

        public int ID { get; set; }

        /// <summary>time value comprising year, quarter and month</summary>

        public int TIME_ID { get; set; }

        /// <summary>source country geo code</summary>

        public string SC_GEO { get; set; }

        /// <summary>source country name</summary>

        public string SC_NAME { get; set; }

        /// <summary>market country geo code</summary>

        public string MC_GEO { get; set; }

        /// <summary>market country name</summary>

        public string MC_NAME { get; set; }

        /// <summary>tariff code</summary>

        public string TARIFF_CODE { get; set; }

        /// <summary>tariff legend</summary>

        public string TARIFF_LEGEND { get; set; }

        /// <summary>side of trade</summary>

        public string SIDE_OF_TRADE { get; set; }

        /// <summary>port ID</summary>

        public int PORT_ID { get; set; }

        /// <summary>port name</summary>

        public string PORT_NAME { get; set; }

        /// <summary>weight</summary>

        public double WEIGHT { get; set; }

        /// <summary>monetary value</summary>

        public double MONETARY_VALUE { get; set; }

        /// <summary>weight: year to date</summary>

        public double YTD_WEIGHT { get; set; }

        /// <summary>monetary value: year to date</summary>

        public double YTD_MONETARY_VALUE { get; set; }

        /// <summary>year</summary>

        public int YEAR { get; set; }

        /// <summary>quarter</summary>

        public int QUARTER { get; set; }

        /// <summary>month</summary>

        public int MONTH { get; set; }

    }
}
