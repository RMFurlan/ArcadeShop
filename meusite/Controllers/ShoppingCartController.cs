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

        MeuSiteContext _storeDB = new MeuSiteContext();
        private ShoppingCart _shoppingCart;
        public ShoppingCartController(MeuSiteContext context, ShoppingCart shoppingCart)
        {
            _storeDB = context;
            _shoppingCart = shoppingCart;
        }
        public IActionResult Index()
        {
            var cart = _shoppingCart.GetCart(this.HttpContext);

            var viewlModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewlModel);
        }

        public IActionResult AddToCart(int id)
        {
            var addedItem = _storeDB.Items
                .Include(i => i.Category)
                .Include(i => i.Producer)
                .FirstOrDefault(i => i.ItemId == id);

            var cart = _shoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedItem);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            _shoppingCart = _shoppingCart.GetCart(this.HttpContext);

            var item = _storeDB.Carts.FirstOrDefaultAsync(i => i.RecordId == id);

            var itemCount = _shoppingCart.RemoveFromCart(id);

            var result = new ShoppingCartRemoveViewModel
            {
                Message = "Item removed from cart successfully.",
                CartTotal = _shoppingCart.GetTotal(),
                CartCount = _shoppingCart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(result);
        }

        public ActionResult CartSummary()
        {
            var cart = _shoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
