using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class DashBoardQueryModel
    {
        public string Id { get; set; }
        public int DashItemType { get; set; }
        public int DashItem { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public string SourceCountryGroup { get; set; }
        public string TradeFlowType { get; set; }
        public string MarketCountryGroup { get; set; }
        public string ProductGroup { get; set; }
        public bool GroupByMonth { get; set; }
        public bool GroupByQuarter { get; set; }
        public bool GroupByYear { get; set; }
        public string ProductGroupType { get; set; }
        public string TonnesValuesGroup { get; set; }
        public string PortGroup { get; set; }
        public bool IncludePorts { get; set; }
        public string ProductRegionCode { get; set; }
        public string Product2DigitCode { get; set; }
        public List<DashBoardQuerySourceCountriesModel> SourceCountryGEOs { get; set; }
        public List<DashBoardQueryMarketCountriesModel> MarketCountryGEOs { get; set; }
        public List<DashBoardQueryProductsModel> Products { get; set; }
        public List<DashBoardSaveQueryDateModel> TimeIDs { get; set; }
        public List<DashBoardQueryPortModel> Ports { get; set; }
    }

    public class DashBoardQuerySourceCountriesModel
    {
        public string GeoCode { get; set; }
    }

    public class DashBoardQueryMarketCountriesModel
    {
        public string GeoCode { get; set; }
    }

    public class DashBoardQueryProductsModel
    {
        public string Product { get; set; }
    }

    public class DashBoardSaveQueryDateModel
    {
        public int TimeID { get; set; }
    }

    public class DashBoardQueryPortModel
    {
        public int PortID { get; set; }
    }

}
