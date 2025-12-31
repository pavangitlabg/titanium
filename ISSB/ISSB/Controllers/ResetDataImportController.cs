using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;
using Data.Models;
using ISSB.Helpers;
using ISSB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ISSB.Controllers
{
    public class ResetDataImportController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            if(DataImportService.Instance != null)
            {
                var dataSrv = DataImportService.Instance;
                if(dataSrv.semaphoreReadingZIP.CurrentCount == 0)
                     dataSrv.semaphoreReadingZIP.Release();
                if (dataSrv.semaphoreValidation.CurrentCount == 0)
                    dataSrv.semaphoreValidation.Release();
            }
     
            return View();

        }
    }
}
