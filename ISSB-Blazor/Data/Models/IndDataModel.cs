namespace Data.Models;

public class IndDataModel
{
    public string _id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public DateTime ReportingPeriod { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string FormNumber { get; set; }
    public string FormLink { get; set; }
    public string TabName { get; set; }
    public List<DataCols> Datas { get; set; }
}

public class DataCols
{
    public int Index { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string ColCode { get; set; }
    public double ColValue { get; set; }
    public string Data { get; set; }
    public string _id { get; set; }
}

public class IndDataOutModel
{
    public string _id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public DateTime ReportingPeriod { get; set; }
    public string FormNumber { get; set; }
    public string FormLink { get; set; }
    public string TabName { get; set; }
    public List<DataCols> Datas { get; set; }
    public List<ProductBroadFilterItemModel> HighTree { get; set; }
}

public class IndDataHeaderModel
{
    public string Name { get; set; }
    public string Number { get; set; }
    public DateTime ReportingPeriod { get; set; }
    public string FormNumber { get; set; }
    public string TabName { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}

public class IndDataOutSelectModel
{
    public string Number { get; set; }
    public string FormNumber { get; set; }
}