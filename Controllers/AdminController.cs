using Microsoft.AspNetCore.Mvc;

namespace JobPortalWeb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
