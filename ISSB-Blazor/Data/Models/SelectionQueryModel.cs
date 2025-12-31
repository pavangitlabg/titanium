namespace Data.Models;

public class SelectionQueryModel
{
    public string Id { get; set; }
    public string Selection { get; set; }
    public bool IsSelected { get; set; }
    public List<SaveQuerySourceCountriesModel> SelectedSourceCountries { get; set; }
    public List<SaveQueryMarketCountriesModel> SelectedMarketCountries { get; set; }
    public List<SaveQueryPortModel> SelectedPorts { get; set; }
    public List<SaveQueryProductsModel> SelectedProducts { get; set; }
    public List<QueryDateMonthModel> SelectedYears { get; set; }
    public bool GroupByMonth { get; set; }
    public bool GroupByYear { get; set; }
    public bool GroupByQuarter { get; set; }
    
    public string TonnValueGroup { get; set; }
    public string Currency { get; set; }
    
    public bool IsScheduled { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Comments { get; set; }
    public int RunDay { get; set; }
}