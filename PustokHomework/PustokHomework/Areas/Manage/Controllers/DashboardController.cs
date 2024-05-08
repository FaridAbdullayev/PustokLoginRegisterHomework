using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PustokHomework.Areas.Manage.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        [Area("manage")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
