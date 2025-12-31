/*
    Services/TimeDimensionService.cs

    time dimension service
*/

using Data;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

/// <summary>time dimension service</summary>
public class TimeDimensionService
{
    /// <summary>Return all time dimensions.</summary>
    /// <returns>all time dimensions</returns>
    public async Task<List<TimeDimensionModel>> GetTimeDimensions()
    {
        var db = new DbContext();
        var cursor = await db.TimeDimensionDb.FindAsync(new BsonDocument());
        var results = cursor.ToList();

        return results.Select(x => x.ToTimeDimensionModel()).ToList();
    }
}