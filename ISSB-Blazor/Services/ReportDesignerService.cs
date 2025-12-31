using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services;

public class ReportDesignerService
{
    public async Task<List<ReportDesignerModel>> GetReports(string User)
    {
        var builder = Builders<ReportDesignerDB>.Filter;

        var filter = builder.Eq("User", User);
        var db = new DbContext();
        var cursor = await db.ReportDesignerDb.FindAsync(filter);

        IList<ReportDesignerDB> results = cursor.ToList();
        var modelList = new List<ReportDesignerModel>();

        foreach (var Item in results)
        {
            var model = new ReportDesignerModel
            {
                _id = Item._id.ToString(),
                CreatedDate = Item.CreatedDate,
                LastModifedDate = Item.LastModifedDate,
                Name = Item.Name,
                BuildStatus = Item.BuildStatus
                //  Content = Item.Content,
                //Format = Item.Format,
                //Header = Item.Header,
                //Landscape = Item.Landscape,
                //MarginBottom = Item.MarginBottom,
                //MarginTop = Item.MarginTop,
                //MarginLeft = Item.MarginLeft,
                //MarginRight = Item.MarginRight,
                //ReportID = Item.ReportID,
                //Scale = Item.Scale,
                //Data = Item.Data,
                //FileName = Item.FileName
            };

            modelList.Add(model);
        }

        return modelList.OrderByDescending(x => x._id).ToList();
    }

    public async Task<ReportDesignerModel> GetReportById(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<ReportDesignerDB>.Filter;

        var filter = builder.Eq("_id", RecordId);

        var db = new DbContext();
        var cursor = await db.ReportDesignerDb.FindAsync(filter);

        IList<ReportDesignerDB> results = cursor.ToList();

        var Item = results[0];

        var dbKeysList = new List<ReportKeysModel>();

        if (Item.Keys != null)
            foreach (var lItem in Item.Keys)
            {
                var lModel = new ReportKeysModel
                {
                    Description = lItem.Description,
                    IsSelected = lItem.IsSelected,
                    Key = lItem.Key,
                    Sort = lItem.Sort,
                    Title = lItem.Title,
                    Width = lItem.Width,
                    _id = lItem._id.ToString()
                };

                dbKeysList.Add(lModel);
            }

        var model = new ReportDesignerModel
        {
            _id = Item._id.ToString(),
            CreatedDate = Item.CreatedDate,
            LastModifedDate = Item.LastModifedDate,
            Name = Item.Name,
            Content = Item.Content,
            Header = Item.Header,
            Landscape = Item.Landscape,
            MarginBottom = Item.MarginBottom,
            MarginTop = Item.MarginTop,
            MarginLeft = Item.MarginLeft,
            MarginRight = Item.MarginRight,
            ReportID = Item.ReportID,
            Scale = Item.Scale,
            FileName = Item.FileName,
            PageRanges = Item.PageRanges,
            IncludeHeader = Item.IncludeHeader,
            FileType = Item.FileType,
            PageSize = Item.PageSize,
            PivotTable = Item.PivotTable,
            ValueFormat = Item.ValueFormat,
            WeightFormat = Item.WeightFormat,
            FieldSort = Item.FieldSort,
            FontSize = Item.FontSize,
            User = Item.User,
            HTML = Item.HTML,
            RunFromHTML = Item.RunFromHTML,
            TableHeader = Item.TableHeader,
            ShowTotals = Item.ShowTotals,
            TableStyle = Item.TableStyle,
            PageBreak = Item.PageBreak,
            PageBreakType = Item.PageBreakType,
            CSS = Item.CSS,
            JQUERY = Item.JQUERY,
            Keys = dbKeysList,
            ShowWeight = Item.ShowWeight,
            ShowWeightValueVpt = Item.ShowWeightValueVpt,
            ShowWeightVpt = Item.ShowWeightVpt,
            GroupByFlow = Item.GroupByFlow,
            BuildStatus = Item.BuildStatus
        };

        return model;
    }

