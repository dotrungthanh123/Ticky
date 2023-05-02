using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Ticky.Data;
using Ticky.Models;

namespace Ticky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TickyContext _context;

        public HomeController(ILogger<HomeController> logger, TickyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var TickyContext = _context.Events.Include(a => a.Category).Include(a => a.Retailer);
            ViewBag.Categories = _context.Categories.ToList();
            return View(await TickyContext.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}