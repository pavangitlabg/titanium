using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Services;

public class ExchangeRateServiceOld
{
    public async Task<List<ExchangeModel>> GetRates()
    {
        var db = new DbContext();
        var cursor = await db.ExchangeRatesDb.FindAsync(new BsonDocument());

        IList<ExchangeRatesDB> results = cursor.ToList();
        var modelList = new List<ExchangeModel>();

        foreach (var Item in results)
        {
            var cList = new List<ExchangeListModel>();
            foreach (var lItem in Item.Rates)
            {
                var cModel = new ExchangeListModel
                    { Country = lItem.Country, Currency = lItem.Currency, Value = lItem.Value };
                cList.Add(cModel);
            }

            var model = new ExchangeModel
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

    public async Task<ExchangeModel> GetRateByDay(string day)
    {
        ExchangeRateBaseModel exchangeRates = null;
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync("https://api.exchangeratesapi.io/" + day + "?base=GBP");
            var apiResponse = await response.Content.ReadAsStringAsync();
            exchangeRates = JsonConvert.DeserializeObject<ExchangeRateBaseModel>(apiResponse);
        }

        var exList = new List<ExchangeListModel>();

        var eModel = await GetCountry("AUD", exchangeRates.Rates.AUD, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("BGN", exchangeRates.Rates.BGN, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("BRL", exchangeRates.Rates.BRL, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("CAD", exchangeRates.Rates.CAD, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("CHF", exchangeRates.Rates.CHF, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("CNY", exchangeRates.Rates.CNY, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("CZK", exchangeRates.Rates.CZK, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("DKK", exchangeRates.Rates.DKK, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("EUR", exchangeRates.Rates.EUR, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("GBP", exchangeRates.Rates.GBP, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("HKD", exchangeRates.Rates.HKD, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("HRK", exchangeRates.Rates.HRK, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("HUF", exchangeRates.Rates.HUF, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("IDR", exchangeRates.Rates.IDR, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("ILS", exchangeRates.Rates.ILS, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("INR", exchangeRates.Rates.INR, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("ISK", exchangeRates.Rates.ISK, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("JPY", exchangeRates.Rates.JPY, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("KRW", exchangeRates.Rates.KRW, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("MXN", exchangeRates.Rates.MXN, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("MYR", exchangeRates.Rates.MYR, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("NOK", exchangeRates.Rates.NOK, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("NZD", exchangeRates.Rates.NZD, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("PHP", exchangeRates.Rates.PHP, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("PLN", exchangeRates.Rates.PLN, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("RON", exchangeRates.Rates.RON, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("RUB", exchangeRates.Rates.RUB, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("SEK", exchangeRates.Rates.SEK, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("SGD", exchangeRates.Rates.SGD, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("THB", exchangeRates.Rates.THB, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("TRY", exchangeRates.Rates.TRY, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("USD", exchangeRates.Rates.USD, day, exchangeRates.Date);
        exList.Add(eModel);
        eModel = await GetCountry("ZAR", exchangeRates.Rates.ZAR, day, exchangeRates.Date);
        exList.Add(eModel);

        var exModel = new ExchangeModel
        {
            Currency = exchangeRates.@base,
            Country = "POUNDS STERLING",
            Value = 1,
            Date = exchangeRates.Date,
            ReportDate = exchangeRates.ReportDate,
            Rates = exList
        };

        return exModel;
    }

    private async Task<ExchangeListModel> GetCountry(string code, double rate, string date, string reportdate)
    {
        var eModel = new ExchangeListModel();

        eModel.Currency = code;
        var scModel = await GetCurrenyCode(eModel.Currency);
        if (scModel.CURRENCY_CODE == null)
            eModel.Country = "Undefined";
        else
            eModel.Country = scModel.CURRENCY_NAME.Trim().ToUpper();
        eModel.Date = reportdate;
        eModel.ReportDate = date;
        eModel.Value = rate;
        return eModel;
    }


    public async Task<bool> AddRates(List<ExchangeModel> modelList)
    {
        var db = new DbContext();
        var manyDb = new List<ExchangeRatesDB>();
        foreach (var Item in modelList)
            try
            {
                var rateList = new List<ExchangeListDB>();
                foreach (var rateItem in Item.Rates)
                {
                    var m = new ExchangeListDB
                    {
                        Country = rateItem.Country.ToUpper(),
                        Currency = rateItem.Currency,
                        Value = rateItem.Value
                    };
                    rateList.Add(m);
                }


                var modelDB = new ExchangeRatesDB
                {
                    Currency = Item.Currency,
                    Country = Item.Country,
                    Value = Item.Value,
                    Date = Item.Date,
                    Rates = rateList,
                    ReportDate = Item.ReportDate
                };


                manyDb.Add(modelDB);
            }
            catch (Exception e)
            {
                var eSrv = new ErrorService();
                var eModel = new ErrorModel
                    { Code = "500", Class = "ExchangeRateService Line 89", ErrorMessage = e.Message };
                await eSrv.UpdateError(eModel);
            }

        await db.ExchangeRatesDb.InsertManyAsync(manyDb);
        return true;
    }

    public async Task<bool> DeleteCollection()
    {
        var db = new DbContext();
        await db._database.DropCollectionAsync("ExchangeRatesDB");

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
        var builder = Builders<ExchangeRatesDB>.Filter;

        var filter = builder.Eq("ReportDate", ReportDate);

        var db = new DbContext();

        var cursor = await db.ExchangeRatesDb.FindAsync(filter);

        IList<ExchangeRatesDB> results = cursor.ToList();

        if (results.Count == 0) return 0;

        var Item = results[0];

        var cList = new List<ExchangeListModel>();
        foreach (var lItem in Item.Rates)
        {
            var cModel = new ExchangeListModel
                { Country = lItem.Country, Currency = lItem.Currency, Value = lItem.Value };
            cList.Add(cModel);
        }

        var result = cList.First(s => s.Currency.Equals(code));


        return result.Value;
    }
}