using Data.Enums;

namespace Data.Models;

public class ImportErrorLogModel
{
    public string _id { get; set; }
    public DataTypeEnums Type { get; set; }
    public int BatchNumber { get; set; }
    public string Code { get; set; }
    public string Class { get; set; }
    public string ErrorMessage { get; set; }
}

public class ImportErrorLogJsonModel
{
    public List<ImportErrorLogModel> data { get; set; }
}