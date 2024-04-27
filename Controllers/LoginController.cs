using ShopSavvy.Models;
using Microsoft.AspNetCore.Mvc;

namespace ShopSavvy.Controllers
{
    public class LoginController : Controller
    {
        private readonly ToyStoreDBContext _context;
        public LoginController(ToyStoreDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("UserLogin");
        }

        public IActionResult UserLogin()
        {
            return View("UserLogin");
        }

        [HttpPost]
        public IActionResult UserLogin(User user)
        {
            if (ModelState.IsValid)
            {
                User dbUser = _context.Users.Where(u => u.Role.Equals("User") && u.Username.Equals(user.Username) && u.Password.Equals(user.Password)).FirstOrDefault();
                if (dbUser != null)
                {
                    //successful login
                    HttpContext.Session.SetString("user", "User");
                    HttpContext.Session.SetString("username", user.Username);
                    HttpContext.Session.SetInt32("user_id", dbUser.Id);
                    return RedirectToAction("Index", "UserSite");
                }
                else
                {
                    ViewBag.Error = "Invalid username or password.";
                    return View();
                }
            }
            return View();
        }

        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            if (ModelState.IsValid)
            {
                User userExists = _context.Users.Where(u => u.Role.Equals("User") && u.Username.Equals(user.Username)).FirstOrDefault();
                if(userExists!=null)
                {
                    ViewBag.Error = "username already in use.";
                    return View();
                }
                user.Role = "User";
                _context.Users.Add(user);
                _context.SaveChanges();
                user.Username = "";
                return RedirectToAction("UserLogin");
            }
            return View();
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(User user)
        {
            if (ModelState.IsValid)
            {
                User dbUser = _context.Users.Where(u => u.Role.Equals("Admin") && u.Username.Equals(user.Username) && u.Password.Equals(user.Password)).FirstOrDefault();
                if (dbUser != null)
                {
                    //successful login
                    HttpContext.Session.SetString("user", "Admin");
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ViewBag.Error = "Invalid username or password.";
                    return View();
                }
            }
            return View();
        }

        public IActionResult AdminLogout()
        {
            HttpContext.Session.Clear();
            return View("AdminLogin");
        }

        public IActionResult UserLogout()
        {
            HttpContext.Session.Clear();
            return View("UserLogin");
        }
    }
}
