using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly ILogger<SellersController> _logger;

        public SellersController(ILogger<SellersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}