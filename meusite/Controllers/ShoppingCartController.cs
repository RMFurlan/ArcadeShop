using meusite.Data;
using MeuSite.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoppingstore.Models;
using System.Text.Encodings.Web;

namespace MeuSite.Controllers
{
    public class ShoppingCartController : Controller
    {

        MeuSiteContext storeDB = new MeuSiteContext();

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewlModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewlModel);
        }

        public IActionResult AddToCart(int id)
        {
            var addedItem = storeDB.Items
                .Include(i => i.Category)
                .Include(i => i.Producer)
                .FirstOrDefault(i => i.ItemId == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedItem);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string itemName = storeDB.Carts
                .Single(item => item.RecordId == id).Item.Title;

            int itemCount = cart.RemoveFromCart(id);


            var results = new ShoppingCartRemoveViewModel
            {
                Message = HtmlEncoder.Default.Encode(itemName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
