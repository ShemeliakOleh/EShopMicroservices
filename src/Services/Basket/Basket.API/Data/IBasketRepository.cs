namespace Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetBasket(string userName);
    Task<ShoppingCart> StoreBasket(ShoppingCart basket);
    Task<bool> DeleteBasket(string userName);
}
