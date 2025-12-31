namespace Data.Models;

public class HubMessageModel
{
    public string Message { get; set; }
    public int Progress { get; init; }
    public bool IsVisible { get; init; }
}