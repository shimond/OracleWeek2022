namespace CatalogApi.Contracts;

public interface IProductsRepository
{
    Task<List<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task<Product> Update(Product p);
    Task Delete(int id);
    Task<Product> AddNewProduct(Product p);

}
