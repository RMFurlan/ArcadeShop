using meusite.Data;
using meusite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeuSite.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private MeuSiteContext _context;

        public InventoryController(MeuSiteContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            var username = User.Identity.Name;
            var purchasedItems = _context.OrderDetails
                .Where(od => od.Order.Username == username)
                .Include(od => od.Item)
                .AsEnumerable()
                .DistinctBy(od => od.ItemId)
                .ToList();

            return View(purchasedItems);
        }
    }
}