    public async Task<ReportDesignerModel> Add(ReportDesignerModel model)
    {
        var db = new DbContext();

        var dbKeysList = new List<ReportKeysDB>();

        if (model.Keys != null)
            foreach (var lItem in model.Keys)
            {
                var lModel = new ReportKeysDB
                {
                    Description = lItem.Description,
                    IsSelected = lItem.IsSelected,
                    Key = lItem.Key,
                    Sort = lItem.Sort,
                    Title = lItem.Title,
                    Width = lItem.Width,
                    _id = new BsonObjectId(new ObjectId(lItem._id))
                };

                dbKeysList.Add(lModel);
            }

        var modelDB = new ReportDesignerDB
        {
            CreatedDate = DateTime.Now,
            LastModifedDate = DateTime.Now,
            Name = model.Name,
            Content = model.Content,
            Header = model.Header,
            Landscape = model.Landscape,
            MarginBottom = model.MarginBottom,
            MarginTop = model.MarginTop,
            MarginLeft = model.MarginLeft,
            MarginRight = model.MarginRight,
            ReportID = model.ReportID,
            Scale = model.Scale,
            FileName = model.FileName,
            PageRanges = model.PageRanges,
            IncludeHeader = model.IncludeHeader,
            FileType = model.FileType,
            PageSize = model.PageSize,
            PivotTable = model.PivotTable,
            ValueFormat = model.ValueFormat,
            WeightFormat = model.WeightFormat,
            FieldSort = model.FieldSort,
            User = model.User,
            FontSize = model.FontSize,
            HTML = model.HTML,
            RunFromHTML = model.RunFromHTML,
            TableHeader = model.TableHeader,
            ShowTotals = model.ShowTotals,
            TableStyle = model.TableStyle,
            PageBreak = model.PageBreak,
            PageBreakType = model.PageBreakType,
            CSS = model.CSS,
            JQUERY = model.JQUERY,
            Keys = dbKeysList,
            ShowWeight = model.ShowWeight,
            ShowWeightValueVpt = model.ShowWeightValueVpt,
            ShowWeightVpt = model.ShowWeightVpt,
            GroupByFlow = model.GroupByFlow
        };

        await db.ReportDesignerDb.InsertOneAsync(modelDB);
        var rModel = await GetReportById(modelDB._id.ToString());
        return rModel;
    }

    public async Task<ReportDesignerDB> Save(ReportDesignerModel model)
    {
        var dbKeysList = new List<ReportKeysDB>();

        if (model.Keys != null)
            foreach (var lItem in model.Keys)
            {
                var lModel = new ReportKeysDB
                {
                    Description = lItem.Description,
                    IsSelected = lItem.IsSelected,
                    Key = lItem.Key,
                    Sort = lItem.Sort,
                    Title = lItem.Title,
                    Width = lItem.Width,
                    _id = new BsonObjectId(new ObjectId(lItem._id))
                };

                dbKeysList.Add(lModel);
            }


        var RecordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<ReportDesignerDB>.Filter;
        var filter = builder.Eq("_id", RecordId);
        var db = new DbContext();

        var cursor = await db.ReportDesignerDb.FindAsync(filter);
        IList<ReportDesignerDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new ReportDesignerDB
        {
            _id = Item._id,
            Name = model.Name,
            CreatedDate = model.CreatedDate,
            LastModifedDate = DateTime.Now,
            Content = model.Content,
            Header = model.Header,
            Landscape = model.Landscape,
            MarginBottom = model.MarginBottom,
            MarginTop = model.MarginTop,
            MarginLeft = model.MarginLeft,
            MarginRight = model.MarginRight,
            ReportID = model.ReportID,
            Scale = model.Scale,
            FileName = model.FileName,
            PageRanges = model.PageRanges,
            IncludeHeader = model.IncludeHeader,
            FileType = model.FileType,
            PageSize = model.PageSize,
            PivotTable = model.PivotTable,
            ValueFormat = model.ValueFormat,
            WeightFormat = model.WeightFormat,
            FieldSort = model.FieldSort,
            User = model.User,
            FontSize = model.FontSize,
            HTML = model.HTML,
            RunFromHTML = model.RunFromHTML,
            TableHeader = model.TableHeader,
            ShowTotals = model.ShowTotals,
            TableStyle = model.TableStyle,
            PageBreak = model.PageBreak,
            PageBreakType = model.PageBreakType,
            CSS = model.CSS,
            JQUERY = model.JQUERY,
            Keys = dbKeysList,
            ShowWeight = model.ShowWeight,
            ShowWeightValueVpt = model.ShowWeightValueVpt,
            ShowWeightVpt = model.ShowWeightVpt,
            GroupByFlow = model.GroupByFlow,
            BuildStatus = model.BuildStatus
        };

        var returnModel = await db.ReportDesignerDb.FindOneAndReplaceAsync(filter, modelDB);

        return returnModel;
    }

    public async Task<bool> Delete(string _id)
    {
        var RecordId = new BsonObjectId(new ObjectId(_id));
        var db = new DbContext();

        var builder = Builders<ReportDesignerDB>.Filter;
        var filter = builder.Eq("_id", RecordId);

        await db.ReportDesignerDb.DeleteOneAsync(filter);

        return true;
    }

    //Test Data
    public async Task<MarketCountryJsonModel> GetMarketCountries()
    {
        var srv = new MarketCountryService();
        var data = await srv.GetMarketAllCountries();

        var CountryList = new List<MarketCountryModel>();

        foreach (var d in data) CountryList.Add(d);

        var cList = CountryList.OrderBy(s => s.MARKET_COUNTRY_ID).ToList();

        var dataOut = new MarketCountryJsonModel { Countries = cList };


        return dataOut;
    }
}