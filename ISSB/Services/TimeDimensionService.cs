/*
    Services/TimeDimensionService.cs

    time dimension service
*/

using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using Data ;
using Data.Models ;
using MongoDB.Bson ;
using MongoDB.Driver ;

namespace Services
{
    /// <summary>time dimension service</summary>

    public class TimeDimensionService
    {
        /// <summary>Return all time dimensions.</summary>
        /// <returns>all time dimensions</returns>

        public async Task<List<TimeDimensionModel>> GetTimeDimensions()
        {
            var db = new DBContext() ;
            var cursor = await db.TimeDimensionDB.FindAsync( new BsonDocument() ) ;
            var results = cursor.ToList() ;

            return results.Select( x => x.ToTimeDimensionModel() ).ToList() ;
        }
    }
}