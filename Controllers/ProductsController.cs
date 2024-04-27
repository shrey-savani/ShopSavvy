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
    public class ProductsController : Controller
    {
        private readonly ToyStoreDBContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ProductsController(ToyStoreDBContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {

            if (HttpContext.Session.GetString("user") != null && !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            var toyStoreDBContext = _context.Products.Include(p => p.Category);
            return View(await toyStoreDBContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (HttpContext.Session.GetString("user") != null && !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {

            if (HttpContext.Session.GetString("user") != null && !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImagePath,Price,CategoryId")] Product product, IFormFile ProductImageFile)
        {

            if (HttpContext.Session.GetString("user") != null && !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            if (ModelState.IsValid)
            {
                if (ProductImageFile == null || ProductImageFile.Length == 0)
                {
                    ModelState.AddModelError("ImageFile", "File not uploaded");
                    ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
                    return View(product);
                }

                string fileName = Path.GetFileName(ProductImageFile.FileName);
                string filePath = Path.Combine(_appEnvironment.WebRootPath, "toys_imgs", fileName);
                product.ImagePath = fileName;

                // file validation
                string extension = Path.GetExtension(fileName);
                if (!IsImage(extension)) // Implement your validation logic here
                {
                    ModelState.AddModelError("ImageFile", "Invalid file. allowed formats (JPG, JPEG, PNG)");
                    ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
                    return View(product);
                }

                // Create uploads folder if it doesn't exist
                if (!Directory.Exists(Path.Combine(_appEnvironment.WebRootPath, "toys_imgs")))
                {
                    Directory.CreateDirectory(Path.Combine(_appEnvironment.WebRootPath, "toys_imgs"));
                }

                // Save the uploaded file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImageFile.CopyToAsync(stream);
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        private bool IsImage(string extension)
        {
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png";
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("user") != null && !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImagePath,Price,CategoryId")] Product product, IFormFile? ProductImageFile)
        {
            if (HttpContext.Session.GetString("user") != null && !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ProductImageFile != null && ProductImageFile.Length != 0)
                    {
                        string fileName = Path.GetFileName(ProductImageFile.FileName);
                        string filePath = Path.Combine(_appEnvironment.WebRootPath, "toys_imgs", fileName);
                        product.ImagePath = fileName;

                        // file validation
                        string extension = Path.GetExtension(fileName);
                        if (!IsImage(extension)) // Implement your validation logic here
                        {
                            ModelState.AddModelError("ImageFile", "Invalid file. allowed formats (JPG, JPEG, PNG)");
                            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
                            return View(product);
                        }

                        // Create uploads folder if it doesn't exist
                        if (!Directory.Exists(Path.Combine(_appEnvironment.WebRootPath, "toys_imgs")))
                        {
                            Directory.CreateDirectory(Path.Combine(_appEnvironment.WebRootPath, "toys_imgs"));
                        }

                        // Save the uploaded file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ProductImageFile.CopyToAsync(stream);
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("user") != null && !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("user") != null && !HttpContext.Session.GetString("user").Equals("Admin"))
            {
                return RedirectToAction("Login", "AdminLogin");
            }
            if (_context.Products == null)
            {
                return Problem("Entity set 'ToyStoreDBContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
