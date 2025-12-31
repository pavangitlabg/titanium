/*
    Data/Models/ReportingTModel.cs

    reporting data item: source country grouping true

    // TODO Oliver ReportingTModel Delete this if it is not needed.
*/

namespace Data.Models
{
    /// <summary>reporting type 0 data item: no grouping in anything</summary>

    public class ReportingTModel : ReportingModel
    {
        public string SC_GEO         { get ; set ; } // TradeDataDB.SC_GEO
        public string SIDE_OF_TRADE  { get ; set ; } // TradeDataDb.SIDE_OF_TRADE
        public int    YEAR           { get ; set ; } // TradeDataDB.YEAR
        public int    MONTH          { get ; set ; } // TradeDataDB.MONTH
        public string H_TARIFF       { get ; set ; } // TradeDataDB.H_TARIFF
        public string MC_GEO         { get ; set ; } // TradeDataDB.MC_GEO
        public double WEIGHT         { get ; set ; } // sum( TradeDataDB.WEIGHT ) / 1000
        public double MONETARY_VALUE { get ; set ; } // sum( TradeDataDB.MONETARY_VALUE )

        public string SOURCE_COUNTRY { get ; set ; } // SourceCountryDB.NAME
//        public string SOT            { get ; set ; } // SIDE_OF_TRADE.LEGEND: use TradeDataDB.SIDE_OF_TRADE
        public string SCODE          { get ; set ; } // TariffDB.WTO_CODE
        public string SC_DESCRIPTION { get ; set ; } // TariffDB.WTO_CODE_LEGEND
        public string TARIFF         { get ; set ; } // TariffDB.HARMONISED_TARIFF_CODE
        public string DESCRIPTION    { get ; set ; } // TariffDB.HARMONISED_TARIFF_SHORT_LEGEND
        public string MARKET_COUNTRY { get ; set ; } // MarketCountryDB.NAME
//        public double VALUE          { get ; set ; } // sum( TradeFactDB.MONETARY_VALUE )

        // public string SOURCE_GEO     { get ; set ; } // SourceCountryDB.GEO_CODE
        // public string GEO            { get ; set ; } // MarketCountryDB.GEO_CODE
    }
}