using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.DBModels;
using Data.Models;
using Microsoft.Exchange.WebServices.Autodiscover;
using Microsoft.Exchange.WebServices.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class EmailService
    {

        public async System.Threading.Tasks.Task SendEmailTo(string toEmail, string subject, string mailbody)
        {
            var mailSrv = this;
            var mailModel = await mailSrv.GetEmailCredentials();

            string to = toEmail; //To address    
            string from = mailModel.EmailAddress; //From address    
            MailMessage message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = mailbody,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            SmtpClient client = new SmtpClient("smtp.office365.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential(mailModel.EmailAddress, mailModel.EmailPassword);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                var eModel = new ErrorModel
                {
                    Code = "Email",
                    Class = "EmailService Code 50",
                    ErrorMessage = ex.Message
                };
                var errorSrv = new ErrorService();
                await errorSrv.UpdateError(eModel);
                Console.WriteLine(ex.Message);
            }
        }

        public async System.Threading.Tasks.Task SendEmail(string subject, string mailbody)
        {
            var mailSrv = this;
            var mailModel = await mailSrv.GetEmailCredentials();

            string to = "charles.jardine@nathan-software.com"; //To address    
            string from = "charles.jardine@nathan-software.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            message.Subject = subject;
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.office365.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential(mailModel.EmailAddress, mailModel.EmailPassword);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                var eModel = new ErrorModel
                {
                    Code = "Email",
                    Class = "EmailService Code 88",
                    ErrorMessage = ex.Message
                };
                var errorSrv = new ErrorService();
                await errorSrv.UpdateError(eModel);
                Console.WriteLine(ex.Message);
            }
        }
        public async System.Threading.Tasks.Task ScanMail()
        {
            var importSrv = DataImportService.Instance;
            var mailSrv = this;
            var mailModel = await mailSrv.GetEmailCredentials();
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2016);
           

            try
            {
                service.Credentials = new NetworkCredential(mailModel.EmailAddress, mailModel.EmailPassword);
                service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

                // FolderId inboxId = new FolderId(WellKnownFolderName.Inbox, "charles.jardine@nathan-software.com");
                ItemView view = new ItemView(1000);

                view.OrderBy.Add(ItemSchema.DateTimeReceived, Microsoft.Exchange.WebServices.Data.SortDirection.Descending);
                try
                {
                    var emails = await service.FindItems(WellKnownFolderName.Inbox, view);

                    foreach (var message in emails)
                    {
                        var msg = await EmailMessage.Bind(service, message.Id, new PropertySet(BasePropertySet.IdOnly, ItemSchema.Attachments, EmailMessageSchema.IsRead, ItemSchema.Subject, ItemSchema.Body));

                        if (!msg.IsRead)
                        {
                            if (message.HasAttachments)
                            {

                                foreach (var attachment in msg.Attachments)
                                {
                                    if (attachment is FileAttachment)
                                    {
                                        FileAttachment fileAttachment = attachment as FileAttachment;

                                        // Load the file attachment into memory and print out its file name.
                                        await fileAttachment.Load();
                                        var filename = fileAttachment.Name;
                                        bool bZIP;
                                        //bool bCSV, bXLXS, bXLS, bTXT, bZIP, bRAR;
                                        //bCSV = filename.Contains(".csv");
                                        //bXLXS = filename.Contains(".xlxs");
                                        //bXLS = filename.Contains(".xls");
                                        //bTXT = filename.Contains(".txt");
                                        bZIP = filename.Contains(".zip");
                                        // bRAR = filename.Contains(".rar");

                                        // if (bCSV || bXLS || bXLXS || bTXT || bZIP || bRAR)
                                        if (bZIP)
                                        {
                                            var controlSrv = new SystemControlService();
                                            var cntl = await controlSrv.GetSystemControl();

#if RELEASE
                                            string filePath = mailModel.FilePath + cntl.LastFileNumber.ToString() + "-" + fileAttachment.Name;
                                                        
#endif
#if DEBUG
                                            string filePath = "/Users/charlesjardine/Projects/ISSB/ISSB/wwwroot/reports/" + cntl.LastFileNumber.ToString() + " - " + fileAttachment.Name;

#endif
                                            var theStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                            await fileAttachment.Load(theStream);
                                            theStream.Close();
                                            theStream.Dispose();


                                            var importModel = new DataFileImportModel
                                            {
                                                Idx = cntl.LastFileNumber,
                                                Date = DateTime.Now,
                                                FileLocation = mailModel.FilePath,
                                                FileName = fileAttachment.Name,
                                                Source = "EMAIL",
                                                FileStatus = "Imported",
                                                Message = msg.Body,
                                                Subject = msg.Subject

                                            };

                                            await importSrv.SaveImportFile(importModel);

                                            cntl.LastFileNumber++;
                                            await controlSrv.UpdateControl(cntl);
                                        }
                                    }
                                }
                                msg.IsRead = true;
                                await msg.Update(ConflictResolutionMode.AutoResolve);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var eModel = new ErrorModel
                    {
                        Code = "Email",
                        Class = "EmailService Code 187 Unable to get email. Possible password wrong",
                        ErrorMessage = e.Message
                    };
                    var errorSrv = new ErrorService();
                    await errorSrv.UpdateError(eModel);
                }
                
            }
            catch (AutodiscoverRemoteException ex)
            {
                var eModel = new ErrorModel
                {
                    Code = "Email",
                    Class = "EmailService Code 201",
                    ErrorMessage = ex.Message
                };
                var errorSrv = new ErrorService();
                await errorSrv.UpdateError(eModel);
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<EmailCredentialsModel> GetEmailCredentials()
        {

            var db = new DBContext();
            var cursor = await db.EmailCredentialsDB.FindAsync(new BsonDocument());

            IList<EmailCredentialsDB> results = cursor.ToList();

            if (results.Count == 0)
            {
                var modelEmpty = new EmailCredentialsModel
                {
                    EmailAddress = string.Empty,
                    EmailPassword = string.Empty,
                    FilePath = string.Empty,
                    DownloadFilePath = string.Empty
                };
                return modelEmpty;
            }

            var model = new EmailCredentialsModel
            {
                EmailAddress = results[0].EmailAddress,
                EmailPassword = EncryptionHelper.Decrypt(results[0].EmailPassword),
                FilePath = results[0].FilePath,
                DownloadFilePath = results[0].DownloadFilePath
            };

            return model;
        }

        public async Task<EmailCredentialsModel> GetEmailCredentialsByEmail(string email)
        {
            var filter = Builders<EmailCredentialsDB>.Filter.Eq(x => x.EmailAddress, email);
            var db = new DBContext();
            var cursor = await db.EmailCredentialsDB.FindAsync(filter);

            IList<EmailCredentialsDB> results = cursor.ToList();

            var model = new EmailCredentialsModel
            {
                EmailAddress = results[0].EmailAddress,
                EmailPassword = EncryptionHelper.Decrypt(results[0].EmailPassword),
                FilePath = results[0].FilePath
            };

            return model;
        }

        public async Task<bool> SaveEmailCredentials(EmailCredentialsModel model)
        {
            bool bReturn = false;
            var db = new DBContext();

            var cursor = await db.EmailCredentialsDB.FindAsync(new BsonDocument());
            IList<EmailCredentialsDB> results = cursor.ToList();

            if (results.Count > 0)
            {

                var deleteMode = await GetEmailCredentials();
                var filter = Builders<EmailCredentialsDB>.Filter.Eq(x => x.EmailAddress, deleteMode.EmailAddress);

                await db.EmailCredentialsDB.DeleteOneAsync(filter);
            }

            var newModel = new EmailCredentialsDB
            {
                EmailAddress = model.EmailAddress,
                EmailPassword = EncryptionHelper.Encrypt(model.EmailPassword),
                FilePath = model.FilePath,
                DownloadFilePath = model.DownloadFilePath
            };
            await db.EmailCredentialsDB.InsertOneAsync(newModel);

            bReturn = true;

            return bReturn;
        }
    }
}


