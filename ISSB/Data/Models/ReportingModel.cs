/*
    Data/Models/ReportingModel.cs

    reporting data item
*/

using MongoDB.Bson ;

namespace Data.Models
{
    /// <summary>reporting data item</summary>

    public abstract class ReportingModel { public BsonObjectId _id { get ; set ; } }
}