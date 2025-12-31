/*
    Data/DBModels/ReportingTDB.cs

    reporting database item:
      - SC_GEO included

    TODO Oliver ReportingTDB Delete this if it is not needed.
*/

using MongoDB.Bson;

namespace Data.DBModels;

public class ReportingTDB
{
    /// <summary>grouping identifier</summary>

    public BsonDocument _id { get; set; }

    /// <summary>source country geo code</summary>

//        public string SC_GEO { get ; set ; }

    /// <summary>weight</summary>

    public double WEIGHT { get; set; }

    /// <summary>monetary value</summary>

    public double MONETARY_VALUE { get; set; }

    /// <summary>grouping identifier</summary>
    public class Id
    {
        /// <summary>source country geo code</summary>

        public string SC_GEO { get; set; }

        /// <summary>side of trade</summary>

        public string SIDE_OF_TRADE { get; set; }

        /// <summary>year</summary>

        public int YEAR { get; set; }
    }
}