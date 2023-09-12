using meusite.Data;
using meusite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace shoppingstore.Models
{
    public class ShoppingCart
    {
        MeuSiteContext _storeDB = new MeuSiteContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        public ShoppingCart GetCart(HttpContext context)
        {   
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        
        public ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Item item)
        {

            var cartItem = _storeDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ItemId == item.ItemId);

            if (cartItem == null)
            {

                cartItem = new Cart
                {
                    ItemId = item.ItemId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                _storeDB.Carts.Add(cartItem);
            }
            else
            {

                cartItem.Count++;
            }

            _storeDB.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {

            var cartItem = _storeDB.Carts.Single(cart => cart.CartId == ShoppingCartId && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _storeDB.Carts.Remove(cartItem);
                }
                _storeDB.SaveChangesAsync();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = _storeDB.Carts.Where(
                cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                _storeDB.Carts.Remove(cartItem);
            }

            _storeDB.SaveChangesAsync();
        }
        public List<Cart> GetCartItems()
        {
            return _storeDB.Carts.Where(
                cart => cart.CartId == ShoppingCartId)
                .Include(i => i.Item)
                .ToList();
        }
        public int GetCount()
        {

            int? count = (from cartItems in _storeDB.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {

            var total = (from cartItems in _storeDB.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Item.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ItemId = item.ItemId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Item.Price,
                    Quantity = item.Count
                };

                orderTotal += (item.Count * item.Item.Price);

                _storeDB.OrderDetails.Add(orderDetail);

            }

            order.Total = orderTotal;


            _storeDB.SaveChanges();

            EmptyCart();

            return order.OrderId;
        }

        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {

                    var tempCartId = Guid.NewGuid();

                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }
            return context.Session.GetString(CartSessionKey).ToString();
        }
        public void MigrateCart(string Email)
        {
            var shoppingCart = _storeDB.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = Email;
            }
            _storeDB.SaveChanges();
        }


    }
}