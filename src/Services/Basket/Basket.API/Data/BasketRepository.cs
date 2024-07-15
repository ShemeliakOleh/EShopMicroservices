using Basket.API.Exceptions;
using Marten;

namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var basket = await session.LoadAsync<ShoppingCart>(userName);
        if(basket == null)
        {
            throw new BasketNotFoundException(userName);
        }
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket)
    {
        session.Store(basket);
        await session.SaveChangesAsync();

        return basket;
    }
    public async Task<bool> DeleteBasket(string userName)
    {
        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync();

        return true;
    }
}
