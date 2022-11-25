
namespace CatalogApi.Data;

public class CatalogDb : DbContext
{
    public DbSet<Product> Products { get; set; }


	public CatalogDb(DbContextOptions<CatalogDb> options) : base(options)
	{

	}
}
