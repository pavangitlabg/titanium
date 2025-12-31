/*
    Services/ReportModelFactory.cs

    utility to build lists of models

    The utility builds lists of model type ReportOutModel.
*/

using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Data.Models;
using Data.DBModels;
using System;
using System.Drawing;

namespace Services
{
    /// <summary>product detail options</summary>

    public enum ProductDetailOption
    {
        /// <summary>broad product</summary>

        Broad,

        /// <summary>medium level product detail (2 digits)</summary>

        Medium,

        /// <summary>high level product detail</summary>

        High,

        /// <summary>full product detail</summary>

        Long
    }

    /// <summary>utility to build lists of models</summary>

    public class ReportModelFactory
    {
        private List<ReportOutModel> reportList; // model list

        /// <summary>product detail</summary>

        public ProductDetailOption ProductDetail;

        /// <summary>source countries</summary>

        public List<SourceCountryModel> SourceCountries { get; set; }

        /// <summary>market countries</summary>

        public List<MarketCountryModel> MarketCountries { get; set; }

        /// <summary>tariffs</summary>

        public List<TariffModel> Tariffs { get; set; }

        /// <summary>ports</summary>

        public List<PortsModel> Ports { get; set; }

        /// <summary>product detail option</summary>

        public string ProductGroupType
        {
            set
            {
                switch (value)
                {
                    case "Broad": ProductDetail = ProductDetailOption.Broad; break;
                    case "Medium": ProductDetail = ProductDetailOption.Medium; break;
                    case "High": ProductDetail = ProductDetailOption.High; break;
                    case "Long": ProductDetail = ProductDetailOption.Long; break;
                }
            }

            get
            {
                switch (ProductDetail)
                {
                    case ProductDetailOption.Broad: return "Broad";
                    case ProductDetailOption.Medium: return "Medium";
                    case ProductDetailOption.High: return "High";
                    case ProductDetailOption.Long: return "Long";
                }

                return string.Empty;
            }
        }

        /// <summary>model list</summary>

        public IQueryable Queryable { get { return reportList.AsQueryable(); } }

        public ReportModelFactory() { reportList = new List<ReportOutModel>(); }

        /// <summary>Add a BSON document to the model list.</summary>
        /// <param name="document">document to add</param>
        /// <param name="criteria">search criteria</param>
        /// <param name="Id">primary key in model list</param>

        public void Add(BsonDocument document, SearchModel criteria, int Id)
        {
           
            ReportTradeDataDB item; // collection item
            ReportTradeDataDB.Id id; // grouping identifier
            SourceCountryModel source; // source country
            MarketCountryModel market; // market country
            TariffModel tariff; // tariff
            PortsModel port; // port


            item = BsonSerializer.Deserialize<ReportTradeDataDB>(document);
            id = BsonSerializer.Deserialize<ReportTradeDataDB.Id>(item._id);


            source = SourceCountries.Where(c => c.GEO_CODE.Equals(id.SC_GEO)).FirstOrDefault();
            market = MarketCountries.Where(c => c.GEO_CODE.Equals(id.MC_GEO)).FirstOrDefault();
            tariff = ProductDetail == ProductDetailOption.Long
                                          ? Tariffs.Where(t => t.TARIFF_ID == id.TARIFF_ID).FirstOrDefault()
                                          : Tariffs.Where(t => t.HARMONISED_TARIFF_CODE.Equals(id.H_TARIFF)).FirstOrDefault();
            port = Ports.Where(p => p.PortID == id.PORT_ID).FirstOrDefault();

            var longTariff = string.Empty;
            if(tariff == null)
            {
                longTariff = string.Empty;
            }
            else
            {
                longTariff = tariff.SOURCE_COUNTRY_TARIFF_CODE;
            }

            var model = new ReportOutModel
            {
                ID = Id,
                TIME_ID = id.TIME_ID,
                SC_GEO = id.SC_GEO,
                SC_NAME = source == null ? null : source.NAME,
                MC_GEO = id.MC_GEO,
                MC_NAME = market == null ? null : market.NAME,
                SOURCE_COUNTRY_TARIFF_CODE = longTariff,
                TARIFF_CODE = ProductDetail == ProductDetailOption.Long ? (tariff == null ? null : tariff.SOURCE_COUNTRY_TARIFF_CODE)
                                                                        : id.H_TARIFF,
                TARIFF_LEGEND = tariff == null ? null
                                               : ProductDetail == ProductDetailOption.Long ? tariff.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND
                                                                                           : tariff.HARMONISED_TARIFF_SHORT_LEGEND,
                SIDE_OF_TRADE = id.SIDE_OF_TRADE.Equals("E") ? "Export" : id.SIDE_OF_TRADE.Equals("I") ? "Import" : id.SIDE_OF_TRADE,
                PORT_ID = id.PORT_ID,
                PORT_NAME = port == null ? null : port.Name,
                WEIGHT = Math.Round(item.WEIGHT / 1000, 0, MidpointRounding.AwayFromZero),
                MONETARY_VALUE = item.MONETARY_VALUE,
                YTD_WEIGHT = Math.Round(item.YTD_WEIGHT / 1000, 0, MidpointRounding.AwayFromZero),
                YTD_MONETARY_VALUE = item.YTD_MONETARY_VALUE,
                YEAR = id.YEAR,
                QUARTER = id.QUARTER,
                MONTH = id.MONTH
            };

            if (!criteria.ShowWeightZero)
            {
                if (model.WEIGHT > 0)
                    reportList.Add(model);
            }
            else
            {
                reportList.Add(model);
            }

        }

        public void AddToPivot(BsonDocument document, SearchPivotArrayModel criteria, int Id)
        {

            ReportTradeDataDB item; // collection item
            ReportTradeDataDB.Id id; // grouping identifier
            SourceCountryModel source; // source country
            MarketCountryModel market; // market country
            TariffModel tariff; // tariff
            PortsModel port; // port


            item = BsonSerializer.Deserialize<ReportTradeDataDB>(document);
            id = BsonSerializer.Deserialize<ReportTradeDataDB.Id>(item._id);


            source = SourceCountries.Where(c => c.GEO_CODE.Equals(id.SC_GEO)).FirstOrDefault();
            market = MarketCountries.Where(c => c.GEO_CODE.Equals(id.MC_GEO)).FirstOrDefault();
            tariff = ProductDetail == ProductDetailOption.Long
                                          ? Tariffs.Where(t => t.TARIFF_ID == id.TARIFF_ID).FirstOrDefault()
                                          : Tariffs.Where(t => t.HARMONISED_TARIFF_CODE.Equals(id.H_TARIFF)).FirstOrDefault();
            port = Ports.Where(p => p.PortID == id.PORT_ID).FirstOrDefault();

            var longTariff = string.Empty;
            if (tariff == null)
            {
                longTariff = string.Empty;
            }
            else
            {
                longTariff = tariff.SOURCE_COUNTRY_TARIFF_CODE;
            }

            var model = new ReportOutModel
            {
                ID = Id,
                TIME_ID = id.TIME_ID,
                SC_GEO = id.SC_GEO,
                SC_NAME = source == null ? null : source.NAME,
                MC_GEO = id.MC_GEO,
                MC_NAME = market == null ? null : market.NAME,
                SOURCE_COUNTRY_TARIFF_CODE = longTariff,
                TARIFF_CODE = ProductDetail == ProductDetailOption.Long ? (tariff == null ? null : tariff.SOURCE_COUNTRY_TARIFF_CODE)
                                                                        : id.H_TARIFF,
                TARIFF_LEGEND = tariff == null ? null
                                               : ProductDetail == ProductDetailOption.Long ? tariff.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND
                                                                                           : tariff.HARMONISED_TARIFF_SHORT_LEGEND,
                SIDE_OF_TRADE = id.SIDE_OF_TRADE.Equals("E") ? "Export" : id.SIDE_OF_TRADE.Equals("I") ? "Import" : id.SIDE_OF_TRADE,
                PORT_ID = id.PORT_ID,
                PORT_NAME = port == null ? null : port.Name,
                WEIGHT = Math.Round(item.WEIGHT / 1000, 0, MidpointRounding.AwayFromZero),
                MONETARY_VALUE = item.MONETARY_VALUE,
                YTD_WEIGHT = Math.Round(item.YTD_WEIGHT / 1000, 0, MidpointRounding.AwayFromZero),
                YTD_MONETARY_VALUE = item.YTD_MONETARY_VALUE,
                YEAR = id.YEAR,
                QUARTER = id.QUARTER,
                MONTH = id.MONTH
            };

            if (!criteria.ShowWeightZero)
            {
                if (model.WEIGHT > 0)
                    reportList.Add(model);
            }
            else
            {
                reportList.Add(model);
            }

        }
    }
}