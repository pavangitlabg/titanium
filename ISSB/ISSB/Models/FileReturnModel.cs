using System;
namespace ISSB.Models
{
    public class FileReturnModel
    {
        public string FileName { get; set; }
        public byte[] ToWrite { get; set; }
    }
}
