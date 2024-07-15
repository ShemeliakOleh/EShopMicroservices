using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using MediatR;

namespace Basket.API.Features.Basket.DeleteBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart ShoppingCart);
public class GetBasketQueryHandler(IBasketRepository basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var shoppingCart = await basketRepository.GetBasket(query.UserName);
        return new GetBasketResult(shoppingCart!);
    }
}
