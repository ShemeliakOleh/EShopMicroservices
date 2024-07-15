namespace Catalog.API.Features.Products.GetProducts;
public record GetProductsRequst(int? PageNumber, int? PageSize);
public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products",
            async ([AsParameters] GetProductsRequst request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();

                var products = await sender.Send(query);

                var productsResponse = products.Adapt<GetProductsResponse>();

                return Results.Ok(productsResponse);
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }
}
