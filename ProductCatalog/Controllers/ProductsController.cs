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
}