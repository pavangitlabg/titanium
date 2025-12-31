using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class DashboardCreatorService
{
    public async Task<bool> InsertDashRow(DashBoardLayOutModel model)
    {
        var bReturn = false;
        var db = new DbContext();

        var modelDB = new DashBoardLayOutDB
        {
            ID = model.ID,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            HTML = model.HTML
        };

        await db.DashBoardLayOutDb.InsertOneAsync(modelDB);
        bReturn = true;

        return bReturn;
    }

    public async Task<bool> InsertDashObject(DashBoardObjectsModel model)
    {
        var bReturn = false;
        var db = new DbContext();

        var modelDB = new DashBoardObjectsDB
        {
            ID = model.ID,
            Description = model.Description,
            ImageUrl = model.ImageUrl
        };

        await db.DashBoardObjectsDb.InsertOneAsync(modelDB);
        bReturn = true;

        return bReturn;
    }

    public async Task<List<DashBoardObjectsModel>> GetMenuObjects()
    {
        var db = new DbContext();
        var cursor = await db.DashBoardObjectsDb.FindAsync(new BsonDocument());

        IList<DashBoardObjectsDB> results = cursor.ToList();
        var modelList = new List<DashBoardObjectsModel>();

        foreach (var Item in results)
        {
            var model = new DashBoardObjectsModel
            {
                ID = Item.ID,
                Description = Item.Description,
                ImageUrl = Item.ImageUrl
            };

            modelList.Add(model);
        }

        return modelList.OrderBy(x => x.ID).ToList();
    }

    public async Task<List<DashBoardLayOutModel>> GetMenuRows()
    {
        var db = new DbContext();
        var cursor = await db.DashBoardLayOutDb.FindAsync(new BsonDocument());

        IList<DashBoardLayOutDB> results = cursor.ToList();
        var modelList = new List<DashBoardLayOutModel>();

        foreach (var Item in results)
        {
            var model = new DashBoardLayOutModel
            {
                ID = Item.ID,
                Description = Item.Description,
                ImageUrl = Item.ImageUrl,
                HTML = Item.HTML
            };

            modelList.Add(model);
        }

        return modelList.OrderBy(x => x.ID).ToList();
    }

    public async Task<DashBoardLayOutModel> GetMenuHTML(int ID)
    {
        var builder = Builders<DashBoardLayOutDB>.Filter;
        var filter = builder.Eq("ID", ID);

        var db = new DbContext();
        var cursor = await db.DashBoardLayOutDb.FindAsync(filter);
        IList<DashBoardLayOutDB> results = cursor.ToList();

        var Item = results[0];

        var outModel = new DashBoardLayOutModel
            { ID = Item.ID, Description = Item.Description, ImageUrl = Item.ImageUrl, HTML = Item.HTML };
        return outModel;
    }
}