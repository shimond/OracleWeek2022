
namespace CatalogApi.Model;

//public record ProductInOnRow(int Id, string Name, double price);

public record Product
{
    public required int Id { get; init; }
    public required string? Name { get; init; } 
    public required double Price  { get; init; }
}
