using Data;
using Data.DBModels;
using Data.Models;

namespace Services;

public class LoggerService
{
    public async Task<bool> UpdateLogger(LoggerModel model)
    {
        var bReturn = false;

        var db = new DbContext();
        var modelDB = new LoggerDB
        {
            Date = DateTime.Now,
            Message = model.Message
        };

        await db.LoggerDb.InsertOneAsync(modelDB);
        bReturn = true;

        return bReturn;
    }
}