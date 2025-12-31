using MongoDB.Bson;

namespace Data.DBModels;

public class TariffFilterDB
{
    public BsonObjectId _id { get; set; }
    public int Idx { get; set; }
    public int SortOrder { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public bool Active { get; set; }
    public bool CanEdit { get; set; }
    public List<TariffFilterListDB> Items { get; set; }
}

public class TariffFilterListDB
{
    public int SortOrder { get; set; }
    public string TariffCode { get; set; }
    public string Name { get; set; }
}