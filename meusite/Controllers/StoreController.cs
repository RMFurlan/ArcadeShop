using Microsoft.AspNetCore.Mvc;
using meusite.Data;
using Microsoft.EntityFrameworkCore;
using meusite.Models;

namespace meusite.Controllers
{
    public class StoreController : Controller
    {
        MeuSiteContext storeDB = new MeuSiteContext();
        public IActionResult Index()
        {
            var categories = storeDB.Categories.ToList();

            return View(categories);
        }
        public IActionResult Browse(string category)
        {
            var categoryModel = storeDB.Categories.Include("Items")
                .Single(c => c.Name == category);
            return View(categoryModel);
        }
        public IActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Item =  storeDB.Items
                .Include(i => i.Category)
                .Include(i => i.Producer)
                .FirstOrDefault(i => i.ItemId == id);
            if (Item == null)
            {
                return NotFound();
            }
            return View(Item);
        }
    }
}
