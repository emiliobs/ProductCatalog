using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Services;

namespace ProductCatalog
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

            builder.Services.AddSingleton<ProductService>();
            builder.Services.AddSingleton<DatabaseSeeder>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ═══════════════════════════════════════════════════════
            // SEED DATABASE - Add this entire section
            // ═══════════════════════════════════════════════════════

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var seeder = services.GetRequiredService<DatabaseSeeder>();
                    await seeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding database: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Products}/{action=Index}/{id?}");

            app.Run();
        }
    }
}