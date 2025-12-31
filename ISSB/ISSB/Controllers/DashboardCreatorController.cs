using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System.Web;
using System;
using System.Security.Claims;
using Data.Enums;
using ISSB.Helpers;

namespace ISSB.Controllers
{
    public class DashboardCreatorController : Controller
    {

        [Authorize]
        public async Task<IActionResult> Index(int ID, string dashname, string dashdescription,string RecordID)
        {

            if (dashname == null)
            {
                dashname = string.Empty;
            }

            if (dashdescription == null)
            {
                dashdescription = string.Empty;
            }

            ViewBag.DashName = dashname;
            ViewBag.DashDescription = dashdescription;

            var srv = new DashboardCreatorService();
            var model = await srv.GetMenuRows();
            IQueryable data = model.AsQueryable();

            var objModel = await srv.GetMenuObjects();
            IQueryable objData = objModel.AsQueryable();
            ViewBag.TYPES = objData;

            var usrDashSrv = new UserDashboardService();


            if (ID > 0)
            {
                var srvDash = new DashboardCreatorService();
                var outModel = await srvDash.GetMenuHTML(ID);
                string html = outModel.HTML.Replace("[DASHBORD-HEADER]", outModel.Description);
                var dModel = await usrDashSrv.GetCurrentDesignDashboard(GetUserEmail(),true);



                if(RecordID != null)
                    dModel = await usrDashSrv.GetCurrentDashboardByGUID(RecordID);

                var eType = Data.Enums.DashEnums.OneColRow;

                switch (ID)
                {
                    case 1:
                        {
                            eType = Data.Enums.DashEnums.OneColRow;
                            break;
                        }
                    case 2:
                        {
                            eType = Data.Enums.DashEnums.TwoColRow;
                            break;
                        }
                    case 3:
                        {
                            eType = Data.Enums.DashEnums.ThreeColRow;
                            break;
                        }
                    case 4:
                        {
                            eType = Data.Enums.DashEnums.FourColRow;
                            break;
                        }
                }

                if (dModel == null)
                {
                    var lModel = new UserDashboardItemsModel
                    {
                        HTML = html,
                        ID = 1,
                        Type = eType,
                        Title1 = string.Empty,
                        Title2 = string.Empty,
                        Title3 = string.Empty,
                        Title4 = string.Empty,
                        SubTitle1 = string.Empty,
                        SubTitle2 = string.Empty,
                        SubTitle3 = string.Empty,
                        SubTitle4 = string.Empty,
                        SelectedIndex1 = 0,
                        SelectedIndex2 = 0,
                        SelectedIndex3 = 0,
                        SelectedIndex4 = 0
                    };

                    var listModel = new List<UserDashboardItemsModel>();

                    listModel.Add(lModel);

                    var nModel = new UserDashboardModel
                    {
                        bDesignMode = true,
                        DashBoardName = dashname,
                        Description = dashdescription,
                        ID = 1,
                        UserIdentityName = GetUserEmail(),
                        UserName = GetUserName(),
                        Items = listModel
                    };

                    await usrDashSrv.InsertDashBoard(nModel);
                    // dModel = await usrDashSrv.GetCurrentDesignDashboard(GetUserEmail(), true);


                }
                else
                {
                    var lModel = new UserDashboardItemsModel
                    {
                        HTML = html,
                        ID = dModel.Items.Count + 1,
                        Type = eType,
                        Title1 = string.Empty,
                        Title2 = string.Empty,
                        Title3 = string.Empty,
                        Title4 = string.Empty,
                        SubTitle1 = string.Empty,
                        SubTitle2 = string.Empty,
                        SubTitle3 = string.Empty,
                        SubTitle4 = string.Empty,
                        SelectedIndex1 = 0,
                        SelectedIndex2 = 0,
                        SelectedIndex3 = 0,
                        SelectedIndex4 = 0
                    };

                    dModel.Items.Add(lModel);
                    //changed from update
                    await usrDashSrv.UpdateDashBoard(dModel);

                }

                var request = HttpContext.Request;
                var query = request.Query;

                System.Collections.Specialized.NameValueCollection nvc = HttpUtility.ParseQueryString(query.ToString());
                nvc.Remove("ID");
                nvc.Remove("dashName");

                return RedirectToAction("Index", new { RecordID = RecordID });
            }

            usrDashSrv = new UserDashboardService();
            var ongoingModel = await usrDashSrv.GetCurrentDesignDashboard(GetUserEmail(), true);

            
            if (RecordID != null )
                ongoingModel = await usrDashSrv.GetCurrentDashboardByID(RecordID, false);

            if(ongoingModel == null)
                ongoingModel = await usrDashSrv.GetCurrentDesignDashboard(GetUserEmail(), true);

            if (ongoingModel != null)
            {
                ViewBag.RecordID = ongoingModel._id;
                string htmlBuilder = string.Empty;
                var gridList = new List<DashBoardItemModel>();
                foreach (var X in ongoingModel.Items)
                {
                    string sHTML = string.Empty;
                    if (X.Type.Equals(Data.Enums.DashEnums.OneColRow))
                    {
                        sHTML = X.HTML.Replace("[PLACEHOLDER-TITLE]", "1-1-title-" + X.ID)
                                .Replace("[PLACEHOLDER-SUBTITLE]", "1-1-subtitle-" + X.ID)
                                .Replace("[PLACEHOLDER-TITLE-ERROR]", "1-1-title-error-" + X.ID)
                                .Replace("[PLACEHOLDER-SUBTITLE-ERROR]", "1-1-subtitle-error-" + X.ID)
                                .Replace("[PLACEHOLDER-SAVE]", "save-id-1-1-" + X.ID)
                                .Replace("[PLACEHOLDER-EDIT]", "edit-id-1-1-" + X.ID)
                                .Replace("[PLACEHOLDER-DELETE]", "1-1-delete-" + X.ID)
                                .Replace("[PLACEHOLDER-ITEMHEADER]", "Item [" + X.ID + "]")
                                .Replace("[PLACEHOLDER-GRID]", "grid-id-1-1-" + X.ID);

                        //get the grid Ids
                        var gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Type = (int)Data.Enums.DashEnums.OneColRow,
                            DashboardID = "grid-id-1-1-" + X.ID,
                            SelectButtonID = "grid-id-1-1-" + X.ID,
                            SelectedIndex1 = X.SelectedIndex1,
                            Title1 = X.Title1,
                            SubTitle1 = X.SubTitle1

                        };
                        gridList.Add(gridModel);
                        htmlBuilder += sHTML;
                    }
                    if (X.Type.Equals(Data.Enums.DashEnums.TwoColRow))
                    {
                        sHTML = X.HTML.Replace("[PLACEHOLDER-TITLE1]", "2-1-title-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE2]", "2-2-title-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE1]", "2-1-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE2]", "2-2-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR1]", "2-1-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR2]", "2-2-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR1]", "2-1-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR2]", "2-2-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE1]", "save-id-2-1-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE2]", "save-id-2-2-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT1]", "edit-id-2-1-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT2]", "edit-id-2-2-" + X.ID)
                              .Replace("[PLACEHOLDER-DELETE]", "2-1-delete-" + X.ID)
                              .Replace("[PLACEHOLDER-ITEMHEADER]", "Item [" + X.ID + "]")
                              .Replace("[PLACEHOLDER-GRID1]", "grid-id-2-1-" + X.ID)
                              .Replace("[PLACEHOLDER-GRID2]", "grid-id-2-2-" + X.ID);

                        //get the grid Ids
                        var gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 1,
                            Type = (int)Data.Enums.DashEnums.TwoColRow,
                            DashboardID = "grid-id-2-1-" + X.ID,
                            SelectButtonID = "grid-id-2-1-" + X.ID,
                            SelectedIndex1 = X.SelectedIndex1,

                            Title1 = X.Title1,
                            SubTitle1 = X.SubTitle1

                        };
                        gridList.Add(gridModel);
                        gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 2,
                            Type = (int)Data.Enums.DashEnums.TwoColRow,
                            DashboardID = "grid-id-2-2-" + X.ID,
                            SelectButtonID = "grid-id-2-2-" + X.ID,
                            SelectedIndex2 = X.SelectedIndex2,
                            Title2 = X.Title2,
                            SubTitle2 = X.SubTitle2
                        };
                        gridList.Add(gridModel);
                        htmlBuilder += sHTML;
                    }

                    if (X.Type.Equals(Data.Enums.DashEnums.ThreeColRow))
                    {
                        sHTML = X.HTML.Replace("[PLACEHOLDER-TITLE1]", "3-1-title-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE2]", "3-2-title-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE3]", "3-3-title-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE1]", "3-1-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE2]", "3-2-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE3]", "3-3-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR1]", "3-1-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR2]", "3-2-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR3]", "3-3-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR1]", "3-1-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR2]", "3-2-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR3]", "3-3-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE1]", "save-id-3-1-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE2]", "save-id-3-2-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE3]", "save-id-3-3-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT1]", "edit-id-3-1-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT2]", "edit-id-3-2-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT3]", "edit-id-3-3-" + X.ID)
                              .Replace("[PLACEHOLDER-DELETE]", "3-1-delete-" + X.ID)
                              .Replace("[PLACEHOLDER-ITEMHEADER]", "Item [" + X.ID + "]")
                              .Replace("[PLACEHOLDER-GRID1]", "grid-id-3-1-" + X.ID)
                              .Replace("[PLACEHOLDER-GRID2]", "grid-id-3-2-" + X.ID)
                              .Replace("[PLACEHOLDER-GRID3]", "grid-id-3-3-" + X.ID);

                        //get the grid Ids
                        var gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 1,
                            Type = (int)Data.Enums.DashEnums.ThreeColRow,
                            DashboardID = "grid-id-3-1-" + X.ID,
                            SelectButtonID = "grid-id-3-1-" + X.ID,
                            SelectedIndex1 = X.SelectedIndex1,

                            Title1 = X.Title1,
                            SubTitle1 = X.SubTitle1

                        };
                        gridList.Add(gridModel);
                        gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 2,
                            Type = (int)Data.Enums.DashEnums.ThreeColRow,
                            DashboardID = "grid-id-3-2-" + X.ID,
                            SelectButtonID = "grid-id-3-2-" + X.ID,
                            SelectedIndex2 = X.SelectedIndex2,
                            Title2 = X.Title2,
                            SubTitle2 = X.SubTitle2
                        };
                        gridList.Add(gridModel);
                        gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 3,
                            Type = (int)Data.Enums.DashEnums.ThreeColRow,
                            DashboardID = "grid-id-3-3-" + X.ID,
                            SelectButtonID = "grid-id-3-3-" + X.ID,
                            SelectedIndex3 = X.SelectedIndex3,
                            Title3 = X.Title3,
                            SubTitle3 = X.SubTitle3
                        };
                        gridList.Add(gridModel);
                        htmlBuilder += sHTML;
                    }

                    if (X.Type.Equals(Data.Enums.DashEnums.FourColRow))
                    {
                        sHTML = X.HTML.Replace("[PLACEHOLDER-TITLE1]", "4-1-title-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE2]", "4-2-title-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE3]", "4-3-title-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE4]", "4-4-title-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE1]", "4-1-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE2]", "4-2-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE3]", "4-3-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE4]", "4-4-subtitle-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR1]", "4-1-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR2]", "4-2-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR3]", "4-3-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-TITLE-ERROR4]", "4-4-title-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR1]", "4-1-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR2]", "4-2-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR3]", "4-3-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SUBTITLE-ERROR4]", "4-4-subtitle-error-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE1]", "save-id-4-1-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE2]", "save-id-4-2-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE3]", "save-id-4-3-" + X.ID)
                              .Replace("[PLACEHOLDER-SAVE4]", "save-id-4-4-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT1]", "edit-id-4-1-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT2]", "edit-id-4-2-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT3]", "edit-id-4-3-" + X.ID)
                              .Replace("[PLACEHOLDER-EDIT4]", "edit-id-4-4-" + X.ID)
                              .Replace("[PLACEHOLDER-DELETE]", "4-1-delete-" + X.ID)
                              .Replace("[PLACEHOLDER-ITEMHEADER]", "Item [" + X.ID + "]")
                              .Replace("[PLACEHOLDER-GRID1]", "grid-id-4-1-" + X.ID)
                              .Replace("[PLACEHOLDER-GRID2]", "grid-id-4-2-" + X.ID)
                              .Replace("[PLACEHOLDER-GRID3]", "grid-id-4-3-" + X.ID)
                              .Replace("[PLACEHOLDER-GRID4]", "grid-id-4-4-" + X.ID);

                        //get the grid Ids
                        var gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 1,
                            Type = (int)Data.Enums.DashEnums.FourColRow,
                            DashboardID = "grid-id-4-1-" + X.ID,
                            SelectButtonID = "grid-id-4-1-" + X.ID,
                            SelectedIndex1 = X.SelectedIndex1,

                            Title1 = X.Title1,
                            SubTitle1 = X.SubTitle1

                        };
                        gridList.Add(gridModel);
                        gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 2,
                            Type = (int)Data.Enums.DashEnums.FourColRow,
                            DashboardID = "grid-id-4-2-" + X.ID,
                            SelectButtonID = "grid-id-4-2-" + X.ID,
                            SelectedIndex2 = X.SelectedIndex2,
                            Title2 = X.Title2,
                            SubTitle2 = X.SubTitle2
                        };
                        gridList.Add(gridModel);
                        gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 3,
                            Type = (int)Data.Enums.DashEnums.FourColRow,
                            DashboardID = "grid-id-4-3-" + X.ID,
                            SelectButtonID = "grid-id-4-3-" + X.ID,
                            SelectedIndex3 = X.SelectedIndex3,
                            Title3 = X.Title3,
                            SubTitle3 = X.SubTitle3
                        };
                        gridList.Add(gridModel);
                        gridModel = new DashBoardItemModel
                        {
                            ID = X.ID,
                            Pos = 4,
                            Type = (int)Data.Enums.DashEnums.FourColRow,
                            DashboardID = "grid-id-4-4-" + X.ID,
                            SelectButtonID = "grid-id-4-4-" + X.ID,
                            SelectedIndex4 = X.SelectedIndex4,
                            Title4 = X.Title4,
                            SubTitle4 = X.SubTitle4
                        };
                        gridList.Add(gridModel);
                        htmlBuilder += sHTML;
                    }

                }

                ViewBag.GridList = JsonConvert.SerializeObject(gridList);

                ViewBag.DashName = ongoingModel.DashBoardName;
                ViewBag.DashDescription = ongoingModel.Description;
                if (!ongoingModel.DashBoardName.Equals(string.Empty))
                {
                    var str = new HtmlString(htmlBuilder);
                    ViewBag.HTML = str;
                }
            }

            return View(data);

        }

        [Authorize]
        public IActionResult Add()
        {

            var model = new DashBoardLayOutModel { ID = 1, Description = "xxx Column", ImageUrl = "../img/1-column.png" };
            return View(model);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveAll(string RecordId, string DashboardName, string Description)
        {
            //DashBoardLayOutModel model)
            var srvDash = new UserDashboardService();
            var Message = string.Empty;
            bool bIsValid = true;

            if (RecordId == null)
            {
                bIsValid = false;
                Message = "No Items have been added to your Dashoard";
                return Json(new { success = bIsValid, message = Message });
            }

            var ValidateModel = await srvDash.GetCurrentDashboardByGUID(RecordId);

            if (ValidateModel.Items.Count == 0)
            {
                bIsValid = false;
                Message = "No Items have been added to your Dashoard";
                return Json(new { success = bIsValid, message = Message });
            }

            foreach (var Item in ValidateModel.Items)
            {
                if (Item.Type == Data.Enums.DashEnums.OneColRow)
                {
                    if (Item.Title1.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle1.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query1.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                }

                if (Item.Type == Data.Enums.DashEnums.TwoColRow)
                {
                    if (Item.Title1.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle1.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query1.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Title2.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle2.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query2.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                }

                if (Item.Type == Data.Enums.DashEnums.ThreeColRow)
                {
                    if (Item.Title1.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle1.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query1.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Title2.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle2.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query2.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Title3.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for for Column 3 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle3.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 3 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query3.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 3 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                }

                if (Item.Type == Data.Enums.DashEnums.FourColRow)
                {
                    if (Item.Title1.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle1.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query1.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 1 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Title2.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle2.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query2.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 2 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Title3.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for for Column 3 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle3.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 3 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query3.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 3 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Title4.Equals(string.Empty))
                    {
                        Message = "Please Enter a Title for for Column 4 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                    if (Item.SubTitle4.Equals(string.Empty))
                    {
                        Message = "Please Enter a Sub Title for Column 4 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }

                    if (Item.Query4.SourceCountryGEOs == null)
                    {
                        Message = "Please build a query for Column 4 item [" + Item.ID + "]";
                        bIsValid = false;
                        break;
                    }
                }

            }

            //Save Dashboard
            if (bIsValid)
            {
                ValidateModel.bDesignMode = false;
                ValidateModel.DashBoardName = DashboardName;
                ValidateModel.Description = Description;

                await srvDash.UpdateDashBoardToComplete(ValidateModel);
            }
            // return RedirectToAction("Index", "DashboardCreator", new { @ID = 0 });

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.UserDashboardModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Dashboard Creator Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = RecordId
            };
            new EventLog(EventModel);

            return Json(new { success = bIsValid, message = Message });
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Save(DashBoardLayOutModel model)
        {
            //DashBoardLayOutModel model)
            var srvDash = new DashboardCreatorService();
            await srvDash.InsertDashRow(model);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.DashBoardLayOutModel,
                TransactionType = TransactionTypeEnums.Edit,
                MESSAGE = "Dashboard Creator Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = model
            };
            new EventLog(EventModel);

            return RedirectToAction("Index", "DashboardCreator", new { @ID = 0 });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateSaveItem(
                                                        string idx,
                                                        string dashname,
                                                        string dashDescription,
                                                        string title,
                                                        string subTitle,
                                                        int SelectedIndex,
                                                        string RecordId
                                                       )
        {
            var srvDash = new UserDashboardService();
            var model = await srvDash.GetCurrentDashboardByGUID(RecordId);
            string[] words = idx.Split('-');

            var Type = int.Parse(words[2]);
            var Pos = int.Parse(words[3]);
            var Idx = int.Parse(words[4]);
            model.DashBoardName = dashname;
            model.Description = dashDescription;

            if (Type == 1)
            {
                model.Items[Idx - 1].SelectedIndex1 = SelectedIndex - 1;
                model.Items[Idx - 1].Title1 = title;
                model.Items[Idx - 1].SubTitle1 = subTitle;
            }


            if (Type == 2)
            {
                if (Pos == 1)
                {
                    model.Items[Idx - 1].SelectedIndex1 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title1 = title;
                    model.Items[Idx - 1].SubTitle1 = subTitle;
                }

                if (Pos == 2)
                {
                    model.Items[Idx - 1].SelectedIndex2 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title2 = title;
                    model.Items[Idx - 1].SubTitle2 = subTitle;
                }

            }

            if (Type == 3)
            {
                if (Pos == 1)
                {
                    model.Items[Idx - 1].SelectedIndex1 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title1 = title;
                    model.Items[Idx - 1].SubTitle1 = subTitle;
                }

                if (Pos == 2)
                {
                    model.Items[Idx - 1].SelectedIndex2 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title2 = title;
                    model.Items[Idx - 1].SubTitle2 = subTitle;
                }

                if (Pos == 3)
                {
                    model.Items[Idx - 1].SelectedIndex3 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title3 = title;
                    model.Items[Idx - 1].SubTitle3 = subTitle;
                }

            }

            if (Type == 4)
            {
                if (Pos == 1)
                {
                    model.Items[Idx - 1].SelectedIndex1 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title1 = title;
                    model.Items[Idx - 1].SubTitle1 = subTitle;
                }

                if (Pos == 2)
                {
                    model.Items[Idx - 1].SelectedIndex2 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title2 = title;
                    model.Items[Idx - 1].SubTitle2 = subTitle;
                }

                if (Pos == 3)
                {
                    model.Items[Idx - 1].SelectedIndex3 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title3 = title;
                    model.Items[Idx - 1].SubTitle3 = subTitle;
                }

                if (Pos == 4)
                {
                    model.Items[Idx - 1].SelectedIndex4 = SelectedIndex - 1;
                    model.Items[Idx - 1].Title4 = title;
                    model.Items[Idx - 1].SubTitle4 = subTitle;
                }

            }

            //changed fromupdate
            await srvDash.UpdateDashBoard(model);            

            return Json(new { success = true });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Update(int ID, string dashname)
        {
            return RedirectToAction("Index", "DashboardCreator", new { @ID = ID, @dashname = dashname });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteItem(string ID,string RecordId)
        {
            string[] words = ID.Split('-');

            var usrDashSrv = new UserDashboardService();
            var ongoingModel = await usrDashSrv.GetCurrentDashboardByGUID(RecordId);

            var ListItem = ongoingModel.Items.FirstOrDefault(x => x.ID.Equals(int.Parse(words[3])));
            ongoingModel.Items.Remove(ListItem);

            int cnt = 1;
            foreach (var Item in ongoingModel.Items)
            {
                Item.ID = cnt;
                cnt++;
            }
            //changed from updat
            await usrDashSrv.UpdateDashBoard(ongoingModel);

            var UserId = User.FindFirst(ClaimTypes.Email).Value;
            var EventModel = new SystemEventLogsModel
            {
                DataType = DataTypeEnums.DashBoardLayOutModel,
                TransactionType = TransactionTypeEnums.Delete,
                MESSAGE = "Dashboard Creator Updated, [" + UserId + "]",
                DATE = DateTime.Now,
                USER = UserId,
                Model = ongoingModel
            };
            new EventLog(EventModel);


            return Json(new { success = true, recordid = RecordId });
        }
    

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateSaveQuery(
                                                        int DashItemType,
                                                        int DashItem,
                                                        string SourceCountryGrouping,
                                                        string TradeFlowGrouping,
                                                        string MarketCountryGrouping,
                                                        string ProductType,
                                                        string ProductGrouping,
                                                        string ValuesGrouping,
                                                        bool GroupByMonth,
                                                        bool GroupByQuarter,
                                                        bool GroupByYear,
                                                        string SourceCountries,
                                                        string MarketCountries,
                                                        string Products,
                                                        string YearsMonths,
                                                        string Name,
                                                        string Description,
                                                        string Comments,
                                                        string Id,
                                                        bool UsePorts,
                                                        string PortGrouping,
                                                        string Ports,
                                                        string RecordId,
                                                        string ComboTariffRegionSelector,
                                                        string ComboTariff2DigitSelector)
        {

            var sGeos = new List<DashBoardQuerySourceCountriesModel>();
            string[] GeoCodes = SourceCountries.Split('|');
            foreach (string geo in GeoCodes)
            {
                var sGeo = new DashBoardQuerySourceCountriesModel { GeoCode = geo };
                sGeos.Add(sGeo);
            }

            var mGeos = new List<DashBoardQueryMarketCountriesModel>();
            string[] mGeoCodes = MarketCountries.Split('|');
            foreach (string geo in mGeoCodes)
            {
                var mGeo = new DashBoardQueryMarketCountriesModel { GeoCode = geo };
                mGeos.Add(mGeo);
            }

            var listProducts = new List<DashBoardQueryProductsModel>();
            string[] pList = Products.Split('|');
            foreach (string prod in pList)
            {
                var pROD = new DashBoardQueryProductsModel { Product = prod };
                listProducts.Add(pROD);
            }

            var listPorts = new List<DashBoardQueryPortModel>();
            if (UsePorts)
            {
                string[] portList = Ports.Split('|');
                foreach (string port in portList)
                {
                    if (!port.Equals(string.Empty))
                    {
                        var pt = new DashBoardQueryPortModel { PortID = int.Parse(port) };
                        listPorts.Add(pt);
                    }
                }
            }

            var utils = new HelpersService();
            var TimeIds = utils.DashBoardConvertToTimeDimension(YearsMonths);

            if (Comments == null)
                Comments = string.Empty;
            if (Description == null)
                Description = string.Empty;

            var qSrv = new QueryService();

            if (string.IsNullOrEmpty(Id))
            {
                Id = string.Empty;
            }

            var postModel = new DashBoardQueryModel
            {
                Id = Id,
                DashItemType = DashItemType,
                DashItem = DashItem,
                Date = DateTime.Now,
                User = GetUserEmail(),
                Name = Name,
                Comments = Comments,
                Description = Description,
                SourceCountryGroup = SourceCountryGrouping,
                TradeFlowType = TradeFlowGrouping,
                MarketCountryGroup = MarketCountryGrouping,
                ProductGroupType = ProductType,
                ProductGroup = ProductGrouping,
                TonnesValuesGroup = ValuesGrouping,
                GroupByMonth = GroupByMonth,
                GroupByQuarter = GroupByQuarter,
                GroupByYear = GroupByYear,
                SourceCountryGEOs = sGeos,
                MarketCountryGEOs = mGeos,
                Products = listProducts,
                TimeIDs = TimeIds,
                Ports = listPorts,
                PortGroup = PortGrouping,
                IncludePorts = UsePorts,
                ProductRegionCode = ComboTariffRegionSelector,
                Product2DigitCode = ComboTariff2DigitSelector

            };

            var usrDashSrv = new UserDashboardService();
            var dModel = await usrDashSrv.GetCurrentDashboardByGUID(RecordId);

            //check index

            if (postModel.DashItem == 1)
            {
                dModel.Items[int.Parse(Id) - 1].Query1 = postModel;
               // dModel.Items[postModel.DashItem - 1].Query1 = postModel;
            }
            if (postModel.DashItem == 2)
            {
                dModel.Items[int.Parse(Id) - 1].Query2 = postModel;
               // dModel.Items[postModel.DashItem - 1].Query2 = postModel;
            }
            if (postModel.DashItem == 3)
            {
                dModel.Items[int.Parse(Id) - 1].Query3 = postModel;
               // dModel.Items[postModel.DashItem - 1].Query3 = postModel;
            }
            if (postModel.DashItem == 4)
            {
                dModel.Items[int.Parse(Id) - 1].Query4 = postModel;
               // dModel.Items[postModel.DashItem - 1].Query4 = postModel;
            }

            await usrDashSrv.UpdateDashBoard(dModel);

            return Json(new { success = true, recordid = RecordId });
        }

        private string GetUserEmail()
        {
            var UserEmail = User.FindFirst(ClaimTypes.Email).Value;
            return UserEmail;
        }

        private string GetUserName()
        {
            var UserName = User.FindFirst(ClaimTypes.Name).Value;
            return UserName;
        }
    }
}
