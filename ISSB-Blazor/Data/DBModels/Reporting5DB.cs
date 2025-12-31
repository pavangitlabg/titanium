/*
    Data/DBModels/Reporting5DB.cs

    reporting type 5 database item: all grouped source market products

    TODO Oliver Reporting5DB Delete this if it is not needed.
*/

using MongoDB.Bson;

namespace Data.DBModels;

/// <summary>reporting type 5 database item: all grouped source market products</summary>
public class Reporting5DB
{
    /// <summary>grouping identifier</summary>

    public BsonDocument _id { get; set; }

    /// <summary>weight</summary>

    public double WEIGHT { get; set; }

    /// <summary>monetary value</summary>

    public double MONETARY_VALUE { get; set; }

    /// <summary>grouping identifier</summary>
    public class Id
    {
        public int YEAR { get; set; }
        public string SIDE_OF_TRADE { get; set; }
    }
}