using meusite.Data;
using meusite.Models;
using meusite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace meusite.Controllers
{
    public class HomeController : Controller
    {
        private readonly MeuSiteContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MeuSiteContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var companies = _context.Producers.ToList();
            var gamesByCompany = new Dictionary<Producer, List<Item>>();

            foreach (var company in companies)
            {
                var games = _context.Items.Where(item => item.ProducerId == company.ProducerId).ToList();
                gamesByCompany.Add(company, games);
            }

            return View(gamesByCompany);
        }

        public IActionResult About()
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