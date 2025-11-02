using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductCatalog.Models;

namespace ProductCatalog.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _productsCollection;

    public ProductService(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDataBase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _productsCollection = mongoDataBase.GetCollection<Product>(databaseSettings.Value.ProductsCollectionName);
    }

    // Get all products
    public async Task<List<Product>> GetProductsAsync() => await _productsCollection.Find(_ => true).ToListAsync();

    // Get product by ID
    public async Task<Product?> GetByIdAsync(string id) => await _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    // Create new product
    public async Task CreateAsync(Product newProduct) => await _productsCollection.InsertOneAsync(newProduct);

    // Update existing product
    public async Task UpdateAsync(string id, Product updateProduct) => await _productsCollection.ReplaceOneAsync(x => x.Id == id, updateProduct);

    // Delete product
    public async Task DeleteAsync(string id) => await _productsCollection.DeleteOneAsync(x => x.Id == id);

    // Search product by name or category
    public async Task<List<Product>> SearchAsync(string searchTerm)
    {
        var filter = Builders<Product>.Filter.Or(

            Builders<Product>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
            Builders<Product>.Filter.Regex(x => x.Category, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))

        );

        return await _productsCollection.Find(filter).ToListAsync();
    }
}