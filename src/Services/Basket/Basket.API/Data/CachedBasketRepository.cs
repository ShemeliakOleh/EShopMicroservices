using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{

    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var cachedBasket = await cache.GetStringAsync(userName);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);
        }

        var basket = await repository.GetBasket(userName);

        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket));

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket)
    {
        await repository.StoreBasket(basket);

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

        return basket;
    }
    public async Task<bool> DeleteBasket(string userName)
    {
        var result = await repository.DeleteBasket(userName);

        if (result)
        {
            await cache.RemoveAsync(userName);
        }
        
        return result;
    }
}
