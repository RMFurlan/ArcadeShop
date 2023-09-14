using meusite.Data;
using meusite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shoppingstore.Models;

namespace MeuSite.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        MeuSiteContext storeDB = new MeuSiteContext();
        const string PromoCode = "50";
        public CheckoutController(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
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


                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();

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
            bool isValid = storeDB.Orders.Any(
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
