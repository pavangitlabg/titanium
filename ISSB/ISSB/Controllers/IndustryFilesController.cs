using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using ISSB.Models;
using System;
using System.Collections.Generic;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using ExcelDataReader;
using System.Data;
using Data.Enums;
using System.Text;
using Data.Helpers;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Data;
using Data.DBModels;
using System.Security.Claims;
using OfficeOpenXml.Drawing.Slicer.Style;
using SharpCompress.Compressors.Xz;

namespace ISSB.Controllers
{
    public class IndustryFilesController : Controller
    {
        static List<IndDataOutModel> outList;

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = IndustryService.Instance;
            //await srv.GetIndustryDataColsAll();
            //var modelForm = new FileUploadTypesModel
            //{
            //     Sort = 0,
            //      Name = "Form 1",
            //       Description = string.Empty
            //};

            //await srv.AddFormType(modelForm);

            var model = await srv.GetImportedFiles();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> ReportIndex()
        {
            var srv = IndustryService.Instance;

            var model = await srv.GetReports();
            IQueryable data = model.AsQueryable();

            return View(data);
        }


        [Authorize]
        public async Task<IActionResult> SourceIndex()
        {

            var srv = IndustryService.Instance;
            var source = await srv.GetFormTypes();

            IQueryable sourceData = source.AsQueryable();
            return View(sourceData);

        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var srv = IndustryService.Instance;
            var fTypes = await srv.GetFormTypes();

            return View();
        }

        [Authorize]
        public async Task<IActionResult> AddSave(FileUploadTypesModel model)
        {
            var srv = IndustryService.Instance;

            model.Description = model.Number + " - " + model.Description;

            int cnt = await srv.GetFormTypesBySource(model.Number);

            if (cnt == 0)
                await srv.AddFormType(model);

            return RedirectToAction("Index", "IndustryFiles");
        }


        [Authorize]
        public async Task<IActionResult> Upload()
        {
            var srv = IndustryService.Instance;
            var fTypes = await srv.GetFormTypes();

            List<SelectListItem> FileTypeitems = new List<SelectListItem>();

            foreach (var Item in fTypes)
            {
                var sModel = new SelectListItem
                {
                    Text = Item.Description,
                    Value = Item.Number.ToString()
                };
                FileTypeitems.Add(sModel);
            }


            var modelOut = new IndustryFileUploadModel
            {
                Date = DateTime.Now,
                Source = FileTypeitems,
                Subject = string.Empty,
                Message = string.Empty

            };

            return View(modelOut);
        }

        [Authorize]
        public IActionResult ViewChild(string _id)
        {
            return RedirectToAction("OutputHeader", "IndustryFiles", new { @_id = _id });
        }

        [Authorize]
        public async Task<IActionResult> EditChild(string _id)
        {
            var srv = IndustryService.Instance;

            var model = await srv.GetImportedFileById(_id);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> EditSaveChild(IndustryMasterModel model)
        {
            var srv = IndustryService.Instance;
            var saveModel = await srv.GetImportedFileById(model._id);
            saveModel.Subject = model.Subject;

            await srv.UpdateData(saveModel);

            return RedirectToAction("Index", "IndustryFiles");
        }

        [Authorize]
        public IActionResult ViewDataSet(string _id)
        {
            return RedirectToAction("OutputData", "IndustryFiles", new { @_id = _id });
        }

        [Authorize]
        public async Task<IActionResult> OutputHeader(string _id)
        {
            var srv = IndustryService.Instance;

            var modelList = await srv.GetIndustryData(_id);
            IQueryable data = modelList.AsQueryable();
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> OutputData(string _id)
        {
            var srv = IndustryService.Instance;

            var model = await srv.GetIndustryDataCols(_id);
            @ViewData["Message"] = model.TabName;
            IQueryable data = model.Datas.AsQueryable();
            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = IndustryService.Instance;
            await srv.Delete(_id);
            return RedirectToAction("Index", "IndustryFiles");
        }

        [Authorize]
        public async Task<IActionResult> DeleteReport(string _id)
        {
            var srv = IndustryService.Instance;
            await srv.DeleteReport(_id);
            return RedirectToAction("ReportIndex", "IndustryFiles");
        }

        [Authorize]
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> Save(DataFileUploadModel model)
        {
            if (model.File == null)
            {
                var srv = IndustryService.Instance;
                var fTypes = await srv.GetFormTypes();

                List<SelectListItem> FileTypeitems = new List<SelectListItem>();

                foreach (var Item in fTypes)
                {
                    var sModel = new SelectListItem
                    {
                        Text = Item.Description,
                        Value = Item.Number.ToString()
                    };
                    FileTypeitems.Add(sModel);
                }

                var modelOut = new IndustryFileUploadModel
                {
                    Date = DateTime.Now,
                    Source = FileTypeitems,
                    Subject = string.Empty,
                    Message = string.Empty

                };


                ViewData["Message"] = "Please choose a file for uploading.";
                return View("Upload", modelOut);
            }

            if (!model.File.FileName.Contains(".xlsx"))
            {
                var srv = IndustryService.Instance;
                var fTypes = await srv.GetFormTypes();

                List<SelectListItem> FileTypeitems = new List<SelectListItem>();

                foreach (var Item in fTypes)
                {
                    var sModel = new SelectListItem
                    {
                        Text = Item.Description,
                        Value = Item.Number.ToString()
                    };
                    FileTypeitems.Add(sModel);
                }

                var modelOut = new IndustryFileUploadModel
                {
                    Date = DateTime.Now,
                    Source = FileTypeitems,
                    Subject = string.Empty,
                    Message = string.Empty

                };


                ViewData["Message"] = "Only XLSX files can be uploaded";
                return View("Upload", modelOut);
            }

            if (string.IsNullOrEmpty(model.Subject))
            {
                var srv = IndustryService.Instance;
                var fTypes = await srv.GetFormTypes();

                List<SelectListItem> FileTypeitems = new List<SelectListItem>();

                foreach (var Item in fTypes)
                {
                    var sModel = new SelectListItem
                    {
                        Text = Item.Description,
                        Value = Item.Number.ToString()
                    };
                    FileTypeitems.Add(sModel);
                }

                var modelOut = new IndustryFileUploadModel
                {
                    Date = DateTime.Now,
                    Source = FileTypeitems,
                    Subject = string.Empty,
                    Message = string.Empty

                };

                ViewData["Message"] = "Please enter a subject.";
                return View("Upload", modelOut);
            }

            var filePath = string.Empty;

#if RELEASE
           
            if (model.File.FileName.Contains(".xlsx"))
            {
                filePath = "/var/www/industryData/"; 
            }
#endif

#if DEBUG
            if (model.File.FileName.Contains(".xlsx"))
            {
                filePath = "/Users/charlesjardine/Projects/ISSB/ISSB/wwwroot/IndustryFiles/";
            }
#endif

            bool okToProcesses = false;
            if (model.File.FileName.Contains(model.Source))
            {
                okToProcesses = true;
            }
            else
            {
                var srv = IndustryService.Instance;
                var fTypes = await srv.GetFormTypes();

                List<SelectListItem> FileTypeitems = new List<SelectListItem>();

                foreach (var Item in fTypes)
                {
                    var sModel = new SelectListItem
                    {
                        Text = Item.Description,
                        Value = Item.Number.ToString()
                    };
                    FileTypeitems.Add(sModel);
                }

                var modelOut = new IndustryFileUploadModel
                {
                    Date = DateTime.Now,
                    Source = FileTypeitems,
                    Subject = string.Empty,
                    Message = string.Empty

                };
                ViewData["Message"] = "Invalid file name for " + model.Source;
                return View("Upload", modelOut);
            }

            //Tata UK Collection
            if (okToProcesses)
            {
                if (model.File.Length > 0)
                {
                    filePath = filePath + model.File.FileName;

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await model.File.CopyToAsync(stream);
                    }

                    await ProcessData(filePath, model);
                }
            }

            return RedirectToAction("Index", "IndustryFiles");
        }


        private async Task ProcessData(string filePath, DataFileUploadModel uModel)
        {
            var worksheets = getWorksheetNames(filePath);
            var model = new IndDataModel();
            model.Datas = new List<DataCols>();

            var srv = IndustryService.Instance;
            var postModel = new IndustryMasterModel
            {
                dateTime = DateTime.Now,
                FileName = filePath,

                Subject = uModel.Subject,
                DataType = int.Parse(uModel.Source)
            };
            var indexList = new List<int>();

            var _id = await srv.InsertData(postModel);

            foreach (var Item in worksheets)
            {

                var dataRows = getData(Item, filePath);

                try
                {
                    int row = 0;

                    foreach (var dItem in dataRows)
                    {

                        if (row == 0)
                        {
                            model.Name = dItem.ItemArray.GetValue(1).ToString();
                        }
                        if (row == 1)
                        {

                            model.Number = dItem.ItemArray.GetValue(1).ToString();
                        }
                        if (row == 2)
                        {
                            model.ReportingPeriod = DateTime.Parse(dItem.ItemArray.GetValue(1).ToString());
                            model.Month = model.ReportingPeriod.Month;
                            model.Year = model.ReportingPeriod.Year;
                        }
                        if (row == 3)
                        {
                            model.FormNumber = dItem.ItemArray.GetValue(1).ToString();
                        }

                        //Get the Coloum array
                        if (row == 7)
                        {
                            var colList = dItem.ItemArray.ToList();

                            foreach (var cList in colList)
                            {
                                var sClist = cList.ToString();
                                if (!string.IsNullOrEmpty(sClist))
                                {
                                    if (Char.IsDigit(sClist[0]))
                                    {

                                        if (!cList.ToString().Equals("{}"))
                                        {
                                            int num = int.Parse(cList.ToString());
                                            indexList.Add(num);
                                        }

                                    }
                                }
                            }
                        }


                        if (row >= 9)
                        {
                            var dt = dItem.ItemArray.GetValue(0).ToString().Trim();
                            if (!string.IsNullOrEmpty(dt))
                            {
                                int loopCnt = 2;
                                foreach (var idx in indexList)
                                {
                                    var dataModel = new DataCols();
                                    dataModel.Code = dItem.ItemArray.GetValue(0).ToString().TrimEnd();
                                    dataModel.Description = dItem.ItemArray.GetValue(1).ToString().TrimEnd();

                                    dataModel.ColCode = "C" + idx;

                                    if (dItem.ItemArray.GetValue(loopCnt) != null)
                                    {
                                        var c = dItem.ItemArray.GetValue(loopCnt).ToString();
                                        if (!string.IsNullOrEmpty(c))
                                        {
                                            if (c.Contains("-"))
                                            {
                                                c = c.Replace("-", "");
                                            }

                                            if (Char.IsDigit(c[0]))
                                            {
                                                dataModel.ColValue = int.Parse(dItem.ItemArray.GetValue(loopCnt).ToString());
                                                if (dataModel.ColValue != 0)
                                                {
                                                    model.Datas.Add(dataModel);
                                                }
                                            }
                                        }
                                    }
                                    loopCnt++;
                                }

                            }
                        }

                        row++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                //Save to database
                model.FormLink = _id;
                model.TabName = Item;

                await srv.InsertTataData(model);

                model = new IndDataModel();
                model.Datas = new List<DataCols>();
                indexList = new List<int>();

            }

        }


        public IExcelDataReader getExcelReader(string _path)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var reader = ExcelReaderFactory.CreateReader(System.IO.File.OpenRead(_path));


            return reader;
        }

        public IEnumerable<string> getWorksheetNames(string path)
        {
            var reader = getExcelReader(path);
            var workbook = reader.AsDataSet();

            var sheets = from DataTable sheet in workbook.Tables.Cast<DataTable>() select sheet.TableName;


            return sheets;
        }

        public IEnumerable<DataRow> getData(string sheet, string path, bool firstRowIsColumnNames = false)
        {
            var reader = getExcelReader(path);
            reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = firstRowIsColumnNames
                }
            });
            var workSheet = reader.AsDataSet().Tables[sheet];
            var rows = from DataRow a in workSheet.Rows select a;
            return rows;
        }

        [Authorize]
        public async Task<IActionResult> ReportBuilder(string id)
        {
            ViewBag.FormNumbers = JsonConvert.SerializeObject("{}");
            var srv = IndustryService.Instance;
            var model = new IndustrySavedReportsModel();
            int IdType = 0;
            var formsList = new List<IndDataOutSelectModel>();

            var cols = await srv.GetDataColumns();
            var forms = await srv.GetForms();
            forms = forms.OrderBy(x => x.FormNumber).ThenBy(z => z.Number).ToList();
            var formList = new List<IndForms>();

            var cList = new List<IndCols>();
            int idx = 1;
            cols = cols.OrderBy(x => x.Number).ToList();

            foreach (var Item in cols)
            {
                var cModel = new IndCols
                {
                    Index = idx,
                    Name = Item.Name,
                    Number = "C" + Item.Number.ToString()
                };
                cList.Add(cModel);
                idx++;
            }

            idx = 1;
            foreach (var Item in forms)
            {
                if (!string.IsNullOrEmpty(Item.FormNumber))
                {
                    var f = new IndForms
                    {
                        Index = idx,
                        FormNumber = Item.FormNumber,
                        Name = Item.Name,
                        Number = Item.Number
                    };

                    var fMode = formList.FirstOrDefault(x => x.FormNumber.Equals(f.FormNumber) && x.Number.Equals(f.Number));
                    if (fMode == null)
                    {
                        formList.Add(f);
                        idx++;
                    }
                }
            }
            var selectFormsList = new List<IndForms>();
            var selectColsList = new List<IndCols>();
            var selectBroadTarriffList = new List<string>();

            if (id.Length == 1)
            {
                IdType = int.Parse(id);
                ViewBag.CanExecute = "NO";
            }
            else
            {
                ViewBag.CanExecute = "YES";
                model = await srv.GetReport(id);
                IdType = model.ReportType;

                var savedForms = model.Forms;

                foreach (var Item in savedForms)
                {
                    var fMode = formList.OrderBy(z => z.Index).FirstOrDefault(x => x.FormNumber.Equals(Item.Form) && x.Number.Equals(Item.Number));
                    if (fMode != null)
                    {
                        selectFormsList.Add(fMode);
                    }

                }

                ViewBag.FormNumbers = JsonConvert.SerializeObject(selectFormsList);


                var savedCols = model.ReoprtCols;

                foreach (var Item in savedCols)
                {
                    var fMode = cList.OrderBy(z => z.Index).FirstOrDefault(x => x.Number.Equals(Item));
                    if (fMode != null)
                    {
                        selectColsList.Add(fMode);
                    }
                }

                ViewBag.ColsList = JsonConvert.SerializeObject(selectColsList);
            }

            int cnt = 1;
            List<IndTariffCodes> TariffCodes = new List<IndTariffCodes>();
            List<IndustryRowModel> RowCodes = new List<IndustryRowModel>();
            List<ProductBroadFilterItemModel> BroadItemItemAllowed = new List<ProductBroadFilterItemModel>();

            if (IdType == 0)
            {
                ViewBag.ReportType = IdType.ToString();
                ViewBag.RowCodes = JsonConvert.SerializeObject("{}");
                var tsrv = new TariffService();
                var tCodes = await tsrv.GetTariffsByRegion("CN");
                ViewBag.ProductType = model.ProductType;


                foreach (var Item in tCodes)
                {
                    var m = new IndTariffCodes { Index = cnt, Description = Item.SOURCE_COUNTRY_TARIFF_SHORT_LEGEND, TrariffCode = Item.SOURCE_COUNTRY_TARIFF_CODE.Substring(0, 8) };
                    TariffCodes.Add(m);
                    cnt++;
                }


                var srvP = new ProductService();
                var userSrvP = new UserServices();
                var UserIdP = User.FindFirst(ClaimTypes.Sid).Value;
                var userModelP = await userSrvP.GetUsersById(UserIdP);
                var treeMedumModel = await srvP.GetAllMedumProductsForTree();

                foreach (var Item in treeMedumModel)
                {
                    var ItemItem = new List<ProductMedumFilterItemModel>();
                    foreach (var ItemChild in Item.Items)
                    {
                        var foundItem = userModelP.AllowedTariffCodes.FirstOrDefault(x => x.TariffCode == ItemChild.TariffCode);
                        if (foundItem != null)
                        {

                            var pModel = new ProductMedumFilterItemModel
                            {
                                Name = ItemChild.Name,
                                TariffCode = ItemChild.TariffCode,
                                Description = ItemChild.Description,
                                Sort = ItemChild.Sort
                            };
                            ItemItem.Add(pModel);
                        }
                        Item.Items = ItemItem;
                    }
                }

                IQueryable treeMedumData = treeMedumModel.AsQueryable();
                ViewBag.MedumTreeData = treeMedumData;

                var treeBroadModel = await srvP.GetAllBroadProducts();
                IQueryable treeBroadData = treeBroadModel.AsQueryable();
                ViewBag.BroadTreeData = treeBroadData;

                var BroadItemItems = await srvP.GetBroadProducts();


                foreach (var Item in BroadItemItems)
                {
                    var foundItem = userModelP.AllowedTariffCodes.FirstOrDefault(x => x.TariffCode == Item.TariffCode);
                    if (foundItem != null)
                    {
                        var pModel = new ProductBroadFilterItemModel
                        {
                            Name = foundItem.Name,
                            TariffCode = foundItem.TariffCode,
                            Description = foundItem.Name,
                            Sort = foundItem.SortOrder
                        };
                        BroadItemItemAllowed.Add(pModel);
                    }
                }

                if (model.TCodes != null)
                {
                    var savedTarriff = model.TCodes;

                    foreach (var Item in savedTarriff)
                    {
                        var fMode = BroadItemItemAllowed.OrderBy(z => z.TariffCode).FirstOrDefault(x => x.TariffCode.Equals(Item));
                        if (fMode == null)
                        {
                            if (model.ProductType.Equals("Long"))
                            {
                                selectBroadTarriffList.Add(Item);
                            }
                            else
                            {
                                selectBroadTarriffList.Add(Item.Substring(0, 6));
                            }
                        }
                    }

                    ViewBag.MedumSelectTreeData = JsonConvert.SerializeObject(selectBroadTarriffList);
                }
            }

            if (IdType == 1)
            {
                ViewBag.ReportType = IdType.ToString();
                var rCodes = await srv.GetDataRow();
                cnt = 1;
                foreach (var Item in rCodes)
                {
                    var rModel = new IndustryRowModel
                    {
                        Number = Item.Number,
                        Description = Item.Description,
                        Index = cnt++
                    };

                    var fMode = RowCodes.OrderBy(z => z.Number).FirstOrDefault(x => x.Number.Equals(Item));
                    if (fMode == null)
                    {
                        RowCodes.Add(rModel);
                    }
                }

                if (id.Length > 1)
                {
                    var savedTCode = model.TCodes.OrderBy(x => x).ToList();
                    var selectRowCodesList = new List<IndustryRowModel>();

                    foreach (var Item in savedTCode)
                    {
                        var fMode = RowCodes.OrderBy(z => z.Number).FirstOrDefault(x => x.Number.Equals(Item));
                        if (fMode != null)
                        {
                            selectRowCodesList.Add(fMode);
                        }
                    }

                    ViewBag.RowCodes = JsonConvert.SerializeObject(selectRowCodesList);
                }
            }

            var yearSrv = new LookupsService();
            var modelYear = await yearSrv.GetYearsDropdown();
            //IQueryable dataYears = modelYear.AsQueryable();
            //ViewBag.YearTreeData = dataYears;

            var tableYears = new List<QueryDateMonthModel>();
            cnt = -1;
            foreach (var year in modelYear)
            {
                var mYear = new QueryDateMonthModel
                {
                    Id = cnt++,
                    Year = int.Parse(year.Text),
                    January = false,
                    February = false,
                    March = false,
                    April = false,
                    May = false,
                    June = false,
                    July = false,
                    August = false,
                    September = false,
                    October = false,
                    November = false,
                    December = false
                };
                tableYears.Add(mYear);
            }

            var userSrv = new UserServices();
            var UserId = User.FindFirst(ClaimTypes.Sid).Value;
            var userModel = await userSrv.GetUsersById(UserId);

            var AllowedYears = userModel.AllowedYears;
            string[] years = AllowedYears.Split(',');

            var tableAllowedYears = new List<QueryDateMonthModel>();

            foreach (var y in years)
            {
                var yModel = tableYears.FirstOrDefault(s => s.Year.Equals(int.Parse(y)));
                tableAllowedYears.Add(yModel);
            }
            tableYears = tableAllowedYears;

            var idCnt = 0;
            foreach (var Item in tableYears)
            {
                Item.Id = idCnt;
                idCnt++;
            }

            var selectedMonths = model.SourceTime;
            if (selectedMonths != null)
            {
                foreach (var sMonth in selectedMonths)
                {
                    var year = int.Parse(sMonth.TimeID.ToString().Substring(0, 4));
                    var Mth = int.Parse(sMonth.TimeID.ToString().Substring(5, 2));
                    var tblItem = tableYears.Find(x => x.Year == year);

                    if (Mth == 1)
                    {
                        tblItem.January = true;
                    }

                    if (Mth == 2)
                    {
                        tblItem.February = true;
                    }

                    if (Mth == 3)
                    {
                        tblItem.March = true;
                    }

                    if (Mth == 4)
                    {
                        tblItem.April = true;
                    }

                    if (Mth == 5)
                    {
                        tblItem.May = true;
                    }

                    if (Mth == 6)
                    {
                        tblItem.June = true;
                    }

                    if (Mth == 7)
                    {
                        tblItem.July = true;
                    }

                    if (Mth == 8)
                    {
                        tblItem.August = true;
                    }

                    if (Mth == 9)
                    {
                        tblItem.September = true;
                    }

                    if (Mth == 10)
                    {
                        tblItem.October = true;
                    }

                    if (Mth == 11)
                    {
                        tblItem.November = true;
                    }

                    if (Mth == 12)
                    {
                        tblItem.December = true;
                    }
                }
            }

            var reportModel = new IndReportBuilterModel
            {
                Forms = formList,
                Cols = cList,
                TariffCodes = TariffCodes,
                Years = tableYears,
                DataRows = RowCodes,
                ReportType = IdType,
                HighTree = BroadItemItemAllowed,
                ReportName = model.ReportName
            };

            return View(reportModel);
        }

        /// <summary>
        /// Get Data by Date Range
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> ReportOutput(string SourceCols,
                                                      string SourceForms,
                                                       string SourceTariff,
                                                       string SourceTime,
                                                       string ReportType,
                                                       string ReportName,
                                                       bool SaveReport,
                                                       string ProductType)
        {

            //var model = new IndReportBuilterModel();
            if (string.IsNullOrEmpty(ReportName))
            {
                SaveReport = false;
            }

            string[] arrayForm = SourceForms.Split('|');
            var reportFormInput = new List<IndFormsModel>();
            var reportForms = new List<IndFormsModel>();
            var reportTariff = new List<string>();
            // var reportName = new List<string>();

            foreach (var f in arrayForm)
            {
                var form = f.Split(',');
                var rfModel = new IndFormsModel
                {
                    Form = form[0],
                    Number = form[1],
                    Name = form[2]
                };
                reportFormInput.Add(rfModel);

            }

            var srv = IndustryService.Instance;
            var forms = await srv.GetForms();

            foreach (var z in reportFormInput)
            {
                var form = forms[int.Parse(z.Form) - 1];
                reportForms.Add(z);
            }

            string[] array1 = SourceCols.Split('|');
            var cols = await srv.GetDataColumns();
            cols = cols.OrderBy(x => x.Number).ToList();

            var reportCols = new List<string>();

            foreach (var f in array1)
            {
                var col = cols[int.Parse(f) - 1];
                reportCols.Add("C" + col.Number.ToString());
            }

            string[] tariffArray = SourceTariff.Split('|');

            foreach (var f in tariffArray)
            {
                var pStr = string.Empty;
                if (!string.IsNullOrEmpty(f))
                {
                    pStr = f;
                    if (pStr.Contains("undefined"))
                    {
                        pStr = f.Replace("undefined", "");
                    }
                    if (ReportType == "0")
                        pStr = pStr.PadRight(8, '0');

                    reportTariff.Add(pStr);
                }
            }

            string[] TimeArray = SourceTime.Split('|');

            var YearsMonths = new List<YearMonthModel>();
            int Year = 0;
            int Month = 0;

            foreach (var Item in TimeArray)
            {
                if (Item != string.Empty)
                {

                    if (Item.Length == 4)
                    {
                        Year = int.Parse(Item);
                        Month = 0;
                    }
                    else
                    {
                        Month = int.Parse(Item);
                    }

                    if (Month > 0)
                    {
                        var dm = new YearMonthModel
                        {
                            Month = Month,
                            Year = Year
                        };
                        YearsMonths.Add(dm);
                    }
                }
            }

            bool rType = false;
            if(ReportType.Equals("0"))
            {
                rType = true;
            }

            var reportModel = new IndustryReportBuilterModel
            {
                FormNumber = reportForms,
                ColNumber = reportCols,
                TariffCodes = reportTariff,
                YM = YearsMonths,
                IsTariff = rType
            };

            var Items = await srv.GetData(reportModel);

            outList = new List<IndDataOutModel>();

            foreach (var Item in Items)
            {
                var indList = new List<DataCols>();
                foreach (var LItem in Item.Datas)
                {
                    var imod = new DataCols
                    {
                        Code = LItem.Code,
                        ColCode = LItem.ColCode,
                        ColValue = LItem.ColValue,
                        Description = LItem.Description
                    };

                    indList.Add(imod);
                }

                var md = new IndDataOutModel
                {
                    FormNumber = Item.FormNumber,
                    Name = Item.Name,
                    Number = Item.Number,
                    ReportingPeriod = Item.ReportingPeriod,
                    TabName = Item.TabName,
                    FormLink = Item.FormLink,
                    Datas = indList
                };

                outList.Add(md);
            }


            var outListDB = new List<IndDataOutDB>();

            foreach (var Item in outList)
            {
                var outCols = new List<DataColsDB>();

                foreach (var ICol in Item.Datas)
                {
                    var olM = new DataColsDB
                    {
                        Code = ICol.Code,
                        ColCode = ICol.ColCode,
                        ColValue = ICol.ColValue,
                        Description = ICol.Description
                    };

                    outCols.Add(olM);
                }

                var ItemModel = new IndDataOutDB
                {
                    Datas = outCols,
                    FormNumber = Item.FormNumber,
                    Name = Item.Name,
                    Number = Item.Number,
                    ReportingPeriod = Item.ReportingPeriod,
                    TabName = Item.TabName,
                    FormLink = Item.FormLink
                };

                outListDB.Add(ItemModel);
            }

            if (SaveReport)
            {


                var formsToSave = reportForms.OrderBy(x => x.Form).ThenBy(z => z.Number).ToList();
                var codeToSave = reportTariff.OrderBy(x => x).ToList();
                var ColsToSave = reportCols.OrderBy(x => x).ToList();
                var utils = new HelpersService();
                var TimeIds = utils.ConvertToTimeDimension(SourceTime);

                var saveModel = new IndustrySavedReportsDB
                {
                    Date = DateTime.Now,
                    ReportName = ReportName,
                    ReportType = int.Parse(ReportType),
                    SaveReport = SaveReport,
                    Forms = formsToSave,
                    TCodes = codeToSave,
                    ReoprtCols = ColsToSave,
                    ProductType = ProductType,
                    SourceTime = TimeIds
                };

                await srv.SaveReport(saveModel);
            }

            var jList = outList.OrderBy(x => x.Number).OrderBy(x => x.ReportingPeriod.Year).OrderBy(x => x.ReportingPeriod.Month).OrderBy(x => x.FormNumber).ToList();
            return Json(new { success = true, message = outListDB });
        }

        [Authorize]
        public async Task<IActionResult> ReportDownLoad()
        {
            CsvWriter csvWriter = new CsvWriter();
            StringBuilder sb = new StringBuilder();

            //Get The Col Names and Count
            var colListInt = new List<int>();
            foreach (var Item in outList)
            {
                foreach (var dt in Item.Datas)
                {
                    bool has = colListInt.Any(cus => cus == int.Parse(dt.ColCode.Remove(0, 1)));
                    if (!has)
                        colListInt.Add(int.Parse(dt.ColCode.Remove(0, 1)));
                }
            }

            colListInt = colListInt.OrderBy(x => x).ToList();
            var colList = new List<string>();

            foreach (var col in colListInt)
            {
                colList.Add("C" + col);
            }

            //Get Col Descriptions
            var colNames = new List<string>();

            var srv = IndustryDataService.Instance;
            var colsList = await srv.GetColumns();

            foreach (var desc in colList)
            {
                try
                {
                    var descName = colsList.FirstOrDefault(s => s.Number == int.Parse(desc.Remove(0, 1))).Name;
                    if (descName is not null)
                        colNames.Add(descName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            string val = "Number,Name,Month,Year,Form,TabName,Code,Description";
            int colIdx = 0;
            foreach (var colItem in colList)
            {
                try
                {
                    val += "," + "C" + colNames[colIdx];
                    colIdx++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            sb.Append(val);
            sb.Append('\n');

            foreach (var Item in outList)
            {
                // var DateString = Item.ReportingPeriod.Day + "/" + Item.ReportingPeriod.Month + "/" + Item.ReportingPeriod.Year;
                val = Item.Number + "," + Item.Name + "," + Item.ReportingPeriod.Month + "," + Item.ReportingPeriod.Year + "," + Item.FormNumber + "," + Item.TabName;
                sb.Append(val);

                // sb.AppendLine(csvWriter.CreateCsvLine(Item, properties));
                string csv = string.Empty;
                bool bFirst = true;
                string LastCode = string.Empty;
                string LastColCode = string.Empty;
                int lastIndex = 0;
                foreach (var dt in Item.Datas)
                {
                    dt.Description = dt.Description.Replace(",", " ");

                    if (!GetLastCode(LastCode, dt.Code))
                    {
                        int index = colList.FindIndex(x => x == dt.ColCode);

                        if (bFirst)
                        {
                            csv = string.Empty;
                            csv += "," + dt.Code + "," + dt.Description;
                            bFirst = false;
                            for (int i = 0; i <= index; i++)
                            {
                                csv += ",";
                            }
                            csv += dt.ColValue.ToString() + '\n';
                            sb.Append(csv);
                        }
                        else
                        {
                            csv = string.Empty;
                            csv += ",,,,,," + dt.Code + "," + dt.Description;

                            //if (dt.Code.Equals("110"))
                            //{

                                var cList = Item.Datas.Where(x => x.Code.Equals(dt.Code)).ToList();

                            foreach (var ItemC in cList)
                            {
                                index = colList.FindIndex(x => x == ItemC.ColCode);
                                ItemC.Index = index;
                            }

                            bFirst = true;
                            int lIndex = 0;
                            foreach (var x in cList)
                            {
                                if (bFirst)
                                {
                                    bFirst = false;
                                    lIndex = x.Index;
                                        for (int i = 0; i <= x.Index; i++)
                                        {
                                            csv += ",";
                                        }
                                        // csv += PadLine(lIndex);
                                        csv += x.ColValue.ToString();
                                }
                                else
                                {
                                    int nextIndex = x.Index - lIndex;
                                   
                                    csv += PadLine(nextIndex);
                                    csv += x.ColValue.ToString();
                                    lIndex = x.Index;
                                }
                            }


                            csv += '\n';
                            sb.Append(csv);

                            //}
                        }

                        sb.Append(csv);

                        lastIndex = 0;
                        LastCode = dt.Code;
                        LastColCode = dt.ColCode;

                    }

                }
            }


            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);

            tw.WriteLine(sb.ToString());

            tw.Flush();

            var FileName = "Industry" + ".csv";
            var length = ms.Length;
            tw.Close();
            var toWrite = new byte[length];
            Array.Copy(ms.GetBuffer(), 0, toWrite, 0, length);
            ms.Close();
            return File(toWrite, "text/plain", FileName);
        }

        private bool GetLastCode(string listCode, string lastCode)
        {
            if (listCode.Equals(lastCode))
                return true;
            else
                return false;
        }

        private string PadLine(int idx)
        {
            string szReturn = string.Empty;

            for (int i = 1; i <= idx; i++)
            {
                szReturn += ",";
            }

            //switch (idx)
            //{
            //    case 1:
            //        szReturn += ",";
            //        break;
            //    case 2:
            //        szReturn += ",,";
            //        break;
            //    case 3:
            //        szReturn += ",,,";
            //        break;
            //    case 4:
            //        szReturn += ",,,,";
            //        break;
            //    case 5:
            //        szReturn += ",,,,,";
            //        break;
            //    case 6:
            //        szReturn += ",,,,,,";
            //        break;
            //    case 7:
            //        szReturn += ",,,,,,,";
            //        break;
            //    case 8:
            //        szReturn += ",,,,,,,,";
            //        break;
            //    case 9:
            //        szReturn += ",,,,,,,,,";
            //        break;
            //    case 10:
            //        szReturn += ",,,,,,,,,,";
            //        break;
            //    case 11:
            //        szReturn += ",,,,,,,,,,,";
            //        break;
            //    case 12:
            //        szReturn += ",,,,,,,,,,,,";
            //        break;
            //    case 13:
            //        szReturn += ",,,,,,,,,,,,,";
            //        break;
            //    case 14:
            //        szReturn += ",,,,,,,,,,,,,,";
            //        break;
            //    case 15:
            //        szReturn += ",,,,,,,,,,,,,,,";
            //        break;
            //    case 16:
            //        szReturn += ",,,,,,,,,,,,,,,,";
            //        break;
            //    case 17:
            //        szReturn += ",,,,,,,,,,,,,,,,,";
            //        break;
            //    case 18:
            //        szReturn += ",,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 19:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 20:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 21:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 22:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 23:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 24:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 25:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 26:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 27:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 28:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //    case 29:
            //        szReturn += ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,";
            //        break;
            //}
            return szReturn;
        }

        [Authorize]
        public async Task<IActionResult> RowsIndex()
        {
            var srvrv = IndustryDataService.Instance;
            var rowsList = await srvrv.GetRows();

            IQueryable data = rowsList.AsQueryable();
            return View(data);

        }

        [Authorize]
        public async Task<IActionResult> ColsIndex()
        {
            var srv = IndustryDataService.Instance;
            var colsList = await srv.GetColumns();

            IQueryable data = colsList.AsQueryable();
            return View(data);

        }

        [Authorize]
        public IActionResult AddSource()
        {

            var modelOut = new FileUploadTypesModel
            {
                Description = string.Empty,
                Number = 0
            };

            return View(modelOut);
        }

        [Authorize]
        public IActionResult AddDataRow()
        {

            var modelOut = new IndustryRowModel
            {
                Description = string.Empty,
                Number = string.Empty
            };

            return View(modelOut);
        }

        [Authorize]
        public IActionResult AddDataCol()
        {

            var modelOut = new IndustryColumnModel
            {
                Name = string.Empty,
                Number = 0
            };

            return View(modelOut);
        }

        [Authorize]
        public async Task<IActionResult> EditDataCol(string _id)
        {
            var srv = IndustryDataService.Instance;
            var model = await srv.GetCol(_id);

            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> EditDataRow(string _id)
        {
            var srv = IndustryDataService.Instance;
            var model = await srv.GetRow(_id);

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> EditDataTab(string data)
        {
            string[] dataList = data.Split("|");
            var srv = IndustryService.Instance;
            var model = await srv.GetIndustryDataCols(dataList[0]);

            var dataModel = model.Datas.FirstOrDefault(x => x.Data.Equals(data));

            return View(dataModel);
        }

        [Authorize]
        public async Task<IActionResult> EditDataTabSave(DataCols model)
        {
            string[] dataList = model.Data.Split("|");
            var srv = IndustryService.Instance;
            var dmodel = await srv.GetIndustryDataCols(dataList[0]);
            var dataModel = dmodel.Datas.FirstOrDefault(x => x.Data.Equals(model.Data));

            dataModel.Code = model.Code;
            dataModel.ColCode = model.ColCode;
            dataModel.ColValue = model.ColValue;
            dataModel.Description = model.Description;

            await srv.UpdateIndustryDataCols(dmodel);

            return RedirectToAction("Index", "IndustryFiles");
        }

        [Authorize]
        public async Task<IActionResult> EditSource(string _id)
        {
            var srv = IndustryDataService.Instance;
            var model = await srv.GetSource(_id);

            return View(model);
        }

        public async Task<IActionResult> EditSaveDataRows(IndustryRowModel model)
        {
            var srv = IndustryDataService.Instance;
            await srv.SaveRow(model);
            return RedirectToAction("RowsIndex", "IndustryFiles");
        }

        public async Task<IActionResult> EditSaveSourceRows(FileUploadTypesModel model)
        {
            var srv = IndustryDataService.Instance;
            await srv.SaveSource(model);
            return RedirectToAction("SourceIndex", "IndustryFiles");
        }
        public async Task<IActionResult> EditSaveDataCols(IndustryColumnModel model)
        {
            var srv = IndustryDataService.Instance;
            await srv.SaveCol(model);
            return RedirectToAction("ColsIndex", "IndustryFiles");
        }

        public async Task<IActionResult> AddSaveDataRows(IndustryRowModel model)
        {
            var db = DBContext.Instance;

            var modelDB = new IndustryRowDB
            {
                Number = model.Number,
                Description = model.Description
            };

            await db.IndustryRowDB.InsertOneAsync(modelDB);

            return RedirectToAction("RowsIndex", "IndustryFiles");
        }

        public async Task<IActionResult> AddSaveSourceRows(FileUploadTypesModel model)
        {
            var db = DBContext.Instance;

            var modelDB = new FileUploadTypesDB
            {
                Number = model.Number,
                Description = model.Description
            };

            await db.FileUploadTypesDB.InsertOneAsync(modelDB);

            return RedirectToAction("SourceIndex", "IndustryFiles");
        }

        public async Task<IActionResult> AddSaveDataCols(IndustryColumnModel model)
        {
            var db = DBContext.Instance;

            var modelDB = new IndustryColumnDB
            {
                Number = model.Number,
                Name = model.Name
            };

            await db.IndustryColumnDB.InsertOneAsync(modelDB);

            return RedirectToAction("ColsIndex", "IndustryFiles");
        }

        [Authorize]
        public async Task<IActionResult> DeleteSourceRow(string _id)
        {
            var srv = IndustryDataService.Instance;
            await srv.DeleteSource(_id);
            return RedirectToAction("SourceIndex", "IndustryFiles");
        }

        [Authorize]
        public async Task<IActionResult> DeleteDataRow(string _id)
        {
            var srv = IndustryDataService.Instance;
            await srv.DeleteRow(_id);
            return RedirectToAction("RowsIndex", "IndustryFiles");
        }

        [Authorize]
        public async Task<IActionResult> DeleteDataCol(string _id)
        {
            var srv = IndustryDataService.Instance;
            await srv.DeleteCol(_id);
            return RedirectToAction("ColsIndex", "IndustryFiles");
        }
    }
}