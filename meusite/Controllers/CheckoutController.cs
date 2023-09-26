using meusite.Data;
using meusite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoppingstore.Models;

namespace MeuSite.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly MeuSiteContext _context;
        const string PromoCode = "50";
        public CheckoutController(ShoppingCart shoppingCart, MeuSiteContext context)
        {
            _shoppingCart = shoppingCart;
            _context = context;
        }

        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Payment(IFormCollection values)
        {
            var order = new Order();
            TryUpdateModelAsync(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    var cart = _shoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete", new { id = order.OrderId });
                }
            }
            catch
            {
                return View(order);
            }
        }
        public IActionResult Complete(int id)
        {
            bool isValid = _context.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View("Complete", id);
            }
            else
            {
                return View("Error");
            }
        }
    }

}
