
using CatalogApi.Data;
using Microsoft.Extensions.Options;

namespace CatalogApi.Services;

public class ProductsRepository : IProductsRepository
{
    private readonly CatalogDb db;
    private readonly RedisConfig configuration;

    public ProductsRepository(CatalogDb db, IOptions<RedisConfig>  configuration)
    {
        this.db = db;
        this.configuration = configuration.Value;
    }
    public async Task<Product> AddNewProduct(Product p)
    {
        this.db.Products.Add(p);
        await this.db.SaveChangesAsync();
        return p;
    }

    public async Task<List<Product>> GetAll()
    {
        var items = await this.db.Products.ToListAsync();
        return items;
    }

    public async Task<Product?> GetById(int id)
    {
        var item = await this.db.Products.SingleOrDefaultAsync(x=>x.Id == id);
        return item;
    }

    public async Task<Product> Update(Product p)
    {
        this.db.Products.Update(p);
        await this.db.SaveChangesAsync();
        return p;
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}
