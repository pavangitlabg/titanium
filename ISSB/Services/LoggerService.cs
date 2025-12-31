using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class LoggerService
    {
        public async Task<bool> UpdateLogger(LoggerModel model)
        {
            bool bReturn = false;

            var db = new DBContext();
            var modelDB = new LoggerDB
            {
                Date = DateTime.Now,
                Message = model.Message
            };

            await db.LoggerDB.InsertOneAsync(modelDB);
            bReturn = true;

            return bReturn;


        }
    }
}
