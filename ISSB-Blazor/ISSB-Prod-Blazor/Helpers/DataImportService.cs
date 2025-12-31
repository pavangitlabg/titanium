using System.IO.Compression;
using Data;
using Data.DBModels;
using Data.Enums;
using Data.Models;
using ISSB_Prod_Blazor.Hubs;
using MongoDB.Bson;
using MongoDB.Driver;
using Services;

namespace ISSB_Prod_Blazor.Helpers;

public class DataImportService(MessageService messageService)
{
    private readonly string _localDataPath =
        "/Users/charlesjardine/RiderProjects/ISSB-Prod-Blazor/ISSB-Blazor/ISSB-Prod-Blazor/wwwroot/uploads/";

    private readonly SemaphoreSlim _semaphoreReadingZip = new(1, 1);
    private readonly SemaphoreSlim _semaphoreValidation = new(1, 1);
    private int _fileMonth;
    private int _fileYear;

    private async Task SendMessage(HubMessageModel message)
    {
        await messageService.Send(message);
    }
    public async Task ScanDirectory()
    {
        var msg = new HubMessageModel { Message = "Waiting for files ...", Progress = 0, IsVisible = false };
        await SendMessage(msg);
        var mailSrv = new EmailService(this);
        var mailModel = await mailSrv.GetEmailCredentials();
        try
        {
#if RELEASE
            var removeFiles = new DirectoryInfo(mailModel.FilePath).GetFiles("*.*");
#endif
#if DEBUG
            var removeFiles = new DirectoryInfo(_localDataPath).GetFiles("*.*");
#endif
            foreach (var removeFile in removeFiles)
                if (!removeFile.FullName.Contains(".zip") && !removeFile.FullName.Contains(".uploading"))
                    File.Delete(removeFile.FullName);
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel { Code = "989", Class = "DataImportService Line 33", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);
           
            msg = new HubMessageModel { Message = "Error : " + e.Message, Progress = 0, IsVisible = true };
            await SendMessage(msg);
        }
        //Rename uploaded files
#if RELEASE
            var uploadedFiles = new DirectoryInfo(mailModel.FilePath).GetFiles("*.uploading");
#endif
#if DEBUG
        var uploadedFiles = new DirectoryInfo(_localDataPath).GetFiles("*.uploading");
#endif
        foreach (var uploadItem in uploadedFiles)
            try
            {
                var newPath = uploadItem.FullName.Replace(".uploading", ".zip");
                Thread.Sleep(5000);
                File.Move(uploadItem.FullName, newPath);
            }
            catch (Exception e)
            {
                var eSrv = new ErrorService();
                var eModel = new ErrorModel
                    { Code = "989", Class = "DataImportService Line 33", ErrorMessage = e.Message };
                await eSrv.UpdateError(eModel);
              
                msg = new HubMessageModel { Message = "Error : " + e.Message, Progress = 0, IsVisible = true };
                await SendMessage(msg);
            }
#if RELEASE
            var zipFiles = new DirectoryInfo(mailModel.FilePath).GetFiles("*.zip");
#endif
#if DEBUG
        var zipFiles = new DirectoryInfo(_localDataPath).GetFiles("*.zip");
#endif
        var bOk = true;
        foreach (var zipItem in zipFiles)
        {
            await _semaphoreReadingZip.WaitAsync();
            try
            {
                var fileName = Path.GetFileName(zipItem.FullName);
                var fileParts = fileName.Split('-');
                _fileMonth = int.Parse(fileParts[2]);
                _fileYear = int.Parse(fileParts[3].Replace(".zip", ""));
#if RELEASE
                    ZipFile.ExtractToDirectory(zipItem.FullName, mailModel.FilePath);
#endif
#if DEBUG
                ZipFile.ExtractToDirectory(zipItem.FullName, _localDataPath, overwriteFiles: true);
#endif
            }
            catch (Exception e)
            {
                var eSrv = new ErrorService();
                var eModel = new ErrorModel
                    { Code = "989", Class = "DataImportService Line 33", ErrorMessage = e.Message };
                await eSrv.UpdateError(eModel);
                msg = new HubMessageModel { Message = "Error : " + e.Message, Progress = 0, IsVisible = true };
                await SendMessage(msg);
                bOk = false;
            }

            File.Delete(zipItem.FullName);
            if (bOk)
            {
                var fileName = Path.GetFileName(zipItem.FullName);
                msg = new HubMessageModel { Message = "Reading: " + fileName, Progress = 0, IsVisible = true };
                await SendMessage(msg);
                await ReadFile(fileName, mailModel);
            }

            _semaphoreReadingZip.Release();
        }
    }

    private async Task ReadFile(string supplier, EmailCredentialsModel mailModel)
    {
        var sp = supplier.Split('-');

        if (sp.Length != 5)
        {
            var msg = new HubMessageModel { Message = "Invalid File Name", Progress = 0, IsVisible = true };
            await SendMessage(msg);
            return;
        }
        
        var sourcetype = sp[4].Replace(".zip", "");
        var srv = new ClientDataImportService();
        var model = await srv.GetClientBySearchString(sp[1]);
        var TypeModel = new ImportFileTypeModel();
        var TypeSrv = new ImportFileTypeService();
        var TypeList = await TypeSrv.GetFileTypesByKey(model.ImportType);

        TypeModel = TypeList.Find(c => c.NAME.Equals(model.SearchString));
        if (TypeModel == null)
            TypeModel = new ImportFileTypeModel
            {
                NAME = "XXX",
                KEY = "XXX"
            };

        //EUROFER Import
        if (TypeModel.NAME.Equals(model.SearchString) && TypeModel.KEY.Equals("EUROFER"))
        {
            var batchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessEUROFER(mailModel, model, (int)batchNumber);

            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = batchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)batchNumber, "Validating Data", false);
                //Lock
                var vResult = await ProcessValidation((int)batchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)batchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)batchNumber, "Data Import Complete", false);
                //Unlock
            }
            else
            {
                await UpdateBatch((int)batchNumber, "Reading Files Failed, please refer to system Error log", true);
            }
        }

        //Standard GTT Format
        if (TypeModel.NAME.Equals(model.SearchString) && TypeModel.KEY.Equals("GTT"))
        {
            var BatchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessGTT(mailModel, model, (int)BatchNumber,sourcetype);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = BatchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)BatchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)BatchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    //  var bPass = await PostAmendments((int)BatchNumber);
                    // if(bPass)
                    await UpdateBatch((int)BatchNumber, "Data Import Complete", false);
                //  else
                //await UpdateBatch((int)BatchNumber, "Amendments Failed, please refer to Import Error Screen", true);
            }
            else
            {
                await UpdateBatch((int)BatchNumber, "Reading Files Failed, please refer to system Error log", true);
            }
        }

        //Standard Japan Format
        if (TypeModel.NAME.Equals(model.SearchString) && TypeModel.KEY.Equals("JAPAN"))
        {
            var BatchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessJAPAN(mailModel, model, (int)BatchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = BatchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)BatchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)BatchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)BatchNumber, "Data Import Complete", false);
            }
            else
            {
                await UpdateBatch((int)BatchNumber, "Reading Files Failed, please refer to system Error log", true);
            }
        }

        //Standard Ireland Format
        if (TypeModel.NAME.Equals(model.SearchString) && TypeModel.KEY.Equals("INDONESIA"))
        {
            var BatchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessINDONESIA(mailModel, model, (int)BatchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = BatchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)BatchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)BatchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)BatchNumber, "Data Import Complete", false);
            }
            else
            {
                await UpdateBatch((int)BatchNumber, "Reading Files Failed, please refer to system Error log", true);
            }
        }

        if (TypeModel.NAME.Equals(model.SearchString) && TypeModel.KEY.Equals("IRE"))
        {
            var BatchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessIreland(mailModel, model, (int)BatchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = BatchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)BatchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)BatchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)BatchNumber, "Data Import Complete", false);
            }
            else
            {
                await UpdateBatch((int)BatchNumber, "Reading Files Failed, please refer to system Error log", true);
            }
        }


        if (model.SearchString.Equals("UK_Old"))
        {
            var BatchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessUK(mailModel, model, (int)BatchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = BatchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)BatchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)BatchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)BatchNumber, "Data Import Complete", false);
            }
        }

        //if (model.SearchString.Equals("AUSTRALIA"))
        //{

        //    var BatchNumber = await SetBatch(model.SearchString);

        //    var bResult = await ProcessAustralia(mailModel, model, (int)BatchNumber);


        //    if (bResult)
        //    {
        //        //Update BatchNumber
        //        var sysCont = new SystemControlService();
        //        var sysControlModel = await sysCont.GetSystemControl();
        //        sysControlModel.ImportBatch = BatchNumber;
        //        await sysCont.UpdateControl(sysControlModel);

        //        await UpdateBatch((int)BatchNumber, "Validating Austraila Data", false);
        //        var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

        //        if (!vResult)
        //        {
        //            await UpdateBatch((int)BatchNumber, "Validation Austraila Failed, please refer to Import Error Screen", true);
        //        }
        //        else
        //        {
        //            await UpdateBatch((int)BatchNumber, "Data Austraila Import Complete", false);
        //        }
        //    }

        //}

        if (model.SearchString.Equals("CANADA_OLD"))
        {
            var BatchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessCanada(mailModel, model, (int)BatchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = BatchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)BatchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)BatchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)BatchNumber, "Data Import Complete", false);
            }
        }

        if (model.SearchString.Equals("SKOREA"))
        {
            var BatchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessSKorea(mailModel, model, (int)BatchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = BatchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)BatchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)BatchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)BatchNumber, "Data Import Complete", false);
            }
        }

        if (model.SearchString.Equals("BRAZIL_OLD"))
        {
            var BatchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessBrazil(mailModel, model, (int)BatchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = BatchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)BatchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)BatchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)BatchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)BatchNumber, "Data Import Complete", false);
            }
        }


        if (model.SearchString.Equals("CHINA_OLD"))
        {
            var batchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessCHINA(mailModel, model, (int)batchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = batchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)batchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)batchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)batchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)batchNumber, "Data Import Complete", false);
            }
            //Cleanup
        }

        if (model.SearchString.Equals("MALAYSIA_OLD"))
        {
            var batchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessMalaysia(mailModel, model, (int)batchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = batchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)batchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)batchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)batchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)batchNumber, "Data Import Complete", false);
            }
            //Cleanup
        }

        if (model.SearchString.Equals("ARGENTINA_OLD"))
        {
            var batchNumber = await SetBatch(model.SearchString);

            var bResult = await ProcessArgentina(mailModel, model, (int)batchNumber);


            if (bResult)
            {
                //Update BatchNumber
                var sysCont = new SystemControlService();
                var sysControlModel = await sysCont.GetSystemControl();
                sysControlModel.ImportBatch = batchNumber;
                await sysCont.UpdateControl(sysControlModel);

                await UpdateBatch((int)batchNumber, "Validating Data", false);
                var vResult = await ProcessValidation((int)batchNumber, model.RegionCode);

                if (!vResult)
                    await UpdateBatch((int)batchNumber, "Validation Failed, please refer to Import Error Screen", true);
                else
                    await UpdateBatch((int)batchNumber, "Data Import Complete", false);
            }
            //Cleanup
        }
    }

    private async Task<bool> ProcessGTT(EmailCredentialsModel mailModel, ClientDataImportModel model, int BatchNumber,string sourceType)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
#if RELEASE
            var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
#endif
#if DEBUG
        var dataFiles = new DirectoryInfo(_localDataPath).GetFiles(model.FileExtenstion);
#endif
        var StageingList = new List<PendingImportModel>();
        var StageingAmendments = new List<PendingImportModel>();

        var LinesRead = 0;
        try
        {
            var PortsSrv = new PortsService();
            var PortList = new List<PortsModel>();

            var countryLookup = new CountryCrossOverService();
            var mModelList = new List<CountryCrossOverChildModel>();

            if (model.SearchString.Equals("UK"))
                await srv.DeleteAmendments(model.SourceGEO);


            foreach (var dataFile in dataFiles)
            {
                var lines = File.ReadLines(dataFile.FullName);
                //Callback?.Invoke("Reading File : " + dataFile.FullName + " Number of lines : " + lines.Count());
                
                var msgFileName = Path.GetFileName(dataFile.FullName);
                var enumerable = lines as string[] ?? lines.ToArray();
                var lineCount = enumerable.Length;
                var statusMsg = "Reading File : " + msgFileName + " Number of lines : " + lineCount.ToString();
                var msg = new HubMessageModel { Message = statusMsg, Progress = 0, IsVisible = true };
                await SendMessage(msg);
                var bHeader = true;
                var cnt = 0;

                foreach (var line in enumerable)
                {
                    if (model.Header.Equals("X") && bHeader)
                    {
                        bHeader = false;
                    }
                    else
                    {
                        var bMC_GEO = true;
                        var words = line.Split(model.Data[0]);

                        //if (LinesRead == 244)
                        //{

                        //}

                        if (words != null)
                        {
                            var pad = 3;
                            var importModel = new PendingImportModel();
                            importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                            importModel.BATCH_NO = BatchNumber;
                            importModel.YEAR = int.Parse(words[0]);
                            importModel.MONTH = int.Parse(words[1]);
                            importModel.SourceType = sourceType;

                            if (model.SearchString.Equals("NEWZEALAND")
                                || model.SearchString.Equals("NORWAY")
                                || model.SearchString.Equals("AUSTRALIA"))
                                pad = 2;

                            if (model.SearchString.Equals("NORWAY") || model.SearchString.Equals("SOUTHAFRICA"))
                            {
                                importModel.SC_GEO = model.SourceGEO;
                                importModel.COO_GEO_CODE = model.SourceGEO;
                            }
                            else
                            {
                                importModel.SC_GEO = words[2].PadLeft(pad, '0');
                                importModel.COO_GEO_CODE = words[2].PadLeft(pad, '0');
                            }

                            if (PortList.Count == 0)
                                PortList = await PortsSrv.GetPortsByGeoCode(importModel.SC_GEO);

                            if (PortList.Count == 1)
                            {
                                importModel.PORT_ID = 0;
                            }
                            else
                            {
                                var PortModel = new PortsModel();
                                if (words.Length == 12)
                                {
                                    PortModel = PortList.Find(x => x.AlphaCode.Equals(words[11]));

                                    if (PortModel == null)
                                        importModel.PORT_ID = 0;
                                    else
                                        importModel.PORT_ID = PortModel.PortID;
                                }
                            }


                            var sot = words[4].Substring(0, 1).ToUpper();
                            if (sot.Equals("I") || sot.Equals("1"))
                                importModel.SIDE_OF_TRADE = "I";
                            else
                                importModel.SIDE_OF_TRADE = "E";

                            //if (words.Length == 12)
                            //{
                            //    if (!int.TryParse(words[11], out int value))
                            //    {
                            //        value = 0;
                            //    }
                            //    else
                            //    {
                            //        importModel.PORT_ID = int.Parse(words[11]);
                            //    }
                            //}

                            importModel.IMPORT_TARIFF = words[5];
                            importModel.H_TARIFF = words[5].Substring(0, 6);

                            importModel.IMP_UNIT = words[9];


                            if (model.CountryMapping)
                            {
                                if (mModelList.Count == 0)
                                    mModelList = await countryLookup.GetChildCountryList(model.SourceGEO);

                                if (words[6].Equals("...")) words[6] = "958";

                                var MC_GEO = words[6].PadLeft(pad, '0');

                                var mModel = mModelList.Find(x => x.Source_Country_Value.Equals(MC_GEO));
                                if (mModel != null)
                                {
                                    importModel.MC_GEO = mModel.GEO_CODE;
                                    importModel.CWC_GEO_CODE = mModel.GEO_CODE;
                                }
                                else
                                {
                                    importModel.MC_GEO = words[6].PadLeft(pad, '0');
                                    importModel.CWC_GEO_CODE = words[6].PadLeft(pad, '0');
                                    bMC_GEO = false;
                                    //var eSrv = new ErrorService();
                                    //var eModel = new ErrorModel { Code = "MAP", Class = "Error in ProcessGTT", ErrorMessage = "Failed to MAP: " + importModel.MC_GEO + " in with Country Crossover" };
                                    //await eSrv.UpdateError(eModel);
                                    //Callback?.Invoke("Failed to MAP: " + importModel.MC_GEO +
                                    //  " in with Country Crossover");
                                    var fileName = Path.GetFileName(dataFile.FullName);
                                    msg = new HubMessageModel { Message = "Failed to MAP: " + importModel.MC_GEO + 
                                                                        " in with Country Crossover in file " + fileName, Progress = 0, IsVisible = true };
                                    await SendMessage(msg);
                                }
                            }
                            else
                            {
                                importModel.MC_GEO = words[6].PadLeft(pad, '0');
                                importModel.CWC_GEO_CODE = words[6].PadLeft(pad, '0');
                            }

                            importModel.IMPORT_TARIFF = words[5];

                            var weightTypes = model.WeightType.Split(',');
                            var bHasWeight = false;

                            foreach (var wType in weightTypes)
                            {
                                var sType = wType.ToLower();
                                if (sType.Contains("kg") || sType.Contains("ki") || sType.Contains("s") ||
                                    sType.Contains("1"))
                                    if (words[9].ToLower().Contains("kg") || words[9].ToLower().Contains("ki") ||
                                        words[9].ToLower().Contains("s") || words[9].ToLower().Contains("1"))
                                    {
                                        importModel.WEIGHT = double.Parse(words[8]);
                                        bHasWeight = true;
                                    }

                                if (sType.Contains("tn") || sType.Contains("to"))
                                    if (words[9].ToLower().Contains("tn") || words[9].ToLower().Contains("to"))
                                    {
                                        importModel.WEIGHT = double.Parse(words[8]) * 1000;
                                        bHasWeight = true;
                                    }
                            }

                            importModel.MONETARY_VALUE = double.Parse(words[10]);


                            if (_fileMonth == importModel.MONTH && _fileYear == importModel.YEAR && bHasWeight &&
                                bMC_GEO)
                                StageingList.Add(importModel);
                            else if (model.SearchString.Equals("UK")) StageingAmendments.Add(importModel);
                        }
                    }

                    cnt++;
                    LinesRead++;

                    if (cnt == 1000)
                    {
                        cnt = 0;
                        //var percentComplete = (int)Math.Round(100 * complete / LinesRead);
                        var fileName = Path.GetFileName(dataFile.FullName);
                        msg = new HubMessageModel { Message = "Number of lines Read : " + LinesRead + " " + fileName, Progress = 100, IsVisible = true };
                        await SendMessage(msg);
                    }
                }

                if (StageingList.Count > 0)
                {
                    await srv.AddForValidation(StageingList);
                }
                else
                {
                    //Callback?.Invoke("No Data can be found in this file that can be processed!");
                    msg = new HubMessageModel
                    {
                        Message = "No Data can be found in this file that can be processed!", Progress = 0,
                        IsVisible = true
                    };
                    await SendMessage(msg);
                }

                if (StageingAmendments.Count > 0)
                {
                    //Callback?.Invoke("Updateing Amendments for : " + model.SearchString);
                    var countryCode = StageingAmendments[0].SC_GEO;

                    var scService = new SourceCountryService();
                    var mcService = new MarketCountryService();
                    var tariffService = new TariffService();
                    var RegionCode = "CN";

                    var TariffList = await tariffService.GetLookupTariffsByRegion(RegionCode);
                    var StageingAmendmentsUpdate = new List<PendingImportModel>();
                    var scModel = await scService.GetSourceCountryByGeoCode(countryCode);

                    foreach (var Item in StageingAmendments)
                    {
                        if (scModel.SOURCE_COUNTRY_ID != 0) Item.SOURCE_COUNTRY_ID = scModel.SOURCE_COUNTRY_ID;
                        var mcModel = await mcService.GetMarketCountryByGeoCode(Item.MC_GEO);
                        if (mcModel.MARKET_COUNTRY_ID != 0) Item.MARKET_COUNTRY_ID = mcModel.MARKET_COUNTRY_ID;

                        var tariff = Item.IMPORT_TARIFF.PadRight(11, '0');
                        var tModel = TariffList.FirstOrDefault(s => s.LongTariffCode.Equals(tariff)) ??
                                     new TariffSearchModel { ID = 0 };
                        if (tModel.ID != 0) Item.TARIFF_ID = tModel.ID;
                        if (Item.SOURCE_COUNTRY_ID != 0 &&
                            Item.MARKET_COUNTRY_ID != 0 &&
                            Item.TARIFF_ID != 0)
                            if (Item.WEIGHT != 0 || Item.MONETARY_VALUE != 0)
                                StageingAmendmentsUpdate.Add(Item);
                    }

                    if (StageingAmendmentsUpdate.Count > 0)
                        await srv.AddForAmendments(StageingAmendmentsUpdate);

                    //Now do the Updates here
                }

                ;

                StageingList = new List<PendingImportModel>();
                StageingAmendments = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "Error in ProcessGTT line number " + LinesRead + "", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records [ProcessGTT]" + e.Message);
            bReturn = false;
        }

        return bReturn;
    }

    private async Task<bool> ProcessSKorea(EmailCredentialsModel mailModel, ClientDataImportModel model,
        int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
        var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
        var StageingList = new List<PendingImportModel>();
        try
        {
            var countryLookup = new SourceCountryMappingService();
            foreach (var dataFile in dataFiles)
            {
                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);

                var bHeader = true;
                foreach (var line in lines)
                    if (model.Header.Equals("X") && bHeader)
                    {
                        bHeader = false;
                    }
                    else
                    {
                        var words = line.Split('|');

                        var importModel = new PendingImportModel();
                        importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                        importModel.BATCH_NO = BatchNumber;
                        importModel.SIDE_OF_TRADE = words[0];
                        importModel.YEAR = int.Parse(words[1].Substring(0, 4));
                        importModel.MONTH = int.Parse(words[1].Substring(4, 2));
                        importModel.IMPORT_TARIFF = words[2];
                        var lModel = await countryLookup.GetSourceCountryByGeoCode(words[3]);
                        importModel.MC_GEO = lModel.ISSB_Country_Geo_Code;
                        importModel.CWC_GEO_CODE = lModel.ISSB_Country_Geo_Code;

                        importModel.SC_GEO = "728";
                        importModel.COO_GEO_CODE = "728";
                        importModel.PORT_ALPHA = words[4];
                        importModel.IMP_UNIT = words[7];
                        importModel.WEIGHT = double.Parse(words[6]);
                        importModel.MONETARY_VALUE = double.Parse(words[8]);
                        StageingList.Add(importModel);
                    }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessArgentina(EmailCredentialsModel mailModel, ClientDataImportModel model,
        int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
        var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
        var StageingList = new List<PendingImportModel>();
        try
        {
            var sFlow = "I";
            var countryLookup = new SourceCountryMappingService();
            foreach (var dataFile in dataFiles)
            {
                if (dataFile.FullName.Contains("Export")) sFlow = "E";

                if (dataFile.FullName.Contains("Import")) sFlow = "I";
                var FileParts = dataFile.FullName.Split('-');

                var Year = int.Parse(FileParts[2]);
                var Month = int.Parse(FileParts[3].Substring(0, 2));

                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);

                var bHeader = true;
                foreach (var line in lines)
                    if (model.Header.Equals("X") && bHeader)
                    {
                        bHeader = false;
                    }
                    else
                    {
                        var words = line.Split(';');

                        var importModel = new PendingImportModel();

                        int num1;
                        var res = int.TryParse(words[0], out num1);
                        if (res) importModel.YEAR = int.Parse(words[0]);


                        res = int.TryParse(words[1], out num1);
                        if (res) importModel.MONTH = int.Parse(words[1]);

                        if (Year.Equals(importModel.YEAR) && Month.Equals(importModel.MONTH))
                        {
                            importModel.IMPORT_TARIFF = words[2].Trim();

                            importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                            importModel.BATCH_NO = BatchNumber;
                            importModel.SIDE_OF_TRADE = sFlow;


                            var lModel = await countryLookup.GetSourceCountryByGeoCode(words[3]);
                            importModel.MC_GEO = lModel.ISSB_Country_Geo_Code;
                            importModel.CWC_GEO_CODE = lModel.ISSB_Country_Geo_Code;
                            importModel.PORT_ALPHA = string.Empty;

                            //TODO MC_GEO v SC_GEO depents on Import or Export ?
                            importModel.SC_GEO = "528";
                            importModel.COO_GEO_CODE = "528";

                            importModel.MONETARY_VALUE = double.Parse(words[5].Replace(",", ""));
                            importModel.WEIGHT = double.Parse(words[4].Replace(",", ""));
                            importModel.IMP_UNIT = string.Empty;


                            //TODO What do I do with Qty 2 
                            StageingList.Add(importModel);
                        }
                    }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessMalaysia(EmailCredentialsModel mailModel, ClientDataImportModel model,
        int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
        var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
        var StageingList = new List<PendingImportModel>();
        try
        {
            var sFlow = "I";
            var countryLookup = new SourceCountryMappingService();
            foreach (var dataFile in dataFiles)
            {
                if (dataFile.FullName.Contains("Export")) sFlow = "E";

                if (dataFile.FullName.Contains("Import")) sFlow = "I";
                var FileParts = dataFile.FullName.Split('-');

                var Year = int.Parse(FileParts[2]);
                var Month = int.Parse(FileParts[3].Substring(0, 2));

                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);

                var bHeader = true;
                foreach (var line in lines)
                    if (model.Header.Equals("X") && bHeader)
                    {
                        bHeader = false;
                    }
                    else
                    {
                        var words = line.Split(',');

                        var importModel = new PendingImportModel();

                        int num1;
                        var res = int.TryParse(words[6], out num1);
                        if (res) importModel.YEAR = int.Parse(words[6]);


                        res = int.TryParse(words[7], out num1);
                        if (res) importModel.MONTH = int.Parse(words[7]);

                        if (Year.Equals(importModel.YEAR) && Month.Equals(importModel.MONTH))
                        {
                            importModel.IMPORT_TARIFF = words[5];

                            importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                            importModel.BATCH_NO = BatchNumber;
                            importModel.SIDE_OF_TRADE = sFlow;


                            var lModel = await countryLookup.GetSourceCountryByGeoCode(words[2]);
                            importModel.MC_GEO = lModel.ISSB_Country_Geo_Code;
                            importModel.CWC_GEO_CODE = lModel.ISSB_Country_Geo_Code;
                            importModel.PORT_ALPHA = string.Empty;

                            lModel = await countryLookup.GetSourceCountryByGeoCode(words[0]);
                            importModel.SC_GEO = lModel.ISSB_Country_Geo_Code;
                            importModel.COO_GEO_CODE = lModel.ISSB_Country_Geo_Code;

                            importModel.MONETARY_VALUE = double.Parse(words[8]);
                            importModel.WEIGHT = double.Parse(words[9]);
                            importModel.IMP_UNIT = words[10];


                            //TODO What do I do with Qty 2 
                            StageingList.Add(importModel);
                        }
                    }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessBrazil(EmailCredentialsModel mailModel, ClientDataImportModel model,
        int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
        var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
        var StageingList = new List<PendingImportModel>();
        try
        {
            var sFlow = "I";
            var countryLookup = new SourceCountryMappingService();
            foreach (var dataFile in dataFiles)
            {
                if (dataFile.FullName.Contains("Export")) sFlow = "E";

                if (dataFile.FullName.Contains("Import")) sFlow = "I";

                var fileName = Path.GetFileName(dataFile.FullName);
                var msg = new HubMessageModel { Message = "Reading File : " + fileName, Progress = 0, IsVisible = true };
                await SendMessage(msg);
                var lines = File.ReadLines(dataFile.FullName);

                var bHeader = true;
                foreach (var line in lines)
                    if (model.Header.Equals("X") && bHeader)
                    {
                        bHeader = false;
                    }
                    else
                    {
                        var words = line.Split(',');

                        var importModel = new PendingImportModel();
                        importModel.YEAR = int.Parse(words[0]);
                        importModel.MONTH = int.Parse(words[1]);
                        importModel.IMPORT_TARIFF = words[2];

                        importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                        importModel.BATCH_NO = BatchNumber;
                        importModel.SIDE_OF_TRADE = sFlow;


                        var lModel = await countryLookup.GetSourceCountryByGeoCode(words[4]);
                        importModel.MC_GEO = lModel.ISSB_Country_Geo_Code;
                        importModel.CWC_GEO_CODE = lModel.ISSB_Country_Geo_Code;
                        importModel.PORT_ALPHA = words[5];

                        //TODO Should this flip depending on flow?
                        importModel.SC_GEO = "508";
                        importModel.COO_GEO_CODE = "508";

                        importModel.IMP_UNIT = words[3];
                        importModel.WEIGHT = double.Parse(words[9]);
                        importModel.MONETARY_VALUE = double.Parse(words[10]);
                        StageingList.Add(importModel);
                    }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessCanada(EmailCredentialsModel mailModel, ClientDataImportModel model,
        int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
        var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
        var StageingList = new List<PendingImportModel>();
        try
        {
            var countryLookup = new SourceCountryMappingService();
            foreach (var dataFile in dataFiles)
            {
                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);

                var bHeader = true;
                foreach (var line in lines)
                    if (model.Header.Equals("X") && bHeader)
                    {
                        bHeader = false;
                    }
                    else
                    {
                        var words = line.Split(',');

                        var importModel = new PendingImportModel();
                        importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                        importModel.BATCH_NO = BatchNumber;
                        importModel.MONTH = int.Parse(words[0]);

                        var strYear = DateTime.Now.ToString("yyyy");

                        var strDataYear = strYear.Substring(0, 2) + words[1];
                        importModel.YEAR = int.Parse(strDataYear);
                        importModel.SIDE_OF_TRADE = words[2];

                        var lModel = await countryLookup.GetSourceCountryByGeoCode(words[4]);
                        importModel.SC_GEO = lModel.ISSB_Country_Geo_Code;
                        importModel.COO_GEO_CODE = lModel.ISSB_Country_Geo_Code;

                        lModel = await countryLookup.GetSourceCountryByGeoCode(words[6]);
                        importModel.MC_GEO = lModel.ISSB_Country_Geo_Code;
                        importModel.CWC_GEO_CODE = lModel.ISSB_Country_Geo_Code;

                        importModel.IMPORT_TARIFF = words[5];
                        importModel.IMP_UNIT = words[14];


                        //if (words[14].Equals("KG"))
                        //    importModel.WEIGHT = double.Parse(words[8]);
                        //else
                        //    importModel.WEIGHT = double.Parse(words[8]) * 1000;
                        importModel.WEIGHT = double.Parse(words[8]);
                        importModel.MONETARY_VALUE = double.Parse(words[10]);
                        StageingList.Add(importModel);
                    }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessUK(EmailCredentialsModel mailModel, ClientDataImportModel model, int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
        var PortSrv = new PortsService();

        var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);

        if (dataFiles == null)
        {
            bReturn = false;
            return bReturn;
        }

        var dt = model.Data;
        var data = dt.Split(',');
        var DataSize = data.Count();

        var countryLookup = new SourceCountryMappingService();
        var StageingList = new List<PendingImportModel>();
        try
        {
            foreach (var dataFile in dataFiles)
            {
                //Callback?.Invoke("Reading File : " + dataFile.FullName);

                var lines = File.ReadLines(dataFile.FullName);

                StageingList = new List<PendingImportModel>();

                foreach (var line in lines)
                    try
                    {
                        var DataCnt = 0;
                        var DataPos = 0;

                        var DataValues = new string[DataSize];
                        foreach (var dataValue in data)
                        {
                            var numericPosition = new string(dataValue.Where(char.IsDigit).ToArray());
                            var EndPos = int.Parse(numericPosition);
                            DataValues[DataCnt] = line.Substring(DataPos, EndPos);

                            DataPos += EndPos;
                            DataCnt++;
                        }

                        var UkModel = new PendingImportModel();

                        if (DataValues[1].Equals("1"))
                            UkModel.SIDE_OF_TRADE = "I";
                        else
                            UkModel.SIDE_OF_TRADE = "E";

                        UkModel.IMPORT_TARIFF = DataValues[2];

                        var lModel = await countryLookup.GetSourceCountryByGeoCode(DataValues[3]);
                        UkModel.MC_GEO = lModel.ISSB_Country_Geo_Code;
                        UkModel.CWC_GEO_CODE = lModel.ISSB_Country_Geo_Code;

                        lModel = await countryLookup.GetSourceCountryByGeoCode(DataValues[4]);
                        // UkModel.SC_GEO = "006";
                        UkModel.SC_GEO = lModel.ISSB_Country_Geo_Code;
                        UkModel.COO_GEO_CODE = lModel.ISSB_Country_Geo_Code;


                        UkModel.PORT_ALPHA = DataValues[5].Trim();

                        if (!string.IsNullOrEmpty(UkModel.PORT_ALPHA))
                        {
                            var PortModel = await PortSrv.GetPortByAlphaCode(UkModel.PORT_ALPHA);
                            UkModel.PORT_ID = PortModel.PortID;
                        }

                        UkModel.WEIGHT = double.Parse(DataValues[8]);
                        UkModel.MONETARY_VALUE = double.Parse(DataValues[9]);

                        var strYear = DateTime.Now.ToString("yyyy");

                        var strDataYear = strYear.Substring(0, 2) + DataValues[6];
                        UkModel.YEAR = int.Parse(strDataYear);
                        UkModel.MONTH = int.Parse(DataValues[7]);
                        UkModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                        UkModel.BATCH_NO = BatchNumber;

                        StageingList.Add(UkModel);
                    }
                    catch (Exception e)
                    {
                        var eSrv = new ErrorService();
                        var eModel = new ErrorModel
                            { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
                        await eSrv.UpdateError(eModel);

                        await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
                        // Callback?.Invoke("Failed to Update Records " + e.Message);
                    }

                await srv.AddForValidation(StageingList);
                //StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (IOException e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessINDONESIA(EmailCredentialsModel mailModel, ClientDataImportModel model,
        int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();

#if RELEASE
            var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
#endif
#if DEBUG
        var dataFiles = new DirectoryInfo(_localDataPath).GetFiles(model.FileExtenstion);
#endif
        var cnt = 0;
        var StageingList = new List<PendingImportModel>();
        try
        {
            var countryLookup = new CountryCrossOverService();
            var mModelList = new List<CountryCrossOverChildModel>();

            foreach (var dataFile in dataFiles)
            {
                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);
                cnt = 0;
                var bInsert = true;
                foreach (var line in lines)
                    if (!line.Contains("YEAR"))
                    {
                        bInsert = true;
                        var words = line.Split('\t');
                        if (!string.IsNullOrEmpty(words[0]))
                        {
                            var importModel = new PendingImportModel();
                            importModel.BATCH_NO = BatchNumber;
                            importModel.YEAR = int.Parse(words[0]);
                            importModel.MONTH = int.Parse(words[1]);
                            importModel.H_TARIFF = words[2];

                            importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                            importModel.PORT_ID = 0;

                            if (dataFile.Name.Contains("IMP"))
                                importModel.SIDE_OF_TRADE = "I";
                            else
                                importModel.SIDE_OF_TRADE = "E";

                            importModel.IMPORT_TARIFF = words[2];

                            if (model.CountryMapping)
                            {
                                if (mModelList.Count == 0)
                                    mModelList = await countryLookup.GetChildCountryList(model.SourceGEO);


                                var MC_GEO = words[4].Replace("\"", "").Replace(",", "");
                                ;

                                var mModel = mModelList.Find(x => x.Source_Country_Value.Equals(MC_GEO));
                                if (mModel != null)
                                {
                                    importModel.MC_GEO = mModel.GEO_CODE;
                                    importModel.CWC_GEO_CODE = mModel.GEO_CODE;
                                }
                                else
                                {
                                    importModel.MC_GEO = MC_GEO;
                                    importModel.CWC_GEO_CODE = MC_GEO;
                                    bInsert = false;
                                    //var eSrv = new ErrorService();
                                    //var eModel = new ErrorModel { Code = "MAP", Class = "Error in ProcessGTT", ErrorMessage = "Failed to MAP: " + importModel.MC_GEO + " in with Country Crossover" };
                                    //await eSrv.UpdateError(eModel);
                                    //Callback?.Invoke("Failed to MAP: " + importModel.MC_GEO +
                                    // " in with Country Crossover");
                                }
                            }

                            importModel.SC_GEO = model.SourceGEO;
                            importModel.COO_GEO_CODE = model.SourceGEO;

                            var szWeight = words[5].Replace("\"", "").Replace(",", "");
                            double weight;
                            if (!double.TryParse(szWeight, out weight)) weight = 0;

                            importModel.WEIGHT = weight;

                            var szValue = words[6].Replace("\"", "").Replace(",", "");
                            double value;
                            if (!double.TryParse(szValue, out value)) value = 0;

                            importModel.MONETARY_VALUE = value;
                            cnt++;
                            if (_fileMonth == importModel.MONTH && _fileYear == importModel.YEAR && bInsert)
                                StageingList.Add(importModel);
                        }
                    }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "Error in line [" + cnt + "] ", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessIreland(EmailCredentialsModel mailModel, ClientDataImportModel model,
        int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();

#if RELEASE
            var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
#endif
#if DEBUG
        var dataFiles = new DirectoryInfo(_localDataPath).GetFiles(model.FileExtenstion);
#endif
        var cnt = 0;
        var StageingList = new List<PendingImportModel>();
        try
        {
            var countryLookup = new CountryCrossOverService();
            var mModelList = new List<CountryCrossOverChildModel>();

            foreach (var dataFile in dataFiles)
            {
                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);
                cnt = 0;
                foreach (var line in lines)
                    if (!line.Contains("Year"))
                    {
                        var strValue = line.Replace("\"", "");

                        var words = strValue.Split(model.Data[0]);

                        //if(cnt == 35392)
                        //{
                        //Error in this line
                        //}
                        var importModel = new PendingImportModel();
                        importModel.BATCH_NO = BatchNumber;
                        importModel.YEAR = int.Parse(words[0]);
                        importModel.MONTH = int.Parse(words[1]);

                        importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                        importModel.PORT_ID = 0;

                        if (words[4].Contains("E"))
                            importModel.SIDE_OF_TRADE = "E";
                        else
                            importModel.SIDE_OF_TRADE = "I";

                        importModel.IMPORT_TARIFF = words[5];

                        // var lModel = await countryLookup.GetSourceCountryByGeoCode(line.Substring(16, 3));
                        if (model.CountryMapping)
                        {
                            if (mModelList.Count == 0)
                                mModelList = await countryLookup.GetChildCountryList(model.SourceGEO);


                            var MC_GEO = words[6];

                            var mModel = mModelList.Find(x => x.Source_Country_Value.Equals(MC_GEO));
                            if (mModel != null)
                            {
                                importModel.MC_GEO = mModel.GEO_CODE;
                                importModel.CWC_GEO_CODE = mModel.GEO_CODE;
                            }
                            else
                            {
                                importModel.MC_GEO = MC_GEO;
                                importModel.CWC_GEO_CODE = MC_GEO;

                                var eSrv = new ErrorService();
                                var eModel = new ErrorModel
                                {
                                    Code = "MAP", Class = "Error in ProcessGTT",
                                    ErrorMessage = "Failed to MAP: " + importModel.MC_GEO + " in with Country Crossover"
                                };
                                await eSrv.UpdateError(eModel);
                                // Callback?.Invoke("Failed to MAP: " + importModel.MC_GEO + " in with Country Crossover");
                            }
                        }

                        importModel.SC_GEO = model.SourceGEO;

                        double weight;
                        if (!double.TryParse(words[8], out weight)) weight = 0;

                        importModel.WEIGHT = weight;

                        importModel.MONETARY_VALUE = double.Parse(words[10]);
                        cnt++;
                        if (_fileMonth == importModel.MONTH && _fileYear == importModel.YEAR)
                            StageingList.Add(importModel);
                    }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "Error in line [" + cnt + "] ", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            // Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessJAPAN(EmailCredentialsModel mailModel, ClientDataImportModel model, int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();

#if RELEASE
            var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
#endif
#if DEBUG
        var dataFiles = new DirectoryInfo(_localDataPath).GetFiles(model.FileExtenstion);
#endif

        var StageingList = new List<PendingImportModel>();
        try
        {
            var countryLookup = new CountryCrossOverService();
            var mModelList = new List<CountryCrossOverChildModel>();

            foreach (var dataFile in dataFiles)
            {
                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);

                foreach (var line in lines)
                {
                    var BaseYear = 2000;

                    var importModel = new PendingImportModel();
                    importModel.BATCH_NO = BatchNumber;
                    importModel.YEAR = BaseYear + int.Parse(line.Substring(0, 2));
                    importModel.MONTH = int.Parse(line.Substring(2, 2));

                    importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                    importModel.PORT_ID = 0;

                    if (line.Substring(4, 1).Equals("1"))
                        importModel.SIDE_OF_TRADE = "E";
                    else
                        importModel.SIDE_OF_TRADE = "I";

                    importModel.IMPORT_TARIFF = line.Substring(6, 6);

                    // var lModel = await countryLookup.GetSourceCountryByGeoCode(line.Substring(16, 3));

                    if (model.CountryMapping)
                    {
                        if (mModelList.Count == 0)
                            mModelList = await countryLookup.GetChildCountryList(model.SourceGEO);


                        var MC_GEO = line.Substring(15, 3);

                        var mModel = mModelList.Find(x => x.Source_Country_Value.Equals(MC_GEO));
                        if (mModel != null)
                        {
                            importModel.MC_GEO = mModel.GEO_CODE;
                            importModel.CWC_GEO_CODE = mModel.GEO_CODE;
                        }
                        else
                        {
                            importModel.MC_GEO = MC_GEO;
                            importModel.CWC_GEO_CODE = MC_GEO;

                            var eSrv = new ErrorService();
                            var eModel = new ErrorModel
                            {
                                Code = "MAP", Class = "Error in ProcessGTT",
                                ErrorMessage = "Failed to MAP: " + importModel.MC_GEO + " in with Country Crossover"
                            };
                            await eSrv.UpdateError(eModel);
                            //Callback?.Invoke("Failed to MAP: " + importModel.MC_GEO + " in with Country Crossover");
                        }
                    }

                    importModel.SC_GEO = model.SourceGEO;
                    importModel.WEIGHT = double.Parse(line.Substring(20, 13));

                    importModel.MONETARY_VALUE = double.Parse(line.Substring(33, 10));

                    if (_fileMonth == importModel.MONTH && _fileYear == importModel.YEAR)
                        StageingList.Add(importModel);
                }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessCHINA(EmailCredentialsModel mailModel, ClientDataImportModel model, int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
        var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
        var StageingList = new List<PendingImportModel>();
        try
        {
            var countryLookup = new SourceCountryMappingService();
            foreach (var dataFile in dataFiles)
            {
                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);


                var bHeader = true;
                foreach (var line in lines)
                    if (model.Header.Equals("X") && bHeader)
                    {
                        bHeader = false;
                    }
                    else
                    {
                        var words = line.Split(',');

                        var importModel = new PendingImportModel();
                        importModel.BATCH_NO = BatchNumber;
                        importModel.YEAR = int.Parse(words[0].Substring(0, 4));
                        importModel.MONTH = int.Parse(words[0].Substring(4, 2));

                        importModel.CURRENCY_CODE = model.CurrencyCode.Trim().ToUpper();
                        importModel.PORT_ID = int.Parse(words[4]);
                        importModel.SIDE_OF_TRADE = words[1];
                        var lModel = await countryLookup.GetSourceCountryByGeoCode(words[3]);

                        importModel.MC_GEO = lModel.ISSB_Country_Geo_Code;

                        //TODO Should this be static?
                        importModel.SC_GEO = "720";

                        importModel.IMPORT_TARIFF = words[2];
                        importModel.MONETARY_VALUE = double.Parse(words[8]);

                        //Need to know what col to use and if it's in KG or TONNES
                        if (int.Parse(words[10]).Equals(9))
                            importModel.WEIGHT = double.Parse(words[9]);
                        else
                            importModel.WEIGHT = 0;
                        StageingList.Add(importModel);
                    }

                await srv.AddForValidation(StageingList);
                StageingList = new List<PendingImportModel>();
                File.Delete(dataFile.FullName);
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
        }

        return bReturn;
    }

    private async Task<bool> ProcessEUROFER(EmailCredentialsModel mailModel, ClientDataImportModel model,
        int BatchNumber)
    {
        var bReturn = true;
        var srv = new ValidationEngineService();
#if RELEASE
            var dataFiles = new DirectoryInfo(mailModel.FilePath).GetFiles(model.FileExtenstion);
#endif
#if DEBUG
        var dataFiles = new DirectoryInfo(_localDataPath).GetFiles(model.FileExtenstion);
#endif
        var h = model.Header;

        var header = h.Split(',');
        var HeaderSize = header.Count();
        var HeaderValues = new string[HeaderSize];

        var dt = model.Data;
        var data = dt.Split(',');
        var DataSize = data.Count();

        var tr = model.Trailer;
        var trailer = tr.Split(',');
        var TrailerSize = trailer.Count();
        var TrailerValues = new string[TrailerSize];


        var StageingList = new List<PendingImportModel>();
        try
        {
            //double complete = 0;
            //int percentComplete = (int)Math.Round((100 * complete) / dataFiles.Count());


            var bProsess = true;

            foreach (var dataFile in dataFiles)
            {
                //lock
                //Callback?.Invoke("Reading File : " + dataFile.FullName);
                var lines = File.ReadLines(dataFile.FullName);

                var pos = 0;
                var LineCnt = 0;
                foreach (var line in lines)
                    if (!line.Substring(0, 1).Equals("T"))
                    {
                        if (line.Substring(0, 1).Equals("1"))
                        {
                            var HeaderCnt = 0;
                            pos = 0;
                            LineCnt = 0;
                            HeaderValues = new string[HeaderSize];
                            foreach (var headValue in header)
                            {
                                var numericPosition = new string(headValue.Where(char.IsDigit).ToArray());
                                var EndPos = int.Parse(numericPosition);
                                HeaderValues[HeaderCnt] = line.Substring(pos, EndPos);
                                pos += EndPos;
                                HeaderCnt++;
                            }

                            if (HeaderValues[10].Equals("1"))
                                bProsess = true;
                            else
                                bProsess = false;
                        }

                        if (bProsess)
                        {
                            if (line.Substring(0, 1).Equals("2"))
                            {
                                //Process Data
                                var DataCnt = 0;
                                var DataPos = 0;

                                var DataValues = new string[DataSize];
                                foreach (var dataValue in data)
                                {
                                    var numericPosition = new string(dataValue.Where(char.IsDigit).ToArray());
                                    var EndPos = int.Parse(numericPosition);
                                    DataValues[DataCnt] = line.Substring(DataPos, EndPos);

                                    DataPos += EndPos;
                                    DataCnt++;
                                }

                                var importModel = new PendingImportModel();
                                importModel.BATCH_NO = BatchNumber;
                                importModel.SC_GEO = HeaderValues[1];
                                importModel.MC_GEO = DataValues[2];

                                var bInsert = true;
                                foreach (var value in DataValues[1].Trim())
                                {
                                    bInsert = char.IsDigit(value);

                                    if (!bInsert)
                                    {
                                        LineCnt++;
                                        break;
                                    }
                                }

                                importModel.H_TARIFF = DataValues[1].Trim().Substring(0, 6);
                                importModel.IMPORT_TARIFF = DataValues[1].Trim();
                                importModel.CWC_GEO_CODE = DataValues[2];
                                importModel.COO_GEO_CODE = DataValues[3];
                                if (HeaderValues[3].Equals("1"))
                                {
                                    importModel.SIDE_OF_TRADE = "I";
                                    importModel.MC_GEO = DataValues[3];
                                }
                                else
                                {
                                    importModel.SIDE_OF_TRADE = "E";
                                }

                                importModel.YEAR = int.Parse(HeaderValues[4]);
                                importModel.MONTH = int.Parse(HeaderValues[5]);
                                importModel.WEIGHT = double.Parse(DataValues[4]);
                                importModel.MONETARY_VALUE = double.Parse(DataValues[5]);
                                importModel.CURRENCY_CODE = HeaderValues[8];
                                importModel.PORT_ID = 0;

                                //PORT
                                if (bInsert)
                                    if (_fileMonth == importModel.MONTH && _fileYear == importModel.YEAR)
                                        StageingList.Add(importModel);
                            }

                            if (line.Substring(0, 1).Equals("3"))
                            {
                                //Cleanup in here
                                var TrailerCnt = 0;
                                var TrailerPos = 0;
                                foreach (var trailerValue in trailer)
                                {
                                    var numericPosition = new string(trailerValue.Where(char.IsDigit).ToArray());
                                    var EndPos = int.Parse(numericPosition);
                                    TrailerValues[TrailerCnt] = line.Substring(TrailerPos, EndPos);
                                    TrailerPos += EndPos;
                                    TrailerCnt++;
                                }

                                if (StageingList.Count > 0)
                                    await srv.AddForValidation(StageingList);

                                //if ((StageingList.Count + LineCnt) != int.Parse(TrailerValues[1]))
                                //{
                                //    //Fail
                                //    await UpdateBatch(BatchNumber, "Failed to Update Records, The number of records do not match", true);
                                //    bReturn = false;
                                //    Callback?.Invoke("Failed to Update Records, The number of records do not match : " + dataFile.FullName);
                                //}
                                //else
                                //{
                                //    //Process
                                //    await srv.AddForValidation(StageingList);
                                //}
                                StageingList = new List<PendingImportModel>();
                            }
                        }
                    }

                File.Delete(dataFile.FullName);
                // complete++;
                // percentComplete = (int)Math.Round((100 * complete) / dataFiles.Count());
                // Callback?.Invoke("complete : " + $"{percentComplete:P}");
            }
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 182", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);

            await UpdateBatch(BatchNumber, "Failed to Update Records " + e.Message, false);
            //Callback?.Invoke("Failed to Update Records " + e.Message);
            bReturn = false;
        }

        return bReturn;
    }

    public async Task<List<DataFileImportModel>> GetImportedFiles()
    {
        var db = new DbContext();
        var cursor = await db.DataFileImportDb.FindAsync(new BsonDocument());

        IList<DataFileImportDB> results = cursor.ToList();
        var modelList = new List<DataFileImportModel>();

        foreach (var Item in results)
        {
            var model = new DataFileImportModel
            {
                _id = Item._id.ToString(),
                Date = Item.Date,
                Idx = Item.Idx,
                FileLocation = Item.FileLocation,
                FileName = Item.FileName,
                FileStatus = Item.FileStatus,
                Message = Item.Message,
                Source = Item.Source,
                Subject = Item.Subject
            };

            modelList.Add(model);
        }

        return modelList.OrderByDescending(x => x._id).ToList();
    }


    public async Task<bool> SaveImportFile(DataFileImportModel model)
    {
        var bReturn = false;
        var db = new DbContext();

        var newModel = new DataFileImportDB
        {
            Idx = model.Idx,
            Date = model.Date,
            Subject = model.Subject,
            FileLocation = model.FileLocation,
            FileName = model.FileName,
            FileStatus = model.FileStatus,
            Message = model.Message,
            Source = model.Source
        };
        await db.DataFileImportDb.InsertOneAsync(newModel);

        bReturn = true;

        return bReturn;
    }

    public async Task<bool> ProcessValidation(int BatchNumber, string RegionCode)
    {
        await _semaphoreValidation.WaitAsync();
        var bReturn = true;
        var srv = new ValidationEngineService();
        var eService = new ImportErrorService();
        var scService = new SourceCountryService();
        var mcService = new MarketCountryService();
        var tariffService = new TariffService();

        // var tariffList = await tariffService.GetTariffs();
        // var tariffList = await tariffService.GetTariffsByRegion(RegionCode);
        var TariffList = await tariffService.GetLookupTariffsByRegion(RegionCode);
        //int index = 0;

        var validateList = await srv.ValidateImport(BatchNumber);
        //Callback?.Invoke("Validating Imported Files ... ");

        double complete = 0, counter = 0;
        var percentComplete = (int)Math.Round(100 * complete / validateList.Count());
        
        var msg = new HubMessageModel { Message = "Validating Imported Files ... ", Progress = percentComplete, IsVisible = true };
        await SendMessage(msg);
        counter = 0;

        foreach (var Item in validateList)
        {
            //Convert Currency
            var pad = '0';
            var ReportDate = Item.YEAR + "-" + Item.MONTH.ToString().PadLeft(2, pad) + "-01";
            if (Item.MONETARY_VALUE > 0)
            {
                Item.MONETARY_VALUE = await ConvertCurrency(ReportDate, Item.CURRENCY_CODE, Item.MONETARY_VALUE);
                if (Item.MONETARY_VALUE.Equals(0))
                {
                    await UpdateBatch(BatchNumber, "No currency could be calculated for this record", true);


                    var errorModel = await eService.GetCode(Item.CURRENCY_CODE);

                    if (errorModel.Code.Equals(string.Empty))
                    {
                        errorModel.BatchNumber = BatchNumber;
                        errorModel.Code = Item.CURRENCY_CODE;
                        errorModel.Type = DataTypeEnums.CurrencyCode;
                        errorModel.ErrorMessage =
                            "No currency could be calculated for this record " + Item.CURRENCY_CODE;
                        errorModel.Class = "DataImportService";
                        await eService.AddError(errorModel);
                    }
                    msg = new HubMessageModel { Message = "No currency could be calculated for this record " + Item.CURRENCY_CODE, Progress = percentComplete, IsVisible = true };
                    await SendMessage(msg);

                    bReturn = false;
                }
            }

            if (string.IsNullOrEmpty(Item.SC_GEO)) Item.SC_GEO = "999";

            var scModel = await scService.GetSourceCountryByGeoCode(Item.SC_GEO);
            if (scModel.SOURCE_COUNTRY_ID.Equals(0))
            {
                var scExceptionModel = await scService.GetSourceCountryExceptionByGeoCode(Item.SC_GEO);
                if (scExceptionModel.SOURCE_COUNTRY_ID.Equals(0))
                {
                    await UpdateBatch(BatchNumber, "1 No Source Country for this record 1 " + Item.SC_GEO, true);
                    var errorModel = await eService.GetCode(Item.SC_GEO);

                    if (errorModel.Code.Equals(string.Empty))
                    {
                        errorModel.BatchNumber = BatchNumber;
                        errorModel.Code = Item.SC_GEO;
                        errorModel.Type = DataTypeEnums.SourceCountry;
                        errorModel.ErrorMessage = "2 No Source Country for this record " + Item.SC_GEO;
                        errorModel.Class = "DataImportService";
                        await eService.AddError(errorModel);
                    }

                    bReturn = false;
                    //Callback?.Invoke("3 No Source Country for this record " + Item.SC_GEO);
                    msg = new HubMessageModel { Message = "3 No Source Country for this record " + Item.SC_GEO, Progress = percentComplete, IsVisible = true };
                    await SendMessage(msg);
                }
            }

            Item.SOURCE_COUNTRY_ID = scModel.SOURCE_COUNTRY_ID;

            if (string.IsNullOrEmpty(Item.MC_GEO)) Item.MC_GEO = "999";

            var mcModel = await mcService.GetMarketCountryByGeoCode(Item.MC_GEO);
            if (mcModel.MARKET_COUNTRY_ID.Equals(0))
            {
                var mcExceptionModel = await mcService.GetMarketCountryExceptionByGeoCode(Item.MC_GEO);
                if (mcExceptionModel.MARKET_COUNTRY_ID.Equals(0))
                {
                    await UpdateBatch(BatchNumber, "No Market Country for this record " + Item.MC_GEO, true);
                    var errorModel = await eService.GetCode(Item.MC_GEO);

                    if (errorModel.Code.Equals(string.Empty))
                    {
                        errorModel.BatchNumber = BatchNumber;
                        errorModel.Code = Item.MC_GEO;
                        errorModel.Type = DataTypeEnums.MarketCountry;
                        errorModel.ErrorMessage = "No Market Country for this record " + Item.MC_GEO;
                        errorModel.Class = "DataImportService";
                        await eService.AddError(errorModel);
                    }

                    bReturn = false;
                    //Callback?.Invoke("No Market Country for this record " + Item.MC_GEO);
                    msg = new HubMessageModel { Message = "No Market Country for this record " + Item.MC_GEO, Progress = percentComplete, IsVisible = true };
                    await SendMessage(msg);
                }
            }

            Item.MARKET_COUNTRY_ID = mcModel.MARKET_COUNTRY_ID;

            var tModel = new TariffSearchModel();
            try
            {
                if (RegionCode.Equals("HS"))
                {
                    tModel = TariffList.FirstOrDefault(s =>
                                 s.ShortTariffCode.Equals(Item.H_TARIFF) && s.ReigionCode.Equals(RegionCode.Trim())) ??
                             new TariffSearchModel { ID = 0 };
                }
                else
                {
                    tModel = TariffList.FirstOrDefault(s =>
                        s.LongTariffCode.Equals(Item.IMPORT_TARIFF) && s.SideOfTrade.Equals(Item.SIDE_OF_TRADE) &&
                        s.ReigionCode.Equals(RegionCode.Trim())) ?? new TariffSearchModel { ID = 0 };
                    if (tModel.ID == 0)
                        tModel = TariffList.FirstOrDefault(s =>
                            s.LongTariffCode.Equals(Item.IMPORT_TARIFF) && s.SideOfTrade.Equals("B") &&
                            s.ReigionCode.Equals(RegionCode.Trim())) ?? new TariffSearchModel { ID = 0 };
                }
            }
            catch (Exception e)
            {
                var eSrv = new ErrorService();
                var eModel = new ErrorModel
                    { Code = "988", Class = "DataImportService Line 524", ErrorMessage = e.Message };
                await eSrv.UpdateError(eModel);

                msg = new HubMessageModel { Message = "Failed to Purge Records " + e.Message, Progress = percentComplete, IsVisible = true };
                await SendMessage(msg);
                //Callback?.Invoke("Failed to Purge Records " + e.Message);
            }

            if (tModel.ID.Equals(0))
            {
                //Now Check H_TARIFF
                var hCode = Item.IMPORT_TARIFF.Substring(0, 6); // + "00000";
                

                if (tModel.ID.Equals(0))
                {
                    var tExceptionModel = await tariffService.GetTariffExceptionByCode(Item.IMPORT_TARIFF);
                    if (tExceptionModel.SOURCE_COUNTRY_TARIFF_CODE.Equals(string.Empty))
                    {
                        await UpdateBatch(BatchNumber, "No Tariff Code found for this record " + Item.IMPORT_TARIFF,
                            true);

                        var errorModel = await eService.GetCode(Item.IMPORT_TARIFF);

                        if (errorModel.Code.Equals(string.Empty))
                        {
                            errorModel.BatchNumber = BatchNumber;
                            errorModel.Code = Item.IMPORT_TARIFF;
                            errorModel.Type = DataTypeEnums.TariffCode;
                            errorModel.ErrorMessage = "No Tariff Code found for this record " + Item.IMPORT_TARIFF;
                            errorModel.Class = "DataImportService";
                            await eService.AddError(errorModel);
                        }

                        bReturn = false;
                        //Callback?.Invoke("No Tariff Code found for this record " + Item.IMPORT_TARIFF);
                        msg = new HubMessageModel { Message = "No Tariff Code found for this record " + Item.IMPORT_TARIFF, Progress = percentComplete, IsVisible = true };
                        await SendMessage(msg);
                    }
                }
            }

            if (!tModel.ID.Equals(0))
                Item.TARIFF_ID = tModel.ID;

            var YTD = await srv.CalculateYTD(Item);

            Item.YTD_MONETARY_VALUE = YTD.YTD_MONETARY_VALUE + Item.MONETARY_VALUE;
            Item.YTD_WEIGHT = YTD.YTD_WEIGHT + Item.WEIGHT;

            counter++;
            complete++;
            if (counter.Equals(1000))
            {
                percentComplete = (int)Math.Round(100 * complete / validateList.Count());
                
                msg = new HubMessageModel { Message = "Validating Imported Files ... ", Progress = percentComplete, IsVisible = true };
                await SendMessage(msg);
                counter = 0;
            }
        }

        percentComplete = 100;
         msg = new HubMessageModel { Message = "Validating Imported Files ... ", Progress = percentComplete, IsVisible = true };
        await SendMessage(msg);
        if (!bReturn)
        {
            await srv.DeleteByBatchNumber(BatchNumber);

            var errorModel = await srv.GetHeader(BatchNumber);
            errorModel.STATUS = "Validation Complete with failure!";
            errorModel.FAIL = true;
            errorModel.POSTED = false;
            errorModel.END_DATE = DateTime.Now;
            await srv.UpdateHeader(errorModel);

            //Callback?.Invoke("Validation Complete with failure!");
            percentComplete = 0;
            
             msg = new HubMessageModel { Message = "Validation Complete with failure!",Progress = percentComplete, IsVisible = true };
            await SendMessage(msg);
        }
        else
        {
            await srv.DeleteByBatchNumber(BatchNumber);
            await srv.AddAfterValidation(validateList);
            await PurgeData();

            var errorModel = await srv.GetHeader(BatchNumber);
            errorModel.STATUS = "Validation Complete with success";
            errorModel.FAIL = false;
            errorModel.POSTED = false;
            errorModel.END_DATE = DateTime.Now;
            await srv.UpdateHeader(errorModel);

            //Callback?.Invoke("Validation Complete with success");
            percentComplete = 0;
            
             msg = new HubMessageModel { Message = "Validation Complete with success",Progress = percentComplete, IsVisible = true };
            await SendMessage(msg);
        }

        _semaphoreValidation.Release();
        return bReturn;
    }

    private async Task<bool> PurgeData()
    {
        //Callback?.Invoke("Purging Redundent Codes");

        var srv = new ValidationEngineService();
        try
        {
            await srv.DeleteZeroSourceCountries(0);
            await srv.DeleteZeroMarketCountries(0);
            await srv.DeleteZeroTariffId(0);
        }
        catch (Exception e)
        {
            var eSrv = new ErrorService();
            var eModel = new ErrorModel
                { Code = "988", Class = "DataImportService Line 524", ErrorMessage = e.Message };
            await eSrv.UpdateError(eModel);


            //Callback?.Invoke("Failed to Purge Records " + e.Message);
            var msg = new HubMessageModel { Message = "Failed to Purge Records " + e.Message,Progress = 0, IsVisible = true };
            await SendMessage(msg);
        }

        return true;
    }


    private async Task<double> ConvertCurrency(string ReportDate, string code, double value)
    {
        var srv = new OpenExchangeRateService();
        var rate = await srv.GetRate(ReportDate, code);

        if (rate.Equals(0)) return 0;

        // double cRate = 1 / rate;
        var total = value / rate;
        return total;
    }

    private async Task<long> SetBatch(string searchString)
    {
        var srv = new ValidationEngineService();
        var sysCont = new SystemControlService();
        var sysControlModel = await sysCont.GetSystemControl();
        var batchNumber = sysControlModel.ImportBatch;
        batchNumber++;
        sysControlModel.ImportBatch = batchNumber;

        var headerModel = new PendingImportHeaderModel
        {
            DATE = DateTime.Now,
            BATCH_NO = (int)batchNumber,
            IMPORT_TYPE = searchString,
            STATUS = "Reading Data Files ..",
            FAIL = false
        };

        await srv.AddHeader(headerModel);

        return batchNumber;
    }

    private async Task UpdateBatch(int batchNumber, string message, bool status)
    {
        var srv = new ValidationEngineService();
        var errorModel = await srv.GetHeader(batchNumber);

        errorModel.STATUS = message;
        errorModel.FAIL = status;
        errorModel.END_DATE = DateTime.Now;
        errorModel.POSTED = false;
        await srv.UpdateHeader(errorModel);
    }

    public async Task<bool> Delete(string _id)
    {
        var recordId = new BsonObjectId(new ObjectId(_id));
        var db = new DbContext();

        var builder = Builders<DataFileImportDB>.Filter;
        var filter = builder.Eq("_id", recordId);

        await db.DataFileImportDb.DeleteOneAsync(filter);

        return true;
    }

    public async Task<DataFileImportModel> GetHeaderById(string _id)
    {
        var recordId = new BsonObjectId(new ObjectId(_id));

        var builder = Builders<DataFileImportDB>.Filter;

        var filter = builder.Eq("_id", recordId);

        var db = new DbContext();
        var cursor = await db.DataFileImportDb.FindAsync(filter);

        IList<DataFileImportDB> results = cursor.ToList();

        var Item = results[0];

        var model = new DataFileImportModel
        {
            _id = Item._id.ToString(),
            Date = Item.Date,
            FileLocation = Item.FileLocation,
            FileName = Item.FileName,
            FileStatus = Item.FileStatus,
            Idx = Item.Idx,
            Message = Item.Message,
            Source = Item.Source,
            Subject = Item.Subject
        };

        return model;
    }

    public async Task<DataFileImportDB> Update(DataFileImportModel model)
    {
        var recordId = new BsonObjectId(new ObjectId(model._id));
        var builder = Builders<DataFileImportDB>.Filter;
        var filter = builder.Eq("_id", recordId);
        var db = new DbContext();

        var cursor = await db.DataFileImportDb.FindAsync(filter);
        IList<DataFileImportDB> results = cursor.ToList();

        var Item = results[0];

        var modelDB = new DataFileImportDB
        {
            _id = Item._id,
            Date = model.Date,
            FileLocation = model.FileLocation,
            FileName = model.FileName,
            FileStatus = model.FileStatus,
            Idx = model.Idx,
            Message = model.Message,
            Source = model.Source,
            Subject = model.Subject
        };

        await db.DataFileImportDb.FindOneAndReplaceAsync(filter, modelDB);

        return modelDB;
    }

    public async Task<DataFileImportModel> DoesExitst(string FileName)
    {
        var builder = Builders<DataFileImportDB>.Filter;
        var filter = builder.Eq("FileName", FileName);
        var db = new DbContext();

        var cursor = await db.DataFileImportDb.FindAsync(filter);
        IList<DataFileImportDB> results = cursor.ToList();
        if (results.Count().Equals(0)) return new DataFileImportModel { FileName = string.Empty };


        var Item = results[0];

        var modelDB = new DataFileImportModel
        {
            _id = Item._id.ToString(),
            Date = Item.Date,
            FileLocation = Item.FileLocation,
            FileName = Item.FileName,
            FileStatus = Item.FileStatus,
            Idx = Item.Idx,
            Message = Item.Message,
            Source = Item.Source,
            Subject = Item.Subject
        };

        return modelDB;
    }
}