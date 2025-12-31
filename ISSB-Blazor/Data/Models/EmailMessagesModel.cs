using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class EmailMessagesModel
{
    public string _id { get; set; }
    public int MessageID { get; set; }

    [Display(Name = "Subject")] public string Title { get; set; }

    public string HTML { get; set; }
}