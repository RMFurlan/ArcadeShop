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
        private readonly ShoppingCart _shoppingCart;

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
            var item = storeDB.Items
                .Include(i => i.Category)
                .Include(i => i.Producer)
                .FirstOrDefault(i => i.ItemId == id);

            if (item != null)
            {
                var itemCount = _shoppingCart.RemoveFromCart(item);

                if (itemCount > 0)
                {
                    var cartTotal = _shoppingCart.GetTotal();
                    var cartCount = _shoppingCart.GetCount();

                    var result = new
                    {
                        ItemCount = itemCount,
                        CartTotal = cartTotal,
                        CartCount = cartCount,
                        Message = "Item removido do carrinho com sucesso."
                    };

                    return Json(result);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }



        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
