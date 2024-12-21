using Microsoft.AspNetCore.Mvc;

namespace Product.Application.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
