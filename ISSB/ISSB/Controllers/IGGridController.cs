using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ISSB.Controllers
{
    public class IGGridController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var srv = new PortsService();
            var model = await srv.GetAllPorts();
            IQueryable data = model.AsQueryable();

            return View(data);
        }
    }
}
