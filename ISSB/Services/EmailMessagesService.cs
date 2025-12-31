
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
    public class EmailMessagesService
    {
        public async Task<List<EmailMessagesModel>> GetMessages()
        {
            var db = new DBContext();
            var cursor = await db.EmailMessagesDB.FindAsync(new BsonDocument());

            IList<EmailMessagesDB> results = cursor.ToList();
            var modelList = new List<EmailMessagesModel>();

            foreach (var Item in results)
            {
                var model = new EmailMessagesModel
                {
                    _id = Item._id.ToString(),
                    HTML = Item.HTML,
                    MessageID = Item.MessageID,
                    Title = Item.Title
                };

                modelList.Add(model);
            }

            return modelList.OrderBy(x => x._id).ToList();
        }

        public async Task<EmailMessagesModel> GetMessageById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<EmailMessagesDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = new DBContext();
            var cursor = await db.EmailMessagesDB.FindAsync(filter);

            IList<EmailMessagesDB> results = cursor.ToList();

            var Item = results[0];

            var model = new EmailMessagesModel
            {
                _id = Item._id.ToString(),

                HTML = Item.HTML,
                MessageID = Item.MessageID,
                Title = Item.Title
            };

            return model;
        }

        public async Task<EmailMessagesModel> GetMessageByMessageId(int MessageId)
        {
          
            var builder = Builders<EmailMessagesDB>.Filter;

            var filter = builder.Eq("MessageID", MessageId);

            var db = new DBContext();
            var cursor = await db.EmailMessagesDB.FindAsync(filter);

            IList<EmailMessagesDB> results = cursor.ToList();

            var Item = results[0];

            var model = new EmailMessagesModel
            {
                _id = Item._id.ToString(),

                HTML = Item.HTML,
                MessageID = Item.MessageID,
                Title = Item.Title
            };

            return model;
        }

        public async Task<bool> Add(EmailMessagesModel model)
        {
            var db = new DBContext();

            var modelDB = new EmailMessagesDB
            {
                HTML = model.HTML,
                MessageID = model.MessageID,
                Title = model.Title
            };

            await db.EmailMessagesDB.InsertOneAsync(modelDB);

            return true;
        }

        public async Task<EmailMessagesDB> Save(EmailMessagesModel model)
        {

            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var builder = Builders<EmailMessagesDB>.Filter;
            var filter = builder.Eq("_id", RecordId);
            var db = new DBContext();

            var cursor = await db.EmailMessagesDB.FindAsync(filter);
            IList<EmailMessagesDB> results = cursor.ToList();

            var Item = results[0];

            var modelDB = new EmailMessagesDB
            {
                _id = Item._id,
                HTML = model.HTML,
                MessageID = model.MessageID,
                Title = model.Title

            };

            var returnModel = await db.EmailMessagesDB.FindOneAndReplaceAsync(filter, modelDB);

            return returnModel;

        }

        public async Task<bool> Delete(EmailMessagesModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = new DBContext();

            var builder = Builders<EmailMessagesDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.EmailMessagesDB.DeleteOneAsync(filter);

            return true;
        }
    }
}
