using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopSavvy.Models;

namespace ShopSavvy.Controllers
{
    public class UsersController : Controller
    {
        private readonly ToyStoreDBContext _context;

        public UsersController(ToyStoreDBContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            return _context.Users != null ?
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'ToyStoreDBContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Role")] User user)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Role")] User user)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("user") == null || !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            if (_context.Users == null)
            {
                return Problem("Entity set 'ToyStoreDBContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
