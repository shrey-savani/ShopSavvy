using ShopSavvy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace ShopSavvy.Controllers
{
    public class UserSiteController : Controller
    {
        private readonly ToyStoreDBContext _context;

        public UserSiteController(ToyStoreDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("User"))
            {
                return RedirectToAction("UserLogin", "Login");
            }
            List<Product> products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        public IActionResult Cart()
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("User"))
            {
                return RedirectToAction("UserLogin", "Login");
            }
            string cart = HttpContext.Session.GetString("cart");
            Cart user_cart = new Cart();
            if (cart != null)
            {
                user_cart = JsonConvert.DeserializeObject<Cart>(cart);
            }
            return View(user_cart);
        }

        public IActionResult AddToCart(int product_id = 0)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("User"))
            {
                return RedirectToAction("UserLogin", "Login");
            }
            string cart = HttpContext.Session.GetString("cart");
            Cart user_cart = new Cart();
            if (cart != null)
            {
                user_cart = JsonConvert.DeserializeObject<Cart>(cart);
            }
            Product product = _context.Products.Find(product_id);
            user_cart.AddItem(product, 1);
            cart = JsonConvert.SerializeObject(user_cart);
            HttpContext.Session.SetString("cart", cart);
            return RedirectToAction("Cart");
        }

        public IActionResult EditCart(int product_id = 0, int quantity = 0)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("User"))
            {
                return RedirectToAction("UserLogin", "Login");
            }
            string cart = HttpContext.Session.GetString("cart");
            Cart user_cart = new Cart();
            if (cart != null)
            {
                user_cart = JsonConvert.DeserializeObject<Cart>(cart);
            }
            user_cart.UpdateItem(product_id, quantity);
            cart = JsonConvert.SerializeObject(user_cart);
            HttpContext.Session.SetString("cart", cart);
            return RedirectToAction("Cart");
        }

        public IActionResult RemoveFromCart(int product_id = 0)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("User"))
            {
                return RedirectToAction("UserLogin", "Login");
            }
            string cart = HttpContext.Session.GetString("cart");
            Cart user_cart = new Cart();
            if (cart != null)
            {
                user_cart = JsonConvert.DeserializeObject<Cart>(cart);
                Product product = _context.Products.Find(product_id);
                user_cart.RemoveItem(product_id);
                cart = JsonConvert.SerializeObject(user_cart);
                HttpContext.Session.SetString("cart", cart);
            }

            return RedirectToAction("Cart");
        }

        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("User"))
            {
                return RedirectToAction("UserLogin", "Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("User"))
                {
                    return RedirectToAction("UserLogin", "Login");
                }
                int user_id = (int)HttpContext.Session.GetInt32("user_id");
                order.UserId = user_id;
                string cart = HttpContext.Session.GetString("cart");
                Cart user_cart = new Cart();
                if (cart != null)
                {
                    string order_items = "";
                    user_cart = JsonConvert.DeserializeObject<Cart>(cart);
                    foreach (CartProduct cart_pd in user_cart.Products)
                    {
                        order_items += cart_pd.ProductName + "(" + cart_pd.Quantity + "), ";
                    }
                    order.OrderItems = order_items;
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    user_cart.Clear();
                    cart = JsonConvert.SerializeObject(user_cart);
                    HttpContext.Session.SetString("cart", cart);
                    ViewBag.isOrderSuccess = true;
                }
                else
                {
                    ViewBag.isOrderSuccess = false;
                }
                return View("Order_Success");
            }
            return View();
        }
    }
}
