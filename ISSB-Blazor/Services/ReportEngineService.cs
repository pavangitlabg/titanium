/*
    Services/ReportEngineService.cs

    reporting engine service
*/

/*
  Uncomment this to perform timing measurements.

  #define MEASURE
*/

using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Services;

/// <summary>reporting engine service</summary>
public class ReportEngineService
{
    /// <summary>Retrieve reporting data from the database.</summary>
    /// <param name="criteria">search criteria</param>
    /// <param name="factory">utility to build the list of models for return</param>
    /// <returns>reporting data</returns>
    public async Task<ReportModelFactory> GetReport(SearchModel criteria)
    {
        IEnumerable<BsonDocument> batch;

        // var json = Newtonsoft.Json.JsonConvert.SerializeObject(criteria);


        var factory = new ReportModelFactory(); // factory to return

        // Collect data from collections other than TradeDataDB.

        var sourceCountrySrv = new SourceCountryService();
        var marketCountrySrv = new MarketCountryService();
        var tariffService = new TariffService();
        var portService = new PortsService();

        factory.SourceCountries = await sourceCountrySrv.GetSourceCountries();
        factory.MarketCountries = await marketCountrySrv.GetMarketAllCountries();
        factory.Tariffs = await tariffService.GetTariffs(criteria.Products, criteria.ProductGroupType.Equals("Long"));
        factory.Ports = await portService.GetPorts(criteria.Ports);
        factory.ProductGroupType = criteria.ProductGroupType;

        // Collect data from TradeDataDB.

        var db = DbContext.Instance;

        var collection = db._database.GetCollection<BsonDocument>("TradeDataDB");

        var options = new AggregateOptions { AllowDiskUse = true };

        var pipeline = criteria.pipeline();

        var count = 1; // first row number

#if (MEASURE)
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch() ;

                // with dates first

                criteria.SelectDatesFirst = true ;

                sw.Start() ;

                using ( var cursor = await collection.AggregateAsync( pipeline , options ) )
                    while ( await cursor.MoveNextAsync() ) batch = cursor.Current ;

                sw.Stop() ;

                System.Diagnostics.Debug.Print("Elapsed with dates first = {0}" , sw.Elapsed ) ;

                // without dates first

                sw.Reset() ;

                criteria.SelectDatesFirst = false ;

                sw.Start() ;

                using ( var cursor = await collection.AggregateAsync( pipeline , options ) )
                    while ( await cursor.MoveNextAsync() ) batch = cursor.Current ;

                sw.Stop() ;

                System.Diagnostics.Debug.Print("Elapsed without dates first = {0}" , sw.Elapsed ) ;
#endif

        ReportTradeDataPivotDB itemPivot; // collection item

        using (var cursor = await collection.AggregateAsync(pipeline, options))
        {
            while (await cursor.MoveNextAsync())
            {
                batch = cursor.Current;

                foreach (var document in batch)
                    //if (criteria.IsPivot)
                    //{
                    //    itemPivot = BsonSerializer.Deserialize<ReportTradeDataPivotDB>(document);
                    //    foreach (var Item in itemPivot.PIVOT)
                    //    {
                    //        foreach (var ItemData in Item.data)
                    //        {
                    //            var model = new ReportTradeDataDB
                    //            {
                    //                H_TARIFF = ItemData._id.H_TARIFF ?? string.Empty,
                    //                MC_GEO = ItemData._id.MC_GEO ?? string.Empty,
                    //                MONETARY_VALUE = ItemData._id.MONETARY_VALUE,
                    //                MONTH = ItemData._id.MONTH,
                    //                PORT_ID = ItemData._id.PORT_ID,
                    //                QUARTER = ItemData._id.QUARTER,
                    //                SC_GEO = ItemData._id.SC_GEO ?? string.Empty,
                    //                SIDE_OF_TRADE = ItemData._id.SIDE_OF_TRADE ?? string.Empty,
                    //                TARIFF_ID = ItemData._id.TARIFF_ID,
                    //                TIME_ID = ItemData._id.TIME_ID,
                    //                WEIGHT = ItemData._id.WEIGHT,
                    //                YEAR = ItemData._id.YEAR,
                    //                YTD_MONETARY_VALUE = ItemData._id.YTD_MONETARY_VALUE,
                    //                YTD_WEIGHT = ItemData._id.YTD_WEIGHT,
                    //                // _id = ItemData._id.ToString()
                    //            };
                    //            BsonDocument doc = model.ToBsonDocument();
                    //            factory.Add(doc, criteria, count++);
                    //            // bDoc.AddRange(docs);
                    //            //bDoc.Add("H_TARIFF", ItemData._id.H_TARIFF ?? string.Empty);
                    //            //bDoc.Add("MC_GEO", ItemData._id.MC_GEO ?? string.Empty);
                    //            //bDoc.Add("MONETARY_VALUE", ItemData._id.MONETARY_VALUE);
                    //            //bDoc.Add("MONTH", ItemData._id.MONTH);
                    //            //bDoc.Add("PORT_ID", ItemData._id.PORT_ID);
                    //            //bDoc.Add("QUARTER", ItemData._id.QUARTER);
                    //            //bDoc.Add("SC_GEO", ItemData._id.SC_GEO ?? string.Empty);
                    //            //bDoc.Add("SIDE_OF_TRADE", ItemData._id.SIDE_OF_TRADE ?? string.Empty) ;
                    //            //bDoc.Add("TIME_ID", ItemData._id.TIME_ID);
                    //            //bDoc.Add("WEIGHT", ItemData._id.WEIGHT);
                    //            //bDoc.Add("YEAR", ItemData._id.YEAR);
                    //            //bDoc.Add("YTD_MONETARY_VALUE", ItemData._id.YTD_MONETARY_VALUE);
                    //            //bDoc.Add("YTD_WEIGHT", ItemData._id.YTD_WEIGHT);
                    //            // bDoc.Add("_id", string.Empty);
                    //        }
                    //    }
                    //}
                    // else
                    // {
                    factory.Add(document, criteria, count++);
                // }
            }
        }

        return factory;
    }

    public async Task<ReportModelFactory> GetPivotData(SearchModel criteria)
    {
        var pcriteria = new SearchPivotArrayModel
        {
            Comments = criteria.Comments,
            Date = criteria.Date,
            Dates = criteria.Dates,
            Description = criteria.Description,
            GroupByMonth = criteria.GroupByMonth,
            GroupByQuarter = criteria.GroupByQuarter,
            GroupByYear = criteria.GroupByYear,
            Id = criteria.Id,
            IncludePorts = criteria.IncludePorts,
            IsPivot = criteria.IsPivot,
            MarketCountryGeoCodes = criteria.MarketCountryGeoCodes,
            // MarketCountryGroup = criteria.MarketCountryGroup,
            MarketCountryGrouping = criteria.MarketCountryGrouping,
            Name = criteria.Name,
            // PortGroup = criteria.PortGroup,
            PortGrouping = criteria.PortGrouping,
            Ports = criteria.Ports,
            ProductDetail = criteria.ProductDetail,
            // ProductGroup = criteria.ProductGroup,
            ProductGrouping = criteria.ProductGrouping,
            ProductGroupType = criteria.ProductGroupType,
            Products = criteria.Products,
            SelectDatesFirst = criteria.SelectDatesFirst,
            ShowWeightZero = criteria.ShowWeightZero,
            SourceCountryGeoCodes = criteria.SourceCountryGeoCodes,
            // SourceCountryGroup = criteria.SourceCountryGroup,
            SourceCountryGrouping = criteria.SourceCountryGrouping,
            //  TonnesValuesGroup = criteria.TonnesValuesGroup,
            TradeFlow = criteria.TradeFlow,
            // TradeFlowType = criteria.TradeFlowType,
            User = criteria.User,
            Values = criteria.Values
        };


        IEnumerable<BsonDocument> batch;

        var factory = new ReportModelFactory(); // factory to return

        // Collect data from collections other than TradeDataDB.

        var sourceCountrySrv = new SourceCountryService();
        var marketCountrySrv = new MarketCountryService();
        var tariffService = new TariffService();
        var portService = new PortsService();

        factory.SourceCountries = await sourceCountrySrv.GetSourceCountries();
        factory.MarketCountries = await marketCountrySrv.GetMarketAllCountries();
        factory.Tariffs = await tariffService.GetTariffs(criteria.Products, criteria.ProductGroupType.Equals("Long"));
        factory.Ports = await portService.GetPorts(criteria.Ports);
        factory.ProductGroupType = criteria.ProductGroupType;

        // Collect data from TradeDataDB.

        var db = DbContext.Instance;

        var collection = db._database.GetCollection<BsonDocument>("TradeDataDB");

        var options = new AggregateOptions { AllowDiskUse = true };

        var pipeline = criteria.pipeline();

        var count = 1; // first row number

#if (MEASURE)
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch() ;

                // with dates first

                criteria.SelectDatesFirst = true ;

                sw.Start() ;

                using ( var cursor = await collection.AggregateAsync( pipeline , options ) )
                    while ( await cursor.MoveNextAsync() ) batch = cursor.Current ;

                sw.Stop() ;

                System.Diagnostics.Debug.Print("Elapsed with dates first = {0}" , sw.Elapsed ) ;

                // without dates first

                sw.Reset() ;

                criteria.SelectDatesFirst = false ;

                sw.Start() ;

                using ( var cursor = await collection.AggregateAsync( pipeline , options ) )
                    while ( await cursor.MoveNextAsync() ) batch = cursor.Current ;

                sw.Stop() ;

                System.Diagnostics.Debug.Print("Elapsed without dates first = {0}" , sw.Elapsed ) ;
#endif

        ReportTradeDataPivotDB itemPivot; // collection item

        using (var cursor = await collection.AggregateAsync(pipeline, options))
        {
            while (await cursor.MoveNextAsync())
            {
                batch = cursor.Current;

                foreach (var document in batch)
                    if (criteria.IsPivot)
                    {
                        itemPivot = BsonSerializer.Deserialize<ReportTradeDataPivotDB>(document);

                        foreach (var Item in itemPivot.PIVOT)
                        foreach (var ItemData in Item.data)
                        {
                            var model = new ReportTradeDataDB
                            {
                                H_TARIFF = ItemData._id.H_TARIFF ?? string.Empty,
                                MC_GEO = ItemData._id.MC_GEO ?? string.Empty,
                                MONETARY_VALUE = ItemData._id.MONETARY_VALUE,
                                MONTH = ItemData._id.MONTH,
                                PORT_ID = ItemData._id.PORT_ID,
                                QUARTER = ItemData._id.QUARTER,
                                SC_GEO = ItemData._id.SC_GEO ?? string.Empty,
                                SIDE_OF_TRADE = ItemData._id.SIDE_OF_TRADE ?? string.Empty,
                                TARIFF_ID = ItemData._id.TARIFF_ID,
                                TIME_ID = ItemData._id.TIME_ID,
                                WEIGHT = ItemData._id.WEIGHT,
                                YEAR = ItemData._id.YEAR,
                                YTD_MONETARY_VALUE = ItemData._id.YTD_MONETARY_VALUE,
                                YTD_WEIGHT = ItemData._id.YTD_WEIGHT
                                // _id = ItemData._id.ToString()
                            };

                            var doc = model.ToBsonDocument();

                            factory.AddToPivot(doc, pcriteria, count++);
                        }
                    }
                    else
                    {
                        factory.AddToPivot(document, pcriteria, count++);
                    }
            }
        }

        return factory;
    }
}