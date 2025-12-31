namespace Data.Models;

public class SourceCountryFilterModel
{
    public string _id { get; set; }
    public int SortOrder { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public bool Active { get; set; }
    public bool CanEdit { get; set; }
    public List<SourceCountryFilterListModel> Items { get; set; }
}

public class SourceCountryFilterListModel
{
    public int SortOrder { get; set; }
    public string GeoCode { get; set; }
    public string CountryName { get; set; }
}

public class SourceCountryFilterListRegionModel
{
    public int SortOrder { get; set; }
    public string GeoCode { get; set; }
    public string CountryName { get; set; }
    public string RegionName { get; set; }
}