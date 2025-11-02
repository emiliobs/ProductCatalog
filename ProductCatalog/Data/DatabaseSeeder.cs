using ProductCatalog.Models;
using ProductCatalog.Services;

namespace ProductCatalog.Data;

public class DatabaseSeeder
{
    private readonly ProductService _productService;

    public DatabaseSeeder(ProductService productService)
    {
        this._productService = productService;
    }

    public async Task SeedAsync()
    {
        // Check idf database already has products
        var existingProducts = await _productService.GetAllProductsAsyn();

        if (existingProducts.Any())
        {
            Console.WriteLine("Database already contains products. Skipping seed.");
            return;
        }

        Console.WriteLine("Seeding database with sample products.....");

        // Create sample products
        var products = new List<Product>
            {
                new Product
                {
                    Name = "MacBook Pro 16\"",
                    Description = "Powerful laptop with M2 Pro chip, 16GB RAM, and 512GB SSD. Perfect for developers and creative professionals.",
                    Price = 2499.99m,
                    Category = "Electronics",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-30)
                },
                new Product
                {
                    Name = "iPhone 15 Pro",
                    Description = "Latest iPhone with A17 Pro chip, titanium design, and advanced camera system.",
                    Price = 999.99m,
                    Category = "Electronics",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-25)
                },
                new Product
                {
                    Name = "Sony WH-1000XM5 Headphones",
                    Description = "Premium noise-cancelling wireless headphones with exceptional sound quality and 30-hour battery life.",
                    Price = 399.99m,
                    Category = "Electronics",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-20)
                },
                new Product
                {
                    Name = "Samsung 4K Smart TV 55\"",
                    Description = "Stunning 4K QLED display with smart features, HDR support, and built-in streaming apps.",
                    Price = 799.99m,
                    Category = "Electronics",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-18)
                },
                new Product
                {
                    Name = "Nike Air Max 2024",
                    Description = "Comfortable running shoes with responsive cushioning and breathable mesh upper.",
                    Price = 149.99m,
                    Category = "Clothing",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-15)
                },
                new Product
                {
                    Name = "Levi's 501 Original Jeans",
                    Description = "Classic straight-fit jeans made from premium denim. Timeless style for any occasion.",
                    Price = 79.99m,
                    Category = "Clothing",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-12)
                },
                new Product
                {
                    Name = "The North Face Jacket",
                    Description = "Waterproof and breathable outdoor jacket perfect for hiking and camping.",
                    Price = 249.99m,
                    Category = "Clothing",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-10)
                },
                new Product
                {
                    Name = "Instant Pot Duo 7-in-1",
                    Description = "Versatile electric pressure cooker that can replace 7 kitchen appliances.",
                    Price = 89.99m,
                    Category = "Home & Kitchen",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-8)
                },
                new Product
                {
                    Name = "Dyson V15 Vacuum Cleaner",
                    Description = "Powerful cordless vacuum with laser detection and intelligent cleaning modes.",
                    Price = 649.99m,
                    Category = "Home & Kitchen",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-6)
                },
                new Product
                {
                    Name = "KitchenAid Stand Mixer",
                    Description = "Professional-grade stand mixer with 10 speeds and multiple attachments included.",
                    Price = 379.99m,
                    Category = "Home & Kitchen",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new Product
                {
                    Name = "Kindle Paperwhite",
                    Description = "E-reader with high-resolution display, adjustable warm light, and weeks of battery life.",
                    Price = 139.99m,
                    Category = "Books & Media",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-4)
                },
                new Product
                {
                    Name = "Logitech MX Master 3S",
                    Description = "Advanced wireless mouse with ergonomic design and customizable buttons for productivity.",
                    Price = 99.99m,
                    Category = "Accessories",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-3)
                },
                new Product
                {
                    Name = "Mechanical Gaming Keyboard",
                    Description = "RGB backlit mechanical keyboard with Cherry MX switches and programmable macros.",
                    Price = 159.99m,
                    Category = "Accessories",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-2)
                },
                new Product
                {
                    Name = "Fitbit Charge 6",
                    Description = "Advanced fitness tracker with heart rate monitoring, GPS, and sleep tracking.",
                    Price = 159.99m,
                    Category = "Electronics",
                    ImagePath = null,
                    CreatedDate = DateTime.Now.AddDays(-1)
                },
                new Product
                {
                    Name = "Yoga Mat Premium",
                    Description = "Extra thick non-slip yoga mat with carrying strap. Perfect for home workouts.",
                    Price = 39.99m,
                    Category = "Sports & Outdoors",
                    ImagePath = null,
                    CreatedDate = DateTime.Now
                }
            };

        foreach (var product in products)
        {
            await _productService.CreateAsync(product);
            Console.WriteLine($"Added: {product.Name}");
        }

        Console.WriteLine($"Successfully seeded {products.Count} products.");
    }
}