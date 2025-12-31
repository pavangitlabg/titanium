using Data;
using Data.Models;

namespace Services;

public class DBStatsService
{
    public async Task<List<StatsModel>> GetStats()
    {
        var db = DbContext.Instance;
        var data = await db.GetStats();
        return data;
    }
}