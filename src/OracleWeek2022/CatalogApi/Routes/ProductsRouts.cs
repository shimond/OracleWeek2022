namespace CatalogApi.Routes;
public static class ProductsRouts
{
    public static void MapProducts(this WebApplication app)
    {
        var productsGroup = app.MapGroup("/products")
            .AddEndpointFilter<MyEndPointFilter>()
            .WithTags("Products");

        productsGroup.MapGet("env", ()=> Environment.OSVersion);

        productsGroup.MapGet("{id}", GetById).RequireAuthorization();
        productsGroup.MapGet("", GetAll).CacheOutput();
        productsGroup.MapPost("", AddNewProduct).WithDisplayName("AddNewProduct");
    }

    private static async Task<Ok<Product>> AddNewProduct(Product p, IProductsRepository productsRepository)
    {
        var res = await productsRepository.AddNewProduct(p);
        return TypedResults.Ok(res);
    }
    private static async Task<Ok<List<Product>>> GetAll(IProductsRepository productsRepository)
    {
        var result = await productsRepository.GetAll();
        return TypedResults.Ok(result);
    }
    private static async Task<Results<Ok<Product>, NotFound<int>>> GetById(int id, IProductsRepository productsRepository)
    {
        var result = await productsRepository.GetById(id);
        if (result == null)
        {
            return TypedResults.NotFound(2);
        }
        return TypedResults.Ok(result);
    }
}

public class MyEndPointFilter : IEndpointFilter
{
    private readonly ILogger<MyEndPointFilter> logger;

    public MyEndPointFilter(ILogger<MyEndPointFilter> logger)
    {
        this.logger = logger;
    }
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var arguments = System.Text.Json.JsonSerializer.Serialize(context.Arguments);
        this.logger.LogInformation("Before method", arguments, context.HttpContext.Request.GetDisplayUrl());
        //before
        var res = await next(context);
        this.logger.LogInformation("After method");
        //after
        return res;
    }
}

