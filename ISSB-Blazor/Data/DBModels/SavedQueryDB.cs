using MongoDB.Bson;

namespace Data.DBModels;

public class SavedQueryDB
{
    public BsonObjectId _id { get; set; }
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
    public List<SaveQuerySourceCountriesDB> SourceCountryGEOs { get; set; }
    public List<SaveQueryMarketCountriesDB> MarketCountryGEOs { get; set; }
    public List<SaveQueryProductsDB> Products { get; set; }
    public List<SaveQueryDateDB> TimeIDs { get; set; }
    public List<SaveQueryPortDB> Ports { get; set; }
}

public class SaveQuerySourceCountriesDB
{
    public string GeoCode { get; set; }
}

public class SaveQueryMarketCountriesDB
{
    public string GeoCode { get; set; }
}

public class SaveQueryProductsDB
{
    public string Product { get; set; }
}

public class SaveQueryDateDB
{
    public int TimeID { get; set; }
}

public class SaveQueryPortDB
{
    public int PortID { get; set; }
}