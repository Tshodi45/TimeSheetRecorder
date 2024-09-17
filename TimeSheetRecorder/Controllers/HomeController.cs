using TimeSheetRecorder.Data;
using TimeSheetRecorder.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace TimeSheetRecorder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly timeSheetRecorderContext _dbContext;
        public HomeController(ILogger<HomeController> logger, timeSheetRecorderContext eSignContext)
        {
            _logger = logger;
            _dbContext = eSignContext;
        }

        public IActionResult Index()
        {

            return View(_dbContext.TimeSheetDetails.Include(x => x.Project));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}