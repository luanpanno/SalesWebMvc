using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly ILogger<SalesRecordsController> _logger;
        private readonly SalesRecordsService _salesRecordsService;

        public SalesRecordsController(ILogger<SalesRecordsController> logger, SalesRecordsService salesRecordsService)
        {
            _logger = logger;
            _salesRecordsService = salesRecordsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        async public Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordsService.FindByDateAsync(minDate, maxDate);

            return View(result);
        }

        public IActionResult GroupingSearch()
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