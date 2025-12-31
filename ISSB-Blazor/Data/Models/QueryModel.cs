namespace Data.Models;

public class QueryModel
{
    public string User { get; set; }
    public List<SourceCountryModel> SourceCountries { get; set; }
    public List<MarketCountryModel> MarketCountries { get; set; }
    public List<SourceCountryFilterModel> SourceRegions { get; set; }
    public List<MarketCountryFilterModel> MarketRegions { get; set; }
    public List<TariffShortModel> LongTariffCodes { get; set; }
    public List<PortsModel> Ports { get; set; }
    public List<QueryDateMonthModel> YearTable { get; set; }
    public List<ProductBroadFilterItemModel> HighTree { get; set; }
    public List<ReportCurrencyModel> CurrencyUsed { get; set; }
    public List<ProductCustomFilterModel> CustomTariff { get; set; }
    public List<SheduleDayModel> SheduleDays { get; set; }
}