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
}