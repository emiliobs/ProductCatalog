using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using ProductCatalog.Services;

namespace ProductCatalog.Controllers;

public class ProductsController : Controller
{
    private readonly ProductService _productService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductsController(ProductService productService, IWebHostEnvironment webHostEnvironment)
    {
        this._productService = productService;
        this._webHostEnvironment = webHostEnvironment;
    }

    // GET: Products
    public async Task<IActionResult> Index(string searchTerm)
    {
        List<Product> products;

        if (!string.IsNullOrEmpty(searchTerm))
        {
            products = await _productService.SearchAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
        }
        else
        {
            products = await _productService.GetAllProductsAsyn();
        }

        return View(products);
    }

    // GET: Products/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadFolder); // Ensure directory exist

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                product.ImagePath = "/images/products/" + uniqueFileName;
            }

            await _productService.CreateAsync(product);
            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, Product product, IFormFile? imageFile)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct is null)
            {
                return NotFound();
            }

            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(existingProduct.ImagePath))
                {
                    var oldImageProduct = Path.Combine(_webHostEnvironment.WebRootPath, existingProduct.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImageProduct))
                    {
                        System.IO.File.Delete(oldImageProduct);
                    }
                }

                // Upload new image
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                product.ImagePath = $"/images/products/{uniqueFileName}";
            }
            else
            {
                // Keep existing image
                product.ImagePath = existingProduct.ImagePath;
            }

            product.CreatedDate = existingProduct.CreatedDate;
            await _productService.UpdateAsync(id, product);
            TempData["SuccessMessage"] = "Product update successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET Detail Products/5
    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var product = await _productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var product = await _productService.GetByIdAsync(id);
        if (id is null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Porducts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirm(string id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product != null)
        {
            // Delete imageb file if exist
            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _productService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Product deleted successfully!";
        }

        return RedirectToAction(nameof(Index));
    }
}