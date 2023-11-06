using Microsoft.AspNetCore.Mvc;

namespace DimWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
