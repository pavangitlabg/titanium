/*
    Data/DBModels/ReportTradeDataDB.cs

    reporting database item: data from TradeDataDB

    This model is used to extract data out of a BSON document from the TradeDataDB collection.
*/

using MongoDB.Bson;
using Newtonsoft.Json;

namespace Data.DBModels;

/// <summary>reporting database item: data from TradeDataDB</summary>
public class ReportTradeDataDB
{
    /// <summary>grouping identifier</summary>

    public BsonDocument _id { get; set; }

    /// <summary>time value comprising year, quarter and month</summary>

    public int TIME_ID { get; set; }

    /// <summary>source country geo code</summary>

    public string SC_GEO { get; set; }

    /// <summary>market country geo code</summary>

    public string MC_GEO { get; set; }

    /// <summary>tariff code</summary>

    public string H_TARIFF { get; set; }

    /// <summary>side of trade</summary>

    public string SIDE_OF_TRADE { get; set; }

    /// <summary>port ID</summary>

    public int PORT_ID { get; set; }

    /// <summary>weight</summary>

    public double WEIGHT { get; set; }

    /// <summary>monetary value</summary>

    public double MONETARY_VALUE { get; set; }

    /// <summary>weight: year to date</summary>

    public double YTD_WEIGHT { get; set; }

    /// <summary>monetary value: year to date</summary>

    public double YTD_MONETARY_VALUE { get; set; }

    /// <summary>tariff ID</summary>

    public int TARIFF_ID { get; set; }

    /// <summary>year</summary>

    public int YEAR { get; set; }

    /// <summary>quarter</summary>

    public int QUARTER { get; set; }

    /// <summary>month</summary>

    public int MONTH { get; set; }


    /// <summary>grouping identifier</summary>
    public class Id
    {
        /// <summary>time value comprising year, quarter and month</summary>

        public int TIME_ID { get; set; }

        /// <summary>source country geo code</summary>

        public string SC_GEO { get; set; }

        /// <summary>market country geo code</summary>

        public string MC_GEO { get; set; }

        /// <summary>tariff code</summary>

        public string H_TARIFF { get; set; }

        /// <summary>side of trade</summary>

        public string SIDE_OF_TRADE { get; set; }

        /// <summary>port ID</summary>

        public int PORT_ID { get; set; }

        /// <summary>weight</summary>

        public double WEIGHT { get; set; }

        /// <summary>monetary value</summary>

        public double MONETARY_VALUE { get; set; }

        /// <summary>tariff ID</summary>

        public int TARIFF_ID { get; set; }

        /// <summary>year</summary>

        public int YEAR { get; set; }

        /// <summary>quarter</summary>

        public int QUARTER { get; set; }

        /// <summary>month</summary>

        public int MONTH { get; set; }
    }
}

public class ReportTradeDataPivotDB
{
    [JsonProperty("PIVOT")] public IList<PIVOTDATA> PIVOT { get; set; }

    public class PIVOTDATA
    {
        [JsonProperty("_id")] public string _id { get; set; }

        [JsonProperty("data")] public IList<DATA> data { get; set; }
    }

    public class DATA
    {
        public Id _id { get; set; }
    }


    public class Id
    {
        /// <summary>grouping identifier</summary>

        public BsonDocument _id { get; set; }

        /// <summary>time value comprising year, quarter and month</summary>

        public int TIME_ID { get; set; }

        /// <summary>source country geo code</summary>

        public string SC_GEO { get; set; }

        /// <summary>market country geo code</summary>

        public string MC_GEO { get; set; }

        /// <summary>tariff code</summary>

        public string H_TARIFF { get; set; }

        /// <summary>side of trade</summary>

        public string SIDE_OF_TRADE { get; set; }

        /// <summary>port ID</summary>

        public int PORT_ID { get; set; }

        /// <summary>weight</summary>

        public double WEIGHT { get; set; }

        /// <summary>monetary value</summary>

        public double MONETARY_VALUE { get; set; }

        /// <summary>weight: year to date</summary>

        public double YTD_WEIGHT { get; set; }

        /// <summary>monetary value: year to date</summary>

        public double YTD_MONETARY_VALUE { get; set; }

        /// <summary>tariff ID</summary>

        public int TARIFF_ID { get; set; }

        /// <summary>year</summary>

        public int YEAR { get; set; }

        /// <summary>quarter</summary>

        public int QUARTER { get; set; }

        /// <summary>month</summary>

        public int MONTH { get; set; }
    }
}