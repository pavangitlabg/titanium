using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Services;

public class OpenExchangeRateService
{
    private static OpenExchangeRateService _instance;
    private readonly string ApiKey = "469cf477bda74d23ad165b1ac78c8d62";

    public static OpenExchangeRateService Instance
    {
        get
        {
            if (_instance == null) _instance = new OpenExchangeRateService();
            return _instance;
        }
    }

    public async Task<List<OpenAverageExchangeRatesModel>> GetRates()
    {
        var db = new DbContext();
        var cursor = await db.OpenAverageExchangeRatesDb.FindAsync(new BsonDocument());

        IList<OpenAverageExchangeRatesDB> results = cursor.ToList();
        var modelList = new List<OpenAverageExchangeRatesModel>();

        foreach (var Item in results)
        {
            var cList = new List<OpenAverageExchangeListModel>();
            foreach (var lItem in Item.Rates)
            {
                var cModel = new OpenAverageExchangeListModel
                    { Country = lItem.Country, Currency = lItem.Currency, Value = lItem.Value };
                cList.Add(cModel);
            }

            var model = new OpenAverageExchangeRatesModel
            {
                _id = Item._id.ToString(),
                Currency = Item.Currency,
                Date = Item.Date,
                ReportDate = Item.ReportDate,
                Country = Item.Country,
                Value = Item.Value,
                Rates = cList
            };

            modelList.Add(model);
        }

        return modelList.OrderByDescending(x => x._id).ToList();
    }

    public async Task<OpenExchangeRatesBaseModel> GetRateByDayRange(string start, string end)
    {
        dynamic jsonResponse = null;
        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://openexchangerates.org/api/time-series.json?app_id=" +
                                                         ApiKey + "&start=" + start + "&end=" + end + "&base=GBP");
                var apiResponse = await response.Content.ReadAsStringAsync();
                jsonResponse = JsonConvert.DeserializeObject(apiResponse);
            }
        }
        catch (HttpRequestException e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "989", Class = "OpenExchangeRateService Line 48", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);
        }

        var starts = start.Split('-');
        var RateModel = new OpenExchangeRatesBaseModel();
        RateModel.Disclaimer = jsonResponse.disclaimer;
        RateModel.Base = jsonResponse.@base;
        RateModel.Start_date = jsonResponse.start_date;
        RateModel.End_date = jsonResponse.end_date;
        RateModel.License = jsonResponse.license;
        RateModel.Month = int.Parse(starts[1]);
        RateModel.Year = int.Parse(starts[0]);

        var RateList = new List<OpenExchangeRatesPeriodModel>();

        foreach (var jPeriod in jsonResponse.rates)
        {
            var RatesPeriodModel = new OpenExchangeRatesPeriodModel
            {
                Name = jPeriod.Name
            };
            var RatesList = new List<OpenExchangeRatesModel>();
            foreach (var jRates in jPeriod.Last)
            {
                var RatesModel = new OpenExchangeRatesModel { Name = jRates.Name, Rate = jRates.Value };
                RatesList.Add(RatesModel);
            }

            RatesPeriodModel.Rates = RatesList;

            RateList.Add(RatesPeriodModel);
        }

        RateModel.Rates = RateList;

        //Calculate the Average
        await Delete(RateModel);
        //Save to DB
        await Add(RateModel);
        return RateModel;
    }

    public async Task<bool> Add(OpenExchangeRatesBaseModel model)
    {
        var db = new DbContext();

        var periodList = new List<OpenExchangeRatesPeriodDB>();

        foreach (var pItems in model.Rates)
        {
            var periodModel = new OpenExchangeRatesPeriodDB
                { Name = pItems.Name };
            var RatesList = new List<OpenExchangeRatesDB>();
            foreach (var rItem in pItems.Rates)
            {
                var rateModel = new OpenExchangeRatesDB { Name = rItem.Name, Rate = rItem.Rate };
                RatesList.Add(rateModel);
            }

            periodModel.Rates = RatesList;
            periodList.Add(periodModel);
        }

        var modelDB = new OpenExchangeRatesBaseDB
        {
            Base = model.Base,
            Disclaimer = model.Disclaimer,
            End_date = model.End_date,
            License = model.License,
            Month = model.Month,
            Start_date = model.Start_date,
            Year = model.Year,
            Rates = periodList
        };

        await db.OpenExchangeRatesBaseDb.InsertOneAsync(modelDB);

        return true;
    }

    public async Task<bool> Delete(OpenExchangeRatesBaseModel model)
    {
        //BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
        var db = new DbContext();

        var builder = Builders<OpenExchangeRatesBaseDB>.Filter;
        var filter = builder.Eq("Year", model.Year) & builder.Eq("Month", model.Month);

        await db.OpenExchangeRatesBaseDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<bool> RebuildRates()
    {
        var exRateSrv = new OpenExchangeRateService();
        var ToDay = DateTime.Now.ToString("dd.MM.yyyy");
        var dates = ToDay.Split('.');
        var iDay = int.Parse(dates[0]);
        var iMonth = int.Parse(dates[1]);
        var iYear = int.Parse(dates[2]);


        for (var y = 1999; y <= iYear; y++)
        {
            var Start = string.Empty;
            var End = string.Empty;
            var Year = y.ToString();
            var Month = string.Empty;
            var LastDayOfMonth = string.Empty;
            for (var m = 1; m <= 12; m++)
            {
                Month = m.ToString().PadLeft(2, '0');
                LastDayOfMonth = DateTime.DaysInMonth(y, m).ToString();
                Start = Year + "-" + Month + "-01";
                End = Year + "-" + Month + "-" + LastDayOfMonth;

                var TestDate = DateTime.Parse(End);

                if (TestDate <= DateTime.Now) await exRateSrv.GetRateByDayRange(Start, End);
            }
        }

        return true;
    }

    public async Task<OpenExchangeRatesBaseModel> GetExchangeRateItem(int Year, int Month)
    {
        var db = new DbContext();
        var builder = Builders<OpenExchangeRatesBaseDB>.Filter;
        var filter = builder.Eq("Year", Year) & builder.Eq("Month", Month);

        var cursor = await db.OpenExchangeRatesBaseDb.FindAsync(filter);

        IList<OpenExchangeRatesBaseDB> results = cursor.ToList();
        //var modelList = new List<OpenExchangeRatesBaseModel>();
        var Item = results[0];

        var cList = new List<OpenExchangeRatesPeriodModel>();
        foreach (var lItem in Item.Rates)
        {
            var cModel = new OpenExchangeRatesPeriodModel
            {
                Name = lItem.Name
            };

            var eRates = new List<OpenExchangeRatesModel>();
            foreach (var rItem in lItem.Rates)
            {
                var rModel = new OpenExchangeRatesModel
                {
                    Name = rItem.Name,
                    Rate = rItem.Rate
                };
                eRates.Add(rModel);
            }

            cModel.Rates = eRates;
            cList.Add(cModel);
        }

        var model = new OpenExchangeRatesBaseModel
        {
            _id = Item._id.ToString(),
            Base = Item.Base,
            Disclaimer = Item.Disclaimer,
            End_date = Item.End_date,
            License = Item.License,
            Month = Item.Month,
            Start_date = Item.Start_date,
            Year = Item.Year,
            Rates = cList
        };
        return model;
    }

    public async Task<OpenAverageExchangeRatesModel> SetAverageTable(int Year, int Month)
    {
        var exSrv = new OpenExchangeRateService();
        var exModel = await exSrv.GetExchangeRateItem(Year, Month);
        var sYear = Year.ToString();
        var sMonth = Month.ToString().PadLeft(2, '0');

        var exAverageModel = new OpenAverageExchangeRatesModel
        {
            Country = "POUNDS STERLING",
            Currency = "GBP",
            Value = 1.0,
            Date = sYear + "-" + sMonth + "-01",
            ReportDate = sYear + "-" + sMonth + "-01"
        };

        var curList = new List<OpenAverageExchangeListModel>();

        foreach (var Item in exModel.Rates)
        {
            var GroupItems = Item.Rates.ToList();
            foreach (var aveItem in GroupItems)
            {
                var avModel = new OpenAverageExchangeListModel
                {
                    Currency = aveItem.Name,
                    Value = aveItem.Rate
                };
                curList.Add(avModel);
            }
        }

        var AverageList = curList
            .GroupBy(g => new
            {
                g.Currency
            })
            .Select(s => new
            {
                s.Key.Currency,
                Average = s.Average(a => a.Value)
            }).ToList();


        curList = new List<OpenAverageExchangeListModel>();
        foreach (var AverageItem in AverageList)
        {
            var avModel = new OpenAverageExchangeListModel
            {
                Currency = AverageItem.Currency,
                Value = AverageItem.Average,
                Date = exModel.End_date,
                ReportDate = sYear + "-" + sMonth + "-01"
            };

            var CurrencyModel = await GetCurrenyCode(avModel.Currency);
            if (CurrencyModel.CURRENCY_NAME == null)
                avModel.Country = "Country not on system";
            else
                avModel.Country = CurrencyModel.CURRENCY_NAME;

            curList.Add(avModel);
        }

        exAverageModel.Rates = curList;
        var RepDate = sYear + "-" + sMonth + "-01";
        await DeleteAverageRate(RepDate);
        await AddRate(exAverageModel);

        return exAverageModel;
    }

    public async Task<bool> AddRate(OpenAverageExchangeRatesModel model)
    {
        var db = new DbContext();
        var RatesList = new List<OpenAverageExchangeListDB>();

        foreach (var Item in model.Rates)
        {
            var RateModel = new OpenAverageExchangeListDB
            {
                Value = Item.Value,
                Country = Item.Country,
                Currency = Item.Currency,
                Date = Item.Date,
                ReportDate = Item.ReportDate
            };
            RatesList.Add(RateModel);
        }

        var ExchangeRateModel = new OpenAverageExchangeRatesDB
        {
            Country = model.Country,
            Currency = model.Currency,
            Date = model.Date,
            ReportDate = model.ReportDate,
            Value = model.Value,
            Rates = RatesList
        };

        await db.OpenAverageExchangeRatesDb.InsertOneAsync(ExchangeRateModel);
        return true;
    }

    public async Task<bool> DeleteAverageRate(string ReportDate)
    {
        var db = new DbContext();
        var builder = Builders<OpenAverageExchangeRatesDB>.Filter;
        var filter = builder.Eq("ReportDate", ReportDate);

        await db.OpenAverageExchangeRatesDb.FindOneAndDeleteAsync(filter);
        return true;
    }

    public async Task<CurrencyCodesModel> GetCurrenyCode(string currency)
    {
        var builder = Builders<CurrencyCodesDB>.Filter;

        var filter = builder.Eq("CURRENCY_CODE", currency);

        var db = new DbContext();

        var cursor = await db.CurrencyCodesDb.FindAsync(filter);

        IList<CurrencyCodesDB> results = cursor.ToList();

        if (results.Count == 0) return new CurrencyCodesModel();

        var Item = results[0];

        var model = new CurrencyCodesModel
        {
            _id = Item._id.ToString(),

            CURRENCY_CODE = Item.CURRENCY_CODE,
            CURRENCY_NAME = Item.CURRENCY_NAME.TrimEnd(),
            CURRENCY_SYMBOL = Item.CURRENCY_SYMBOL
        };

        return model;
    }

    public async Task<double> GetRate(string ReportDate, string code)
    {
        var builder = Builders<OpenAverageExchangeRatesDB>.Filter;

        var filter = builder.Eq("ReportDate", ReportDate);

        var db = new DbContext();

        var cursor = await db.OpenAverageExchangeRatesDb.FindAsync(filter);

        IList<OpenAverageExchangeRatesDB> results = cursor.ToList();

        if (results.Count == 0) return 0;

        var Item = results[0];

        var cList = new List<OpenAverageExchangeListModel>();
        foreach (var lItem in Item.Rates)
        {
            var cModel = new OpenAverageExchangeListModel
                { Country = lItem.Country, Currency = lItem.Currency, Value = lItem.Value };
            cList.Add(cModel);
        }

        var result = cList.First(s => s.Currency.Equals(code));


        return result.Value;
    }

    public async Task<double> GetRateAverage(string ReportDate, string code)
    {
        var builder = Builders<OpenAverageExchangeRatesDB>.Filter;
        var subYear = ReportDate.Substring(0, 4);
        var subStart = subYear + "-01-01";
        var subEnd = subYear + "-12-01";

        var filter = builder.Gte("ReportDate", subStart) & builder.Lte("ReportDate", subEnd);

        var db = DbContext.Instance;

        var cursor = await db.OpenAverageExchangeRatesDb.FindAsync(filter);

        IList<OpenAverageExchangeRatesDB> results = cursor.ToList();

        if (results.Count == 0) return 0;

        //var Item = results[0];
        var cList = new List<OpenAverageExchangeListModel>();
        foreach (var Item in results)
        foreach (var lItem in Item.Rates)
        {
            var cModel = new OpenAverageExchangeListModel
                { Country = lItem.Country, Currency = lItem.Currency, Value = lItem.Value };
            cList.Add(cModel);
        }

        var resultList = cList.Where(s => s.Currency.Equals(code)).ToList();

        var result = resultList.Average(s => s.Value);

        return result;
    }
}