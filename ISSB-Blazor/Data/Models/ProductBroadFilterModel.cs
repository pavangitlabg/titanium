namespace Data.Models;

public class ProductBroadFilterModel
{
    public int Idx { get; set; }
    public int SortOrder { get; set; }
    public string Name { get; set; }
    public string Deleted { get; set; }

    public List<ProductBroadFilterItemModel> Items { get; set; }
}

public class ProductBroadFilterItemModel
{
    public int Sort { get; set; }
    public string TariffCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

// public class ProductBroadTreeModel
// {
//     public int Id { get; set; }
//     public string FolderName { get; set; }
//     public bool HasSubFolders { get; set; }
//     public List<ProductBroadTreeItemModel> Items { get; set; }
// }

public class BroadItemModel
{
    public int Id { get; set; }
    public string ParentId { get; set; } = null;
    public string TariffCode { get; set; }
    public bool HasSubFolders { get; set; }
    public string FolderName { get; set; }
    public bool Expanded { get; set; }
    
    public bool IsChecked { get; set; }
}