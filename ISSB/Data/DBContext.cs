using System;
using System.Collections.Generic;
using System.Dynamic;
using Data.DBModels;
using MongoDB.Driver;

namespace Data
{
    public class DBContext
    {
        public string Connection = "mongodb://109.228.60.126:27017";

        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
        public static bool IsSSL { get; set; }

        public IMongoDatabase _database { get; set; }
        //public IMongoCollection collection;

        private static readonly Lazy<DBContext> lazy = new Lazy<DBContext>(() => new DBContext());
        public static DBContext Instance { get => lazy.Value; }

        MongoCredential credential;
        MongoClientSettings clientSettings;

        public DBContext()
        {
            ConnectionString = Connection;
            DatabaseName = "ISSB_DATA";

            try
            {
                string username = "admin";
                string password = "Revelati0n2020";

                credential = MongoCredential.CreateCredential("admin", username, password);

                clientSettings = new MongoClientSettings
                {
                    Credential = credential,
                    Server = new MongoServerAddress("109.228.60.126", 27017)
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

        //public async Task InitData()
        //{
        //    //IMongoClient client = new MongoClient(clientSettings);
        //    //_database = client.GetDatabase(DatabaseName);
        //    //_database.CreateCollection("UserDB", null);
        //}

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }


        public IMongoCollection<SystemControlDB> SystemControlDB
        {
            get
            {
                var results = _database.GetCollection<SystemControlDB>("SystemControlDB");
                return results;
            }
        }
        public IMongoCollection<UserDB> UserDB
        {
            get
            {
                var results = _database.GetCollection<UserDB>("UserDB");
                return results;
            }
        }

        public IMongoCollection<TradeFactDB> TradeFactDB
        {
            get
            {
                var results = _database.GetCollection<TradeFactDB>("TradeFactDB");
                return results;
            }
        }

        public IMongoCollection<TariffDB> TariffDB
        {
            get
            {
                var results = _database.GetCollection<TariffDB>("TariffDB");
                return results;
            }
        }

        public IMongoCollection<TariffExceptionsDB> TariffExceptionsDB
        {
            get
            {
                var results = _database.GetCollection<TariffExceptionsDB>("TariffExceptionsDB");
                return results;
            }
        }

        public IMongoCollection<SourceCountryDB> SourceCountryDB
        {
            get
            {
                var results = _database.GetCollection<SourceCountryDB>("SourceCountryDB");
                return results;
            }
        }

        public IMongoCollection<MarketCountryDB> MarketCountryDB
        {
            get
            {
                var results = _database.GetCollection<MarketCountryDB>("MarketCountryDB");
                return results;
            }
        }
    
        public IMongoCollection<MarketCountryExceptionDB> MarketCountryExceptionDB
        {
            get
            {
                var results = _database.GetCollection<MarketCountryExceptionDB>("MarketCountryExceptionDB");
                return results;
            }
        }

        public IMongoCollection<SalesProfileDB> SalesProfileDB
        {
            get
            {
                var results = _database.GetCollection<SalesProfileDB>("SalesProfileDB");
                return results;
            }
        }
        
        public IMongoCollection<CustomerProfileDB> CustomerProfileDB
        {
            get
            {
                var results = _database.GetCollection<CustomerProfileDB>("CustomerProfileDB");
                return results;
            }
        }

        public IMongoCollection<AssetDB> AssetDB
        {
            get
            {
                var results = _database.GetCollection<AssetDB>("AssetDB");
                return results;
            }
        }

        public IMongoCollection<ErrorDB> ErrorDB
        {
            get
            {
                var results = _database.GetCollection<ErrorDB>("ErrorDB");
                return results;
            }
        }

        public IMongoCollection<EmailCredentialsDB> EmailCredentialsDB
        {
            get
            {
                var results = _database.GetCollection<EmailCredentialsDB>("EmailCredentialsDB");
                return results;
            }
        }

        public IMongoCollection<DataFileImportDB> DataFileImportDB
        {
            get
            {
                var results = _database.GetCollection<DataFileImportDB>("DataFileImportDB");
                return results;
            }
        }

        public IMongoCollection<WorldCityDB> WorldCityDB
        {
            get
            {
                var results = _database.GetCollection<WorldCityDB>("WorldCityDB");
                return results;
            }
        }

        public IMongoCollection<SourceCountryFilterDB> SourceCountryFilterDB
        {
            get
            {
                var results = _database.GetCollection<SourceCountryFilterDB>("SourceCountryFilterDB");
                return results;
            }
        }

        public IMongoCollection<MarketCountryFilterDB> MarketCountryFilterDB
        {
            get
            {
                var results = _database.GetCollection<MarketCountryFilterDB>("MarketCountryFilterDB");
                return results;
            }
        }

        public IMongoCollection<PortsDB> PortsDB
        {
            get
            {
                var results = _database.GetCollection<PortsDB>("PortsDB");
                return results;
            }
        }

        public IMongoCollection<TimeDimensionDB> TimeDimensionDB
        {
            get
            {
                var results = _database.GetCollection<TimeDimensionDB>("TimeDimensionDB");
                return results;
            }
        }



        public IMongoCollection<ReportingOutDB> ReportingOutDB
        {
            get
            {
                var results = _database.GetCollection<ReportingOutDB>("ReportingOutDB");
                return results;
            }
        }

        public IMongoCollection<TariffFilterDB> TariffFilterDB
        {
            get
            {
                var results = _database.GetCollection<TariffFilterDB>("TariffFilterDB");
                return results;
            }
        }


        public IMongoCollection<ProductBroadFilterDB> ProductBroadFilterDB
        {
            get
            {
                var results = _database.GetCollection<ProductBroadFilterDB>("ProductBroadFilterDB");
                return results;
            }
        }

        public IMongoCollection<ProductMedumFilterDB> ProductMedumFilterDB
        {
            get
            {
                var results = _database.GetCollection<ProductMedumFilterDB>("ProductMedumFilterDB");
                return results;
            }
        }

        public IMongoCollection<ProductAllFilterDB> ProductAllFilterDB
        {
            get
            {
                var results = _database.GetCollection<ProductAllFilterDB>("ProductAllFilterDB");
                return results;
            }
        }

        public IMongoCollection<DashBoardLayOutDB> DashBoardLayOutDB
        {
            get
            {
                var results = _database.GetCollection<DashBoardLayOutDB>("DashBoardLayOutDB");
                return results;
            }
        }

        public IMongoCollection<UserDashboardDB> UserDashboardDB
        {
            get
            {
                var results = _database.GetCollection<UserDashboardDB>("UserDashboardDB");
                return results;
            }
        }

        public IMongoCollection<DashBoardObjectsDB> DashBoardObjectsDB
        {
            get
            {
                var results = _database.GetCollection<DashBoardObjectsDB>("DashBoardObjectsDB");
                return results;
            }
        }

        public IMongoCollection<TaxCodesDB> TaxCodesDB
        {
            get
            {
                var results = _database.GetCollection<TaxCodesDB>("TaxCodesDB");
                return results;
            }
        }

        public IMongoCollection<YearsDB> YearsDB
        {
            get
            {
                var results = _database.GetCollection<YearsDB>("YearsDB");
                return results;
            }
        }

        public IMongoCollection<SavedQueryDB> SavedQueryDB
        {
            get
            {
                var results = _database.GetCollection<SavedQueryDB>("SavedQueryDB");
                return results;
            }
        }

        public IMongoCollection<TradeDataDB> TradeDataDB
        {
            get
            {
                var results = _database.GetCollection<TradeDataDB>("TradeDataDB");
                return results;
            }
        }

        public IMongoCollection<ClientDataImportDB> ClientDataImportDB
        {
            get
            {
                var results = _database.GetCollection<ClientDataImportDB>("ClientDataImportDB");
                return results;
            }
        }

        public IMongoCollection<PendingImportHeaderDB> PendingImportHeaderDB
        {
            get
            {
                var results = _database.GetCollection<PendingImportHeaderDB>("PendingImportHeaderDB");
                return results;
            }
        }

        public IMongoCollection<PendingImportDB> PendingImportDB
        {
            get
            {
                var results = _database.GetCollection<PendingImportDB>("PendingImportDB");
                return results;
            }
        }
        
        public IMongoCollection<ExchangeRatesDB> ExchangeRatesDB
        {
            get
            {
                var results = _database.GetCollection<ExchangeRatesDB>("ExchangeRatesDB");
                return results;
            }
        }

        public IMongoCollection<CurrencyCodesDB> CurrencyCodesDB
        {
            get
            {
                var results = _database.GetCollection<CurrencyCodesDB>("CurrencyCodesDB");
                return results;
            }
        }

        public IMongoCollection<SourceCountryMappingDB> SourceCountryMappingDB
        {
            get
            {
                var results = _database.GetCollection<SourceCountryMappingDB>("SourceCountryMappingDB");
                return results;
            }
        }

        public IMongoCollection<ImportErrorLogDB> ImportErrorLogDB
        {
            get
            {
                var results = _database.GetCollection<ImportErrorLogDB>("ImportErrorLogDB");
                return results;
            }
        }
        
        public IMongoCollection<ChinaUnitsofMeasurementDB> ChinaUnitsofMeasurementDB
        {
            get
            {
                var results = _database.GetCollection<ChinaUnitsofMeasurementDB>("ChinaUnitsofMeasurementDB");
                return results;
            }
        }
        
        public IMongoCollection<SiteVisitorDB> SiteVisitorDB
        {
            get
            {
                var results = _database.GetCollection<SiteVisitorDB>("SiteVisitorDB");
                return results;
            }
        }

        public IMongoCollection<SourceCountryExceptionDB> SourceCountryExceptionDB
        {
            get
            {
                var results = _database.GetCollection<SourceCountryExceptionDB>("SourceCountryExceptionDB");
                return results;
            }
        }

        public IMongoCollection<SiteIssuesDB> SiteIssuesDB
        {
            get
            {
                var results = _database.GetCollection<SiteIssuesDB>("SiteIssuesDB");
                return results;
            }
        }
        public IMongoCollection<RegisterDB> RegisterDB
        {
            get
            {
                var results = _database.GetCollection<RegisterDB>("RegisterDB");
                return results;
            }
        }
        
        public IMongoCollection<TariffIndexDB> TariffIndexDB
        {
            get
            {
                var results = _database.GetCollection<TariffIndexDB>("TariffIndexDB");
                return results;
            }
        }
        
        public IMongoCollection<Tariff2DigitIndexDB> Tariff2DigitIndexDB
        {
            get
            {
                var results = _database.GetCollection<Tariff2DigitIndexDB>("Tariff2DigitIndexDB");
                return results;
            }
        }
        public IMongoCollection<HomeDB> HomeDB
        {
            get
            {
                var results = _database.GetCollection<HomeDB>("HomeDB");
                return results;
            }
        }
        
        public IMongoCollection<ReportDesignerDB> ReportDesignerDB
        {
            get
            {
                var results = _database.GetCollection<ReportDesignerDB>("ReportDesignerDB");
                return results;
            }
        }
        
        public IMongoCollection<ReportKeysDB> ReportKeysDB
        {
            get
            {
                var results = _database.GetCollection<ReportKeysDB>("ReportKeysDB");
                return results;
            }
        }


        public IMongoCollection<SystemEventLogsDB> SystemEventLogsDB
        {
            get
            {
                var results = _database.GetCollection<SystemEventLogsDB>("SystemEventLogsDB");
                return results;
            }
        }
        
        public IMongoCollection<ImportFileTypeDB> ImportFileTypeDB
        {
            get
            {
                var results = _database.GetCollection<ImportFileTypeDB>("ImportFileTypeDB");
                return results;
            }
        }
        
        public IMongoCollection<EmailMessagesDB> EmailMessagesDB
        {
            get
            {
                var results = _database.GetCollection<EmailMessagesDB>("EmailMessagesDB");
                return results;
            }
        }
        
        public IMongoCollection<CachedReportsDB> CachedReportsDB
        {
            get
            {
                var results = _database.GetCollection<CachedReportsDB>("CachedReportsDB");
                return results;
            }
        }
        
        public IMongoCollection<CountryCrossOverDB> CountryCrossOverDB
        {
            get
            {
                var results = _database.GetCollection<CountryCrossOverDB>("CountryCrossOverDB");
                return results;
            }
        }

        public IMongoCollection<CountryCrossOverChildDB> CountryCrossOverChildDB
        {
            get
            {
                var results = _database.GetCollection<CountryCrossOverChildDB>("CountryCrossOverChildDB");
                return results;
            }
        }
        
        public IMongoCollection<OpenExchangeRatesBaseDB> OpenExchangeRatesBaseDB
        {
            get
            {
                var results = _database.GetCollection<OpenExchangeRatesBaseDB>("OpenExchangeRatesBaseDB");
                return results;
            }
        }
        
        public IMongoCollection<OpenAverageExchangeRatesDB> OpenAverageExchangeRatesDB
        {
            get
            {
                var results = _database.GetCollection<OpenAverageExchangeRatesDB>("OpenAverageExchangeRatesDB");
                return results;
            }
        }
        
        public IMongoCollection<TariffImportDB> TariffImportDB
        {
            get
            {
                var results = _database.GetCollection<TariffImportDB>("TariffImportDB");
                return results;
            }
        }
        
        public IMongoCollection<PendingImportAmendmentsDB> PendingImportAmendmentsDB
        {
            get
            {
                var results = _database.GetCollection<PendingImportAmendmentsDB>("PendingImportAmendmentsDB");
                return results;
            }
        }

        public IMongoCollection<LoggerDB> LoggerDB
        {
            get
            {
                var results = _database.GetCollection<LoggerDB>("LoggerDB");
                return results;
            }
        }
    
        public IMongoCollection<DownloadsDB> DownloadsDB
        {
            get
            {
                var results = _database.GetCollection<DownloadsDB>("DownloadsDB");
                return results;
            }
        }
        
        public IMongoCollection<TopHeaderDB> TopHeaderDB
        {
            get
            {
                var results = _database.GetCollection<TopHeaderDB>("TopHeaderDB");
                return results;
            }
        }

        public IMongoCollection<ReportCurrencyDB> ReportCurrencyDB
        {
            get
            {
                var results = _database.GetCollection<ReportCurrencyDB>("ReportCurrencyDB");
                return results;
            }
        }
        
        public IMongoCollection<AppointmentsDB> AppointmentsDB
        {
            get
            {
                var results = _database.GetCollection<AppointmentsDB>("AppointmentsDB");
                return results;
            }
        }
        
        public IMongoCollection<ProductCustomFilterDB> ProductCustomFilterDB
        {
            get
            {
                var results = _database.GetCollection<ProductCustomFilterDB>("ProductCustomFilterDB");
                return results;
            }
        }
        
        public IMongoCollection<ReportCacheDB> ReportCacheDB
        {
            get
            {
                var results = _database.GetCollection<ReportCacheDB>("ReportCacheDB");
                return results;
            }
        }
        
        public IMongoCollection<IndustryUserDB> IndustryUserDB
        {
            get
            {
                var results = _database.GetCollection<IndustryUserDB>("IndustryUserDB");
                return results;
            }
        }
        
        public IMongoCollection<FileUploadTypesDB> FileUploadTypesDB
        {
            get
            {
                var results = _database.GetCollection<FileUploadTypesDB>("FileUploadTypesDB");
                return results;
            }
        }
        
        public IMongoCollection<IndustryMasterDB> IndustryMasterDB
        {
            get
            {
                var results = _database.GetCollection<IndustryMasterDB>("IndustryMasterDB");
                return results;
            }
        }

        public IMongoCollection<IndDataDB> IndDataDB
        {
            get
            {
                var results = _database.GetCollection<IndDataDB>("IndDataDB");
                return results;
            }
        }
        
        public IMongoCollection<IndustryColumnDB> IndustryColumnDB
        {
            get
            {
                var results = _database.GetCollection<IndustryColumnDB>("IndustryColumnDB");
                return results;
            }
        }
        
        public IMongoCollection<IndustryRowDB> IndustryRowDB
        {
            get
            {
                var results = _database.GetCollection<IndustryRowDB>("IndustryRowDB");
                return results;
            }
        }
        
        public IMongoCollection<IndDataOutDB> IndDataOutDB
        {
            get
            {
                var results = _database.GetCollection<IndDataOutDB>("IndDataOutDB");
                return results;
            }
        }
        
        public IMongoCollection<IndustrySavedReportsDB> IndustrySavedReportsDB
        {
            get
            {
                var results = _database.GetCollection<IndustrySavedReportsDB>("IndustrySavedReportsDB");
                return results;
            }
        }
    }
}
