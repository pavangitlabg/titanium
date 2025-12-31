using System;
using System.Collections.Generic;


namespace Data.Models
{
    public class SavedQueryModel
    {
        public string Id { get; set; }
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
        public bool SaveToVisitorPageIndex { get; set; }
        public bool SaveToDashboard { get; set; }
        public string SelectedCurrency { get; set; }
        public int SelectedSheduleDay { get; set; }
        public bool IsShedule { get; set; }
        public List<SaveQuerySourceCountriesModel> SourceCountryGEOs { get; set; }
        public List<SaveQueryMarketCountriesModel> MarketCountryGEOs { get; set; }
        public List<SaveQueryProductsModel> Products { get; set; }
        public List<SaveQueryDateModel> TimeIDs { get; set; }
        public List<SaveQueryPortModel> Ports { get; set; }
    }

    public class SaveQuerySourceCountriesModel
    {
        public string GeoCode { get; set; }
    }

    public class SaveQueryMarketCountriesModel
    {
        public string GeoCode { get; set; }
    }

    public class SaveQueryProductsModel
    {
        public string Product { get; set; }
    }

    public class SaveQueryDateModel
    {
        public int TimeID { get; set; }
    }

    public class SaveQueryPortModel
    {
        public int PortID { get; set; }
    }
}
