using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LuduStack.Web.Controllers
{
    public class EmployerController : Controller
    {
        [Route("employer")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
