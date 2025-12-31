/*
    Data/Models/ReportOutModel.cs

    reporting data item for extracting repository data
*/

using System.Collections.Generic;

namespace Data.Models
{
    public class ReportOutModel : ReportingModel
    {
        /// <summary>sequential ID</summary>

        public int ID { get ; set ; }

        /// <summary>time value comprising year, quarter and month</summary>

        public int TIME_ID { get ; set ; }

        /// <summary>source country geo code</summary>

        public string SC_GEO { get ; set ; }

        /// <summary>source country name</summary>

        public string SC_NAME { get ; set ; }

        /// <summary>market country geo code</summary>

        public string MC_GEO { get ; set ; }

        /// <summary>market country name</summary>

        public string MC_NAME { get ; set ; }

        /// <summary>tariff code</summary>

        public string TARIFF_CODE { get ; set ; }

        /// <summary>tariff legend</summary>

        public string TARIFF_LEGEND { get ; set ; }

        /// <summary>side of trade</summary>

        public string SIDE_OF_TRADE { get ; set ; }

        /// <summary>port ID</summary>

        public int PORT_ID { get ; set ; }

        /// <summary>port name</summary>

        public string PORT_NAME { get ; set ; }

        /// <summary>weight</summary>

        public double WEIGHT { get ; set ; }

        /// <summary>monetary value</summary>

        public double MONETARY_VALUE { get ; set ; }

        /// <summary>weight: year to date</summary>

        public double YTD_WEIGHT { get ; set ; }

        /// <summary>monetary value: year to date</summary>

        public double YTD_MONETARY_VALUE { get ; set ; }

        /// <summary>year</summary>

        public int YEAR { get ; set ; }

        /// <summary>quarter</summary>

        public int QUARTER { get ; set ; }

        /// <summary>month</summary>

        public int MONTH { get ; set ; }

        public string SOURCE_COUNTRY_TARIFF_CODE { get; set; }
        /*
            TODO Oliver ReportOutModel: Fields not used

            public int    SOURCE_COUNTRY_ID  { get ; set ; }
            public int    MARKET_COUNTRY_ID  { get ; set ; }
            public int    TARIFF_ID          { get ; set ; }

            public string CITY { get; set; }
            public string COUNTRY { get; set; }
            public float LAT { get; set; }
            public float LNG { get; set; }
        */

        /// <summary>quarter/year</summary>

        public string DateQuarter { get { return QUARTER.ToString( "0" ) + "/" + YEAR.ToString( "0000" ) ; } }

        /// <summary>month/year</summary>

        public string DateMonth { get { return MONTH.ToString( "00" ) + "/" + YEAR.ToString( "0000" ) ; } }
    }
}
