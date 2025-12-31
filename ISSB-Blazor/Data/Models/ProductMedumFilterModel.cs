namespace Data.Models;

public class ProductMedumFilterModel
{
    public int Idx { get; set; }
    public int SortOrder { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ProductMedumFilterItemModel> Items { get; set; }
}

public class ProductMedumFilterItemModel
{
    public int Sort { get; set; }
    public string TariffCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}