using System;
using System.Collections.Generic;

namespace Data.Models
{

    public class DirModel
    {
        public int Idx { get; set; }
        public string Description { get; set; }
        public string DirectoryPath { get; set; }
        public DateTime DirAccessed { get; set; }
        public List<FileModel> Items { get; set; }
    }

    public class FileModel
    {
        public int Sort { get; set; }
        public string FileName { get; set; }
        public string DirectoryPath { get; set; }
        public string FileSizeText { get; set; }
        public DateTime FileAccessed { get; set; }

    }


}
