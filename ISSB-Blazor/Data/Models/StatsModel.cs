namespace Data.Models;

public class StatsModel
{
    public double Count { get; set; }
    public string CountAsText { get; set; }

    public double Size { get; set; }
    public double StorageSize { get; set; }
    public string CollectionName { get; set; }
}