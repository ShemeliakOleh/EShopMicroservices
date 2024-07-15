
using Catalog.API.Features.Products.CreateProduct;
using Catalog.API.Models;

namespace Catalog.API.Features.Products.GetProductById;

//public record GetProductByIdRequest();
public record GetProductByIdResponse(Product Product);
public class GetProductByIdEndpoint() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var productResult = await sender.Send(new GetProductByIdQuery(id));
            var productResponse = productResult.Adapt<GetProductByIdResponse>();
            return Results.Ok(productResponse);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}
