using Data.Models;
using Services;

namespace ISSB_Prod_Blazor.Helpers;

public class FileService
{
    private DataImportService _dataImportService;
    public FileService(DataImportService dataImportService)
    {
        _dataImportService = dataImportService;
    }
    public async Task<List<DirModel>> GetFilesFromSystem(string realPath)
    {
        var dirListModel = new List<DirModel>();
        var dr = new DirectoryInfo(realPath);

        var dirRootModel = new DirModel();
        dirRootModel.Idx = 1;
        dirRootModel.Description = "Root";
        dirRootModel.DirectoryPath = dr.FullName;

        // dirRootModel.DirAccessed = dr.LastAccessTime;

        var fileListRootModel = new List<FileModel>();

        var rootfileList = Directory.EnumerateFiles(realPath);
        var Idx = 1;
        foreach (var file in rootfileList)
        {
            var f = new FileInfo(file);

            var fileModel = new FileModel();

            Idx++;
            fileModel.FileName = Path.GetFileName(file);
            fileModel.Sort = Idx;
            fileModel.DirectoryPath = file;
            fileModel.FileAccessed = f.LastAccessTime;
            fileModel.FileSizeText = f.Length < 1024 ? f.Length + " B" : f.Length / 1024 + " KB";
            fileListRootModel.Add(fileModel);
            fileModel.FileName = fileModel.FileName + " [" + fileModel.FileSizeText + "]";
        }

        dirRootModel.Items = fileListRootModel;
        dirListModel.Add(dirRootModel);

        var dirList = Directory.EnumerateDirectories(realPath);
        var dirIdx = 2;
        foreach (var dir in dirList)
        {
            var d = new DirectoryInfo(dir);

            var dirModel = new DirModel();
            dirModel.Idx = dirIdx;
            dirIdx++;
            dirModel.Description = Path.GetFileName(dir);
            dirModel.DirAccessed = d.LastAccessTime;
            dirModel.DirectoryPath = d.FullName;

            var fileListModel = new List<FileModel>();
            Idx = 1;
            var fileList = Directory.EnumerateFiles(dir + "/");
            foreach (var file in fileList)
            {
                var f = new FileInfo(file);

                var fileModel = new FileModel();

                //if (f.Extension.ToLower() != "php" && f.Extension.ToLower() != "aspx"
                //    && f.Extension.ToLower() != "asp")
                //{
                fileModel.Sort = Idx;
                Idx++;
                fileModel.FileName = Path.GetFileName(file);
                fileModel.DirectoryPath = file;
                fileModel.FileAccessed = f.LastAccessTime;
                fileModel.FileSizeText = f.Length < 1024 ? f.Length + " B" : f.Length / 1024 + " KB";
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
        var files = data.Split('|');
        foreach (var dFile in files)
        {
            var fileList = Directory.EnumerateFiles(dFile + "/");
            foreach (var file in fileList) File.Delete(file);

            var mailSrv = new EmailService(_dataImportService);
            var mailModel = await mailSrv.GetEmailCredentials();
            if (!mailModel.FilePath.Equals(dFile)) Directory.Delete(dFile);
            await Task.Delay(500);
        }

        return true;
    }
}