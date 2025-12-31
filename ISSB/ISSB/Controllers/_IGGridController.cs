using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Data.Models;
using System.Linq;

namespace ISSB.Controllers
{
    public class _IGGridController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srvPorts = new PortsService();
            var model = await srvPorts.GetAllPorts();
            IQueryable data = model.AsQueryable();

            return View(data);

        }

    }
}
