namespace Basket.API.Features.Basket.DeleteBasket;

//public record GetBasketRequest(string UserName);
public record GetBasketReponse(ShoppingCart ShoppingCart);
public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var query = new GetBasketQuery(userName);
            var result = await sender.Send(query);
            var response = result.Adapt<GetBasketReponse>();
            return Results.Ok(response);
        })
        .WithName("GetBasket")
        .Produces<GetBasketReponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Basket")
        .WithDescription("Get Basket");
    }
}
