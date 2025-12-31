/*
    Data/Models/Reporting5Model.cs

    reporting type 5 data item: all grouped source market products
*/

namespace Data.Models;

/// <summary>reporting type 5 data item: all grouped source market products</summary>
public class Reporting5Model : ReportingModel
{
    /// <summary>year</summary>

    public int YEAR { get; set; }

    /// <summary>side of trade</summary>

    public string SOT { get; set; }

    /// <summary>weight</summary>

    public double WEIGHT { get; set; }

    /// <summary>monetary value</summary>

    public double VALUE { get; set; }
}