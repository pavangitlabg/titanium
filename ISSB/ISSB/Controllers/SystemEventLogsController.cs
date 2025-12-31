using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Data.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ISSB.Controllers
{
    public class SystemEventLogsController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new SystemEventService();
            var model = await srv.GetSystemEventLogs();
            IQueryable data = model.AsQueryable();

            return View(data);
        }

        [Authorize]
        public async Task<IActionResult> Detail(string _id)
        {
            var srv = new SystemEventService();
            var systeEventModel = await srv.GetSystemEventLogsById(_id);


            switch ((int)systeEventModel.DataType)
            {
                case 5:
                    {
                        UserModel _UserModel = (UserModel)systeEventModel.Model;
                        var scSrv = new SourceCountryService();
                        _UserModel.SCList = await scSrv.GetActiveSourceCountries();
                        var tariffSrv = new ProductService();
                        _UserModel.TCList = await tariffSrv.GetAllProducts();
                        ViewBag.TCList = JsonConvert.SerializeObject(_UserModel.AllowedTariffCodes);
                        return View("UserModelView", _UserModel);
                    }
                case 6:
                    MarketCountryModel _MarketCountryModel = (MarketCountryModel)systeEventModel.Model;
                    return View("MarketCountryModelView", _MarketCountryModel);
                case 7:
                    SourceCountryModel _SourceCountryModel = (SourceCountryModel)systeEventModel.Model;
                    return View("SourceCountryModelView", _SourceCountryModel);
                case 8:
                    AssetModel _AssetModel = (AssetModel)systeEventModel.Model;
                    return View("AssetModelView", _AssetModel);
                case 14:
                    EmailCredentialsModel _EmailCredentialsModel = (EmailCredentialsModel)systeEventModel.Model;
                    return View("EmailCredentialsModelView", _EmailCredentialsModel);
                case 15:
                    ErrorModel _ErrorModel = (ErrorModel)systeEventModel.Model;
                    return View("ErrorModelView", _ErrorModel);
                case 16:
                    ImportFileTypeModel _ImportFileTypeModel = (ImportFileTypeModel)systeEventModel.Model;
                    return View("ImportFileTypeModelView", _ImportFileTypeModel);
                case 17:
                    MarketCountryExceptionModel _MarketCountryExceptionModel = (MarketCountryExceptionModel)systeEventModel.Model;
                    return View("MarketCountryExceptionModelView", _MarketCountryExceptionModel);
                case 18:
                    {
                        MarketCountryFilterModel _MarketCountryFilterModel = (MarketCountryFilterModel)systeEventModel.Model;
                        var filterSrv = new MarketCountryFilterService();
                        var fModelMC = await filterSrv.GetFilterById(_MarketCountryFilterModel._id);

                        ViewBag.ObjId = fModelMC._id;

                        var listItemsMCF = string.Empty;

                        foreach (var item in fModelMC.Items)
                        {
                            listItemsMCF += item.GeoCode + "|";
                        }

                        ViewBag.Name = fModelMC.Name;
                        ViewBag.Notes = fModelMC.Notes;
                        ViewBag.Active = fModelMC.Active;
                        ViewBag.Locked = fModelMC.CanEdit;
                        ViewBag.SelectedItems = listItemsMCF;

                        var srvMC = new MarketCountryService();
                        var model = await srvMC.GetActiveMarketCountries();
                        IQueryable data = model.AsQueryable();
                        return View("MarketCountryFilterModelView", data);                        
                    }                    
                case 19:
                    PortsModel _PortsModel = (PortsModel)systeEventModel.Model;
                    return View("PortsModelView", _PortsModel);
                case 21:
                    ProductAllFilterModel _ProductAllFilterModel = (ProductAllFilterModel)systeEventModel.Model;
                    return View("ProductFilterModelView", _ProductAllFilterModel);
                case 22:
                    RegisterModel _RegisterModel = (RegisterModel)systeEventModel.Model;
                    return View("RegisterModelView", _RegisterModel);
                case 24:
                    SalesProfileModel _SalesProfileModel = (SalesProfileModel)systeEventModel.Model;
                    return View("SalesProfileModelView", _SalesProfileModel);
                case 25:
                    SiteIssuesModel _SiteIssuesModel = (SiteIssuesModel)systeEventModel.Model;
                    return View("SiteIssuesModelView", _SiteIssuesModel);
                case 26:
                    SiteVisitorModel _SiteVisitorModel = (SiteVisitorModel)systeEventModel.Model;
                    return View("SiteVisitorModelView", _SiteVisitorModel);
                case 27:
                    SourceCountryExceptionModel _SourceCountryExceptionModel = (SourceCountryExceptionModel)systeEventModel.Model;
                    return View("SourceCountryExceptionModelView", _SourceCountryExceptionModel);
                case 29:
                    SourceCountryFilterModel _SourceCountryFilterModel = (SourceCountryFilterModel)systeEventModel.Model;

                    var filterSrvSC = new SourceCountryFilterService();
                    var fModel = await filterSrvSC.GetFilterById(_SourceCountryFilterModel._id);
                    ViewBag.ObjId = fModel._id;

                    var listItems = string.Empty;

                    foreach (var item in fModel.Items)
                    {
                        listItems += item.GeoCode + "|";
                    }

                    ViewBag.Name = fModel.Name;
                    ViewBag.Notes = fModel.Notes;
                    ViewBag.Active = fModel.Active;
                    ViewBag.Locked = fModel.CanEdit;
                    ViewBag.SelectedItems = listItems;

                    var srvSC = new SourceCountryService();
                    var modelSC = await srvSC.GetSourceCountries();
                    IQueryable dataSCF = modelSC.AsQueryable();
                    return View("SourceCountryFilterModelView", dataSCF);
                case 30:
                    SourceCountryMappingModel _SourceCountryMappingModel = (SourceCountryMappingModel)systeEventModel.Model;
                    return View("SourceCountryMappingModelView", _SourceCountryMappingModel);
                case 32:
                    TariffModel _TariffModel = (TariffModel)systeEventModel.Model;
                    return View("TariffModelView", _TariffModel);
                case 33:
                    TariffExceptionsModel _TariffExceptionsModel = (TariffExceptionsModel)systeEventModel.Model;
                    return View("TariffExceptionModelView", _TariffExceptionsModel);
                case 34:
                    TariffIndexModel _TariffIndexModel = (TariffIndexModel)systeEventModel.Model;
                    return View("TariffIndexModelView", _TariffIndexModel);
                case 36:
                    {
                        ClientDataImportModel _ClientDataImportModel = (ClientDataImportModel)systeEventModel.Model;
                        var srvType = new ImportFileTypeService();
                        var TypeList = await srvType.GetFileTypes();
                        List<SelectListItem> FileTypeitems = new List<SelectListItem>();
                        var TypeListDistinct = TypeList.Select(e => e.KEY).Distinct().ToList();

                        foreach (var Item in TypeListDistinct)
                        {
                            var sModel = new SelectListItem
                            {
                                Text = Item,
                                Value = Item
                            };
                            FileTypeitems.Add(sModel);
                        }

                        ViewBag.FileTypes = FileTypeitems;
                        var lookupSrv = new TariffService();
                        ViewBag.TariffIndex = await lookupSrv.GetTariffIndexLookup();                      
                        return View("ClientDataImportModelView", _ClientDataImportModel);
                    }                
                case 37:
                    EmailMessagesModel _EmailMessagesModel = (EmailMessagesModel)systeEventModel.Model;
                    return View("EmailMessagesModelView", _EmailMessagesModel);
                case 38:
                    HomeModel _HomeModel = (HomeModel)systeEventModel.Model;
                    return View("ContactUsModelView", _HomeModel);
                case 39:
                    ReportKeysModel _ReportKeysModel = (ReportKeysModel)systeEventModel.Model;
                    return View("ReportKeysModelView", _ReportKeysModel);
                case 41:
                    SavedQueryModel _SavedQueryModel = (SavedQueryModel)systeEventModel.Model;
                    return View("SavedQueryModelView", _SavedQueryModel);
                case 42:
                    CurrencyCodesModel _CurrencyCodesModel = (CurrencyCodesModel)systeEventModel.Model;
                    return View("CurrencyCodesModelView", _CurrencyCodesModel);
                default:
                    return RedirectToAction("Index");

            }

        }


        [Authorize]
        public async Task<IActionResult> Delete(string _id)
        {
            var srv = new SystemEventService();
            var modelOut = await srv.GetSystemEventLogsById(_id);
            await srv.Delete(modelOut);

            return RedirectToAction("Index", "SystemEventLogs");

        }
    }
}
