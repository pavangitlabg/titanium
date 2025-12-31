using System.Dynamic;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data;

public class DbContext
{
    //public IMongoCollection collection;

    private static readonly Lazy<DbContext> Lazy = new(() => new DbContext());
    private readonly MongoClientSettings clientSettings;

    private readonly MongoCredential credential;
    public string Connection = "mongodb://109.228.55.248:27017";

    public DbContext()
    {
        ConnectionString = Connection;
        DatabaseName = "ISSB_DATA";

        try
        {
            var username = "adminUser";
            var password = "Revelati0n2019$$2025";

            credential = MongoCredential.CreateCredential("admin", username, password);

            clientSettings = new MongoClientSettings
            {
                Credential = credential,
                //Server = new MongoServerAddress("109.228.55.248", 27017)
                Server = new MongoServerAddress("109.228.55.248", 27017)
            };

            IMongoClient client = new MongoClient(clientSettings);
            _database = client.GetDatabase(DatabaseName);

            // InitData();
        }
        catch (Exception ex)
        {
            throw new Exception("Can not access to db server.", ex);
        }
    }

    public static string ConnectionString { get; set; }
    public static string DatabaseName { get; set; }
    public static bool IsSSL { get; set; }

    public IMongoDatabase _database { get; set; }
    public static DbContext Instance => Lazy.Value;


    public IMongoCollection<SystemControlDB> SystemControlDb
    {
        get
        {
            var results = _database.GetCollection<SystemControlDB>("SystemControlDB");
            return results;
        }
    }

    public IMongoCollection<UserDB> UserDb
    {
        get
        {
            var results = _database.GetCollection<UserDB>("UserDB");
            return results;
        }
    }

    public IMongoCollection<TradeFactDB> TradeFactDb
    {
        get
        {
            var results = _database.GetCollection<TradeFactDB>("TradeFactDB");
            return results;
        }
    }

    public IMongoCollection<TariffDB> TariffDb
    {
        get
        {
            var results = _database.GetCollection<TariffDB>("TariffDB");
            return results;
        }
    }

    public IMongoCollection<TariffExceptionsDB> TariffExceptionsDb
    {
        get
        {
            var results = _database.GetCollection<TariffExceptionsDB>("TariffExceptionsDB");
            return results;
        }
    }

    public IMongoCollection<SourceCountryDB> SourceCountryDb
    {
        get
        {
            var results = _database.GetCollection<SourceCountryDB>("SourceCountryDB");
            return results;
        }
    }

    public IMongoCollection<MarketCountryDB> MarketCountryDb
    {
        get
        {
            var results = _database.GetCollection<MarketCountryDB>("MarketCountryDB");
            return results;
        }
    }

    public IMongoCollection<MarketCountryExceptionDB> MarketCountryExceptionDb
    {
        get
        {
            var results = _database.GetCollection<MarketCountryExceptionDB>("MarketCountryExceptionDB");
            return results;
        }
    }

    public IMongoCollection<SalesProfileDB> SalesProfileDb
    {
        get
        {
            var results = _database.GetCollection<SalesProfileDB>("SalesProfileDB");
            return results;
        }
    }

    public IMongoCollection<CustomerProfileDB> CustomerProfileDb
    {
        get
        {
            var results = _database.GetCollection<CustomerProfileDB>("CustomerProfileDB");
            return results;
        }
    }

    public IMongoCollection<AssetDB> AssetDb
    {
        get
        {
            var results = _database.GetCollection<AssetDB>("AssetDB");
            return results;
        }
    }

    public IMongoCollection<ErrorDB> ErrorDb
    {
        get
        {
            var results = _database.GetCollection<ErrorDB>("ErrorDB");
            return results;
        }
    }

    public IMongoCollection<EmailCredentialsDB> EmailCredentialsDb
    {
        get
        {
            var results = _database.GetCollection<EmailCredentialsDB>("EmailCredentialsDB");
            return results;
        }
    }

    public IMongoCollection<DataFileImportDB> DataFileImportDb
    {
        get
        {
            var results = _database.GetCollection<DataFileImportDB>("DataFileImportDB");
            return results;
        }
    }

    public IMongoCollection<WorldCityDB> WorldCityDb
    {
        get
        {
            var results = _database.GetCollection<WorldCityDB>("WorldCityDB");
            return results;
        }
    }

    public IMongoCollection<SourceCountryFilterDB> SourceCountryFilterDb
    {
        get
        {
            var results = _database.GetCollection<SourceCountryFilterDB>("SourceCountryFilterDB");
            return results;
        }
    }

    public IMongoCollection<MarketCountryFilterDB> MarketCountryFilterDb
    {
        get
        {
            var results = _database.GetCollection<MarketCountryFilterDB>("MarketCountryFilterDB");
            return results;
        }
    }

    public IMongoCollection<PortsDB> PortsDb
    {
        get
        {
            var results = _database.GetCollection<PortsDB>("PortsDB");
            return results;
        }
    }

    public IMongoCollection<TimeDimensionDB> TimeDimensionDb
    {
        get
        {
            var results = _database.GetCollection<TimeDimensionDB>("TimeDimensionDB");
            return results;
        }
    }


    public IMongoCollection<ReportingOutDB> ReportingOutDb
    {
        get
        {
            var results = _database.GetCollection<ReportingOutDB>("ReportingOutDB");
            return results;
        }
    }

    public IMongoCollection<TariffFilterDB> TariffFilterDb
    {
        get
        {
            var results = _database.GetCollection<TariffFilterDB>("TariffFilterDB");
            return results;
        }
    }


    public IMongoCollection<ProductBroadFilterDB> ProductBroadFilterDb
    {
        get
        {
            var results = _database.GetCollection<ProductBroadFilterDB>("ProductBroadFilterDB");
            return results;
        }
    }

    public IMongoCollection<ProductMedumFilterDB> ProductMedumFilterDb
    {
        get
        {
            var results = _database.GetCollection<ProductMedumFilterDB>("ProductMedumFilterDB");
            return results;
        }
    }

    public IMongoCollection<ProductAllFilterDB> ProductAllFilterDb
    {
        get
        {
            var results = _database.GetCollection<ProductAllFilterDB>("ProductAllFilterDB");
            return results;
        }
    }

    public IMongoCollection<DashBoardLayOutDB> DashBoardLayOutDb
    {
        get
        {
            var results = _database.GetCollection<DashBoardLayOutDB>("DashBoardLayOutDB");
            return results;
        }
    }

    public IMongoCollection<UserDashboardDB> UserDashboardDb
    {
        get
        {
            var results = _database.GetCollection<UserDashboardDB>("UserDashboardDB");
            return results;
        }
    }

    public IMongoCollection<DashBoardObjectsDB> DashBoardObjectsDb
    {
        get
        {
            var results = _database.GetCollection<DashBoardObjectsDB>("DashBoardObjectsDB");
            return results;
        }
    }

    public IMongoCollection<TaxCodesDB> TaxCodesDb
    {
        get
        {
            var results = _database.GetCollection<TaxCodesDB>("TaxCodesDB");
            return results;
        }
    }

    public IMongoCollection<YearsDB> YearsDb
    {
        get
        {
            var results = _database.GetCollection<YearsDB>("YearsDB");
            return results;
        }
    }

    public IMongoCollection<SavedQueryDB> SavedQueryDb
    {
        get
        {
            var results = _database.GetCollection<SavedQueryDB>("SavedQueryDB");
            return results;
        }
    }

    public IMongoCollection<TradeDataDB> TradeDataDb
    {
        get
        {
            var results = _database.GetCollection<TradeDataDB>("TradeDataDB");
            return results;
        }
    }

    public IMongoCollection<ClientDataImportDB> ClientDataImportDb
    {
        get
        {
            var results = _database.GetCollection<ClientDataImportDB>("ClientDataImportDB");
            return results;
        }
    }

    public IMongoCollection<PendingImportHeaderDB> PendingImportHeaderDb
    {
        get
        {
            var results = _database.GetCollection<PendingImportHeaderDB>("PendingImportHeaderDB");
            return results;
        }
    }

    public IMongoCollection<PendingImportDB> PendingImportDb
    {
        get
        {
            var results = _database.GetCollection<PendingImportDB>("PendingImportDB");
            return results;
        }
    }

    public IMongoCollection<ExchangeRatesDB> ExchangeRatesDb
    {
        get
        {
            var results = _database.GetCollection<ExchangeRatesDB>("ExchangeRatesDB");
            return results;
        }
    }

    public IMongoCollection<CurrencyCodesDB> CurrencyCodesDb
    {
        get
        {
            var results = _database.GetCollection<CurrencyCodesDB>("CurrencyCodesDB");
            return results;
        }
    }

    public IMongoCollection<SourceCountryMappingDB> SourceCountryMappingDb
    {
        get
        {
            var results = _database.GetCollection<SourceCountryMappingDB>("SourceCountryMappingDB");
            return results;
        }
    }

    public IMongoCollection<ImportErrorLogDB> ImportErrorLogDb
    {
        get
        {
            var results = _database.GetCollection<ImportErrorLogDB>("ImportErrorLogDB");
            return results;
        }
    }

    public IMongoCollection<ChinaUnitsofMeasurementDB> ChinaUnitsofMeasurementDb
    {
        get
        {
            var results = _database.GetCollection<ChinaUnitsofMeasurementDB>("ChinaUnitsofMeasurementDB");
            return results;
        }
    }

    public IMongoCollection<SiteVisitorDB> SiteVisitorDb
    {
        get
        {
            var results = _database.GetCollection<SiteVisitorDB>("SiteVisitorDB");
            return results;
        }
    }

    public IMongoCollection<SourceCountryExceptionDB> SourceCountryExceptionDb
    {
        get
        {
            var results = _database.GetCollection<SourceCountryExceptionDB>("SourceCountryExceptionDB");
            return results;
        }
    }

    public IMongoCollection<SiteIssuesDB> SiteIssuesDb
    {
        get
        {
            var results = _database.GetCollection<SiteIssuesDB>("SiteIssuesDB");
            return results;
        }
    }

    public IMongoCollection<RegisterDB> RegisterDb
    {
        get
        {
            var results = _database.GetCollection<RegisterDB>("RegisterDB");
            return results;
        }
    }

    public IMongoCollection<TariffIndexDB> TariffIndexDb
    {
        get
        {
            var results = _database.GetCollection<TariffIndexDB>("TariffIndexDB");
            return results;
        }
    }

    public IMongoCollection<Tariff2DigitIndexDB> Tariff2DigitIndexDb
    {
        get
        {
            var results = _database.GetCollection<Tariff2DigitIndexDB>("Tariff2DigitIndexDB");
            return results;
        }
    }

    public IMongoCollection<HomeDB> HomeDb
    {
        get
        {
            var results = _database.GetCollection<HomeDB>("HomeDB");
            return results;
        }
    }

    public IMongoCollection<ReportDesignerDB> ReportDesignerDb
    {
        get
        {
            var results = _database.GetCollection<ReportDesignerDB>("ReportDesignerDB");
            return results;
        }
    }

    public IMongoCollection<ReportKeysDB> ReportKeysDb
    {
        get
        {
            var results = _database.GetCollection<ReportKeysDB>("ReportKeysDB");
            return results;
        }
    }


    public IMongoCollection<SystemEventLogsDB> SystemEventLogsDb
    {
        get
        {
            var results = _database.GetCollection<SystemEventLogsDB>("SystemEventLogsDB");
            return results;
        }
    }

    public IMongoCollection<ImportFileTypeDB> ImportFileTypeDb
    {
        get
        {
            var results = _database.GetCollection<ImportFileTypeDB>("ImportFileTypeDB");
            return results;
        }
    }

    public IMongoCollection<EmailMessagesDB> EmailMessagesDb
    {
        get
        {
            var results = _database.GetCollection<EmailMessagesDB>("EmailMessagesDB");
            return results;
        }
    }

    public IMongoCollection<CachedReportsDB> CachedReportsDb
    {
        get
        {
            var results = _database.GetCollection<CachedReportsDB>("CachedReportsDB");
            return results;
        }
    }

    public IMongoCollection<CountryCrossOverDB> CountryCrossOverDb
    {
        get
        {
            var results = _database.GetCollection<CountryCrossOverDB>("CountryCrossOverDB");
            return results;
        }
    }

    public IMongoCollection<CountryCrossOverChildDB> CountryCrossOverChildDb
    {
        get
        {
            var results = _database.GetCollection<CountryCrossOverChildDB>("CountryCrossOverChildDB");
            return results;
        }
    }

    public IMongoCollection<OpenExchangeRatesBaseDB> OpenExchangeRatesBaseDb
    {
        get
        {
            var results = _database.GetCollection<OpenExchangeRatesBaseDB>("OpenExchangeRatesBaseDB");
            return results;
        }
    }

    public IMongoCollection<OpenAverageExchangeRatesDB> OpenAverageExchangeRatesDb
    {
        get
        {
            var results = _database.GetCollection<OpenAverageExchangeRatesDB>("OpenAverageExchangeRatesDB");
            return results;
        }
    }

    public IMongoCollection<TariffImportDB> TariffImportDb
    {
        get
        {
            var results = _database.GetCollection<TariffImportDB>("TariffImportDB");
            return results;
        }
    }

    public IMongoCollection<PendingImportAmendmentsDB> PendingImportAmendmentsDb
    {
        get
        {
            var results = _database.GetCollection<PendingImportAmendmentsDB>("PendingImportAmendmentsDB");
            return results;
        }
    }

    public IMongoCollection<LoggerDB> LoggerDb
    {
        get
        {
            var results = _database.GetCollection<LoggerDB>("LoggerDB");
            return results;
        }
    }

    public IMongoCollection<DownloadsDB> DownloadsDb
    {
        get
        {
            var results = _database.GetCollection<DownloadsDB>("DownloadsDB");
            return results;
        }
    }

    public IMongoCollection<TopHeaderDB> TopHeaderDb
    {
        get
        {
            var results = _database.GetCollection<TopHeaderDB>("TopHeaderDB");
            return results;
        }
    }

    public IMongoCollection<ReportCurrencyDB> ReportCurrencyDb
    {
        get
        {
            var results = _database.GetCollection<ReportCurrencyDB>("ReportCurrencyDB");
            return results;
        }
    }

    public IMongoCollection<AppointmentsDB> AppointmentsDb
    {
        get
        {
            var results = _database.GetCollection<AppointmentsDB>("AppointmentsDB");
            return results;
        }
    }

    public IMongoCollection<ProductCustomFilterDB> ProductCustomFilterDb
    {
        get
        {
            var results = _database.GetCollection<ProductCustomFilterDB>("ProductCustomFilterDB");
            return results;
        }
    }

    public IMongoCollection<ReportCacheDB> ReportCacheDb
    {
        get
        {
            var results = _database.GetCollection<ReportCacheDB>("ReportCacheDB");
            return results;
        }
    }

    public IMongoCollection<IndustryUserDB> IndustryUserDb
    {
        get
        {
            var results = _database.GetCollection<IndustryUserDB>("IndustryUserDB");
            return results;
        }
    }

    public IMongoCollection<FileUploadTypesDB> FileUploadTypesDb
    {
        get
        {
            var results = _database.GetCollection<FileUploadTypesDB>("FileUploadTypesDB");
            return results;
        }
    }

    public IMongoCollection<IndustryMasterDB> IndustryMasterDb
    {
        get
        {
            var results = _database.GetCollection<IndustryMasterDB>("IndustryMasterDB");
            return results;
        }
    }

    public IMongoCollection<IndDataDB> IndDataDb
    {
        get
        {
            var results = _database.GetCollection<IndDataDB>("IndDataDB");
            return results;
        }
    }

    public IMongoCollection<IndustryColumnDB> IndustryColumnDb
    {
        get
        {
            var results = _database.GetCollection<IndustryColumnDB>("IndustryColumnDB");
            return results;
        }
    }

    public IMongoCollection<IndustryRowDB> IndustryRowDb
    {
        get
        {
            var results = _database.GetCollection<IndustryRowDB>("IndustryRowDB");
            return results;
        }
    }

    public IMongoCollection<IndDataOutDB> IndDataOutDb
    {
        get
        {
            var results = _database.GetCollection<IndDataOutDB>("IndDataOutDB");
            return results;
        }
    }

    public IMongoCollection<IndustrySavedReportsDB> IndustrySavedReportsDb
    {
        get
        {
            var results = _database.GetCollection<IndustrySavedReportsDB>("IndustrySavedReportsDB");
            return results;
        }
    }

    //public async Task InitData()
    //{
    //    //IMongoClient client = new MongoClient(clientSettings);
    //    //_database = client.GetDatabase(DatabaseName);
    //    //_database.CreateCollection("UserDB", null);
    //}

    public async Task<List<StatsModel>> GetStats()
    {
        var database = _database;
        var collections = await database.ListCollectionNames().ToListAsync();

        var statsList = new List<StatsModel>();

        foreach (var collectionName in collections)
            if (!collectionName.Contains("view"))
                try
                {
                    var command = new BsonDocument { { "collStats", collectionName } };
                    var stats = await database.RunCommandAsync<BsonDocument>(command);

                    var count = stats["count"].ToDouble();

                    // var size = stats["size"];
                    // var storageSize = stats["storageSize"];
                    var size = stats["size"].ToDouble() / (1024.0 * 1024.0);
                    var storageSize = stats["storageSize"].ToDouble() / (1024.0 * 1024.0);

                    var model = new StatsModel
                    {
                        CollectionName = $"{collectionName}",
                        Count = count,
                        CountAsText = FormatNumber(count),
                        Size = size,
                        StorageSize = storageSize
                    };

                    statsList.Add(model);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

        return statsList.OrderBy(x => x.CollectionName).ToList();
    }

    private string FormatNumber(double num)
    {
        if (num >= 1_000_000_000)
            return $"Billion: {num / 1_000_000_000:F2}";
        if (num >= 1_000_000)
            return $"Million: {num / 1_000_000:F2}";
        if (num >= 1_000)
            return $"Thousand: {num / 1_000:F2}";
        if (num >= 1_00)
            return $"Hundred: {num / 1_00:F2}";
        return $"{num}";
    }

    public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
    {
        // ExpandoObject supports IDictionary so we can extend it like this
        var expandoDict = expando as IDictionary<string, object>;
        if (expandoDict.ContainsKey(propertyName))
            expandoDict[propertyName] = propertyValue;
        else
            expandoDict.Add(propertyName, propertyValue);
    }
}