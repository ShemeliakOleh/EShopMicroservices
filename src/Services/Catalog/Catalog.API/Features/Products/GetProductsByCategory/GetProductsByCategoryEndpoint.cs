
using Catalog.API.Features.Products.GetProducts;

namespace Catalog.API.Features.Products.GetProductsByCategory;

//public record GetProductsByCategoryRequest();
public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var productsResult = await sender.Send(new GetProductsByCategoryQuery(category));
            return Results.Ok(productsResult.Adapt<GetProductsByCategoryResponse>());
        })
        .WithName("GetProductsByCategory")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products By Category")
        .WithDescription("Get Products By Category");

    }
}
