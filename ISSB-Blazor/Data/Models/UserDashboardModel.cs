using Data.Enums;

namespace Data.Models;

public class UserDashboardModel
{
    public string _id { get; set; }
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public bool bDesignMode { get; set; }
    public string UserIdentityName { get; set; }
    public string UserName { get; set; }
    public string DashBoardName { get; set; }
    public string Description { get; set; }
    public List<UserDashboardItemsModel> Items { get; set; }
}

public class UserDashboardItemsModel
{
    public int ID { get; set; }
    public DashEnums Type { get; set; }
    public string HTML { get; set; }
    public string Title1 { get; set; }
    public string Title2 { get; set; }
    public string Title3 { get; set; }
    public string Title4 { get; set; }
    public string SubTitle1 { get; set; }
    public string SubTitle2 { get; set; }
    public string SubTitle3 { get; set; }
    public string SubTitle4 { get; set; }
    public int SelectedIndex1 { get; set; }
    public int SelectedIndex2 { get; set; }
    public int SelectedIndex3 { get; set; }
    public int SelectedIndex4 { get; set; }
    public DashBoardQueryModel Query1 { get; set; }
    public DashBoardQueryModel Query2 { get; set; }
    public DashBoardQueryModel Query3 { get; set; }
    public DashBoardQueryModel Query4 { get; set; }
}