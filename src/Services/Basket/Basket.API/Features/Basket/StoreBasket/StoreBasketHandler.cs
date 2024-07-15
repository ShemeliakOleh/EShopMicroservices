using Basket.API.Data;
using BuildingBlocks.CQRS;
using Discount.Grpc;
using FluentValidation;

namespace Basket.API.Features.Basket.DeleteBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName cannot be empty");
    }
}
public class StoreBasketCommandHandler
    (IBasketRepository basketRepository, DiscountProtoService.DiscountProtoServiceClient discountService) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeducatDiscount(command.Cart);
        var shoppingCart = await basketRepository.StoreBasket(command.Cart);

        var result = new StoreBasketResult(shoppingCart.UserName);
        return result;
    }

    private async Task DeducatDiscount(ShoppingCart shoppingCart)
    {
        foreach(var cartItem in shoppingCart.Items)
        {
            var coupon = await discountService.GetDiscountAsync(new GetDiscountRequest { ProductName = cartItem.ProductName });
            
            cartItem.Price -= coupon.Amount;
        }
    }
}
