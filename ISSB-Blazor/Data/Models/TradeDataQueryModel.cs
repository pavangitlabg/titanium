using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class TradeDataQueryModel
{
    [Display(Name = "Batch Number")] public int BatchNumber { get; set; }
}