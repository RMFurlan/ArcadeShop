using Microsoft.AspNetCore.Mvc;
using meusite.Data;
using Microsoft.EntityFrameworkCore;

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
            var Item = storeDB.Items.Find(id);
            return View(Item);
        }
    }
}
