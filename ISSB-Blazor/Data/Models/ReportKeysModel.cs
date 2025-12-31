using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class ReportKeysModel
{
    public string _id { get; set; }

    [Display(Name = "Sort")] public int Sort { get; set; }

    [Display(Name = "Key")] public string Key { get; set; }

    [Display(Name = "Description")] public string Description { get; set; }

    [Display(Name = "Title")] public string Title { get; set; }

    [Display(Name = "Selected")] public bool IsSelected { get; set; }

    [Display(Name = "Width")] public string Width { get; set; }
}