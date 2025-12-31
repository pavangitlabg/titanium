using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;

namespace Services
{
    public class FileService
    {
        public async Task<List<DirModel>> GetFilesFromSystem(string realPath)
        {
            List<DirModel> dirListModel = new List<DirModel>();
            DirectoryInfo dr = new DirectoryInfo(realPath);

            DirModel dirRootModel = new DirModel();
            dirRootModel.Idx = 1;
            dirRootModel.Description = "Root";
            dirRootModel.DirectoryPath = dr.FullName;

            // dirRootModel.DirAccessed = dr.LastAccessTime;

            var fileListRootModel = new List<FileModel>();

            IEnumerable<string> rootfileList = Directory.EnumerateFiles(realPath);
            int Idx = 1;
            foreach (string file in rootfileList)
            {
                FileInfo f = new FileInfo(file);

                FileModel fileModel = new FileModel();

                Idx++;
                fileModel.FileName = Path.GetFileName(file);
                fileModel.Sort = Idx;
                fileModel.DirectoryPath = file;
                fileModel.FileAccessed = f.LastAccessTime;
                fileModel.FileSizeText = (f.Length < 1024) ? f.Length.ToString() + " B" : f.Length / 1024 + " KB";
                fileListRootModel.Add(fileModel);
                fileModel.FileName = fileModel.FileName + " [" + fileModel.FileSizeText + "]";
                
            }
            dirRootModel.Items = fileListRootModel;
            dirListModel.Add(dirRootModel);

            IEnumerable<string> dirList = Directory.EnumerateDirectories(realPath);
            int dirIdx = 2;
            foreach (string dir in dirList)
            {
                DirectoryInfo d = new DirectoryInfo(dir);

                DirModel dirModel = new DirModel();
                dirModel.Idx = dirIdx;
                dirIdx++;
                dirModel.Description = Path.GetFileName(dir);
                dirModel.DirAccessed = d.LastAccessTime;
                dirModel.DirectoryPath = d.FullName; 

                var fileListModel = new List<FileModel>();
                Idx = 1;
                IEnumerable<string> fileList = Directory.EnumerateFiles(dir + "/");
                foreach (string file in fileList)
                {
                    FileInfo f = new FileInfo(file);

                    FileModel fileModel = new FileModel();

                    //if (f.Extension.ToLower() != "php" && f.Extension.ToLower() != "aspx"
                    //    && f.Extension.ToLower() != "asp")
                    //{
                    fileModel.Sort = Idx;
                    Idx++;
                    fileModel.FileName = Path.GetFileName(file);
                    fileModel.DirectoryPath = file;
                    fileModel.FileAccessed = f.LastAccessTime;
                    fileModel.FileSizeText = (f.Length < 1024) ? f.Length.ToString() + " B" : f.Length / 1024 + " KB";
                    fileModel.FileName = fileModel.FileName + " [" + fileModel.FileSizeText + "]";
                    fileListModel.Add(fileModel);

                    //}
                }
                dirModel.Items = fileListModel;
                dirListModel.Add(dirModel);
            }

            await Task.Delay(3000);

            return dirListModel.OrderBy(x => x.Idx).ToList();
        }

        public async Task<bool> RemoveFiles(string data)
        {
            string[] files = data.Split('|');
            foreach (string dFile in files)
            {
                
                IEnumerable<string> fileList = Directory.EnumerateFiles(dFile + "/");
                foreach (string file in fileList)
                {
                    File.Delete(file);
                }
                
                var mailSrv = new EmailService();
                var mailModel = await mailSrv.GetEmailCredentials();
                if (!mailModel.FilePath.Equals(dFile))
                {
                    Directory.Delete(dFile);
                }
                await Task.Delay(500);
            }
            return true;
        }
      }
}
