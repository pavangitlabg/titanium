using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.DBModels;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;
using System.Reflection;
using System.IO.Compression;

namespace Services
{
    public class DownloadsService
    {
        private static DownloadsService _instance;
        private string LocalDataPath = "/Users/charlesjardine/Projects/ISSB/ISSB/wwwroot/downloads/";

        public static DownloadsService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DownloadsService();
                }
                return _instance;
            }
        }

        public async Task<List<DownloadsModel>> GetDownloads(string UserID)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(UserID));
            var builder = Builders<DownloadsDB>.Filter;
            var filter = builder.Eq("UserId", RecordId);

            DownloadsModel model;
            var db = DBContext.Instance;
                
            var cursor = await db.DownloadsDB.FindAsync(filter);

            IList<DownloadsDB> results = cursor.ToList();
            var modelList = new List<DownloadsModel>();

            foreach (var item in results)
            {
                model = new DownloadsModel
                {
                    _id = item._id.ToString(),
                    UserId = item.UserId.ToString(),
                    ReportId = item.ReportId.ToString(),
                    Date = (DateTime)item.Date,
                    FilePath = item.FilePath,
                    ReportName = item.ReportName

                };

                modelList.Add(model);
            }

            return modelList.OrderByDescending(x => x._id).ToList();
        }

        public async Task<bool> SaveFile(string Id, string UserId, List<CSVHeaderFieldsModel> fields,List<ReportCSVModel> dataList, string ReportName)
        {
           
            
            try
            {
#if RELEASE
                var setSrv = new EmailService();
                var mailModel = await setSrv.GetEmailCredentials();
                LocalDataPath = mailModel.DownloadFilePath;
               // var RemoveFiles = new DirectoryInfo(LocalDataPath).GetFiles(".csv");
#endif


                var lines = new List<string>();
                string line = string.Empty;
                foreach (var header in fields)
                {
                    if(string.IsNullOrEmpty(header.Name))
                        line += header.FieldName + ",";
                    else
                        line += header.Name + ",";
                }

                var nLine = line.TrimEnd(',');
                lines.Add(nLine);

                foreach (var dt in dataList)
                {
                    string fLine = string.Empty;
                    Type t = dt.GetType();
                    PropertyInfo[] props = t.GetProperties();

                    foreach (var f in fields)
                    {
                        var prop = props.FirstOrDefault(s => s.Name == f.FieldName);

                        if(prop != null)
                        {
                            fLine += prop.GetValue(dt) + ",";
                        }
                    }
                    nLine = fLine.TrimEnd(',');
                    lines.Add(nLine);
                }

                //Write To File
                var FileName = LocalDataPath + Id + ".csv";

                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

                StreamWriter sw = new StreamWriter(FileName,true);
                foreach(var fItem in lines)
                {
                    sw.WriteLine(fItem);
                }

                sw.Close();

                var SaveToZip = FileName + ".zip";

                if (File.Exists(SaveToZip))
                {
                    File.Delete(SaveToZip);
                   // Thread.Sleep(5000);
                }

                DirectoryInfo di = new DirectoryInfo(LocalDataPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    //for specific file 
                    if (fi.ToString() == FileName)
                    {
                        if (Directory.Exists(LocalDataPath + Id))
                        {
                            var RemoveFiles = new DirectoryInfo(LocalDataPath + Id).GetFiles("*.*");
                            foreach (var RemoveFile in RemoveFiles)
                            {
                                File.Delete(RemoveFile.FullName);
                            }

                            Directory.Delete(LocalDataPath + Id);
                        }

                        Directory.CreateDirectory(LocalDataPath + Id);
                        var fName = Path.GetFileName(FileName);
                        File.Move(FileName, LocalDataPath + Id + "/" + fName);
                        ZipFile.CreateFromDirectory(LocalDataPath + Id, SaveToZip, CompressionLevel.Optimal, true);

                        if (Directory.Exists(LocalDataPath + Id))
                        {
                            var RemoveFiles = new DirectoryInfo(LocalDataPath + Id).GetFiles("*.*");
                            foreach (var RemoveFile in RemoveFiles)
                            {
                                File.Delete(RemoveFile.FullName);
                            }

                            Directory.Delete(LocalDataPath + Id);
                        }
                    }
                }

                var dModel = new DownloadsModel
                {
                     Date = DateTime.Now,
                     FilePath = SaveToZip,
                     ReportId = Id,
                     UserId = UserId,
                     ReportName = ReportName
                };

                
                await Add(dModel);

                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

            }
            catch (Exception e)
            {
                var eSrv = new ErrorService();
                var eModel = new ErrorModel { Code = "989", Class = "DownloadsService Line 118", ErrorMessage = e.Message };
                await eSrv.UpdateError(eModel);

            }
            return true;
        }

        public void Compress(FileInfo fi)
        {
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Prevent compressing hidden and 
                // already compressed files.
                if ((File.GetAttributes(fi.FullName)
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".zip")
                {
                    // Create the compressed file.
                    using (FileStream outFile =
                                File.Create(fi.FullName + ".zip"))
                    {
                        using (GZipStream Compress =
                            new GZipStream(outFile,
                            CompressionMode.Compress))
                        {
                            // Copy the source file into 
                            // the compression stream.
                            inFile.CopyTo(Compress);

                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                fi.Name, fi.Length.ToString(), outFile.Length.ToString());
                        }
                    }
                }
            }
        }

        public void DeleteFile(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        public async Task<bool> Add(DownloadsModel model)
        {
            bool bReturn = false;

            BsonObjectId UserId = new BsonObjectId(new ObjectId(model.UserId));
            BsonObjectId ReportId = new BsonObjectId(new ObjectId(model.ReportId));

            var builder = Builders<DownloadsDB>.Filter;
            var filter = builder.Eq("UserId", UserId) & builder.Eq("ReportId", ReportId);

            var db = DBContext.Instance;


            var modelDB = new DownloadsDB
            {
                UserId = UserId,
                ReportId = ReportId,
                Date = model.Date,
                FilePath = model.FilePath,
                ReportName = model.ReportName

            };

            var existsModel = await DoesExist(model);
            if (string.IsNullOrEmpty(existsModel.FilePath))
            {
                await db.DownloadsDB.InsertOneAsync(modelDB);
            }
            else
            {
                BsonObjectId _id = new BsonObjectId(new ObjectId(existsModel._id));
                modelDB._id = _id;
                await db.DownloadsDB.FindOneAndReplaceAsync(filter, modelDB);
            }
            bReturn = true;

            return bReturn;


        }

        public async Task<DownloadsModel> DoesExist(DownloadsModel model)
        {
            BsonObjectId UserId = new BsonObjectId(new ObjectId(model.UserId));
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model.ReportId));

            var builder = Builders<DownloadsDB>.Filter;
            var filter = builder.Eq("UserId", UserId) & builder.Eq("ReportId", RecordId);

            var db = DBContext.Instance;
            var cursor = await db.DownloadsDB.FindAsync(filter);

            IList<DownloadsDB> results = cursor.ToList();

            if(results.Count == 0)
            {
                return new DownloadsModel { Date = DateTime.Now };
            }

            var Item = results[0];

            var outModel = new DownloadsModel
            {
                _id = Item._id.ToString(),
                UserId = Item.UserId.ToString(),
                ReportId = Item.ReportId.ToString(),
                Date = (DateTime)Item.Date,
                FilePath = Item.FilePath,
                ReportName = Item.ReportName

            };

            return outModel;
        }

        public async Task<DownloadsModel> GetById(string _id)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(_id));

            var builder = Builders<DownloadsDB>.Filter;

            var filter = builder.Eq("_id", RecordId);

            var db = DBContext.Instance;
            var cursor = await db.DownloadsDB.FindAsync(filter);

            IList<DownloadsDB> results = cursor.ToList();

            var Item = results[0];

            var model = new DownloadsModel
            {
                _id = Item._id.ToString(),
                UserId = Item.UserId.ToString(),
                ReportId = Item.ReportId.ToString(),
                Date = (DateTime)Item.Date,
                FilePath = Item.FilePath,
                ReportName = Item.ReportName

            };

            return model;
        }

        public async Task<bool> Delete(DownloadsModel model)
        {
            BsonObjectId RecordId = new BsonObjectId(new ObjectId(model._id));
            var db = DBContext.Instance;

            var builder = Builders<DownloadsDB>.Filter;
            var filter = builder.Eq("_id", RecordId);

            var returnModel = await db.DownloadsDB.DeleteOneAsync(filter);

            return true;
        }
    }
}
