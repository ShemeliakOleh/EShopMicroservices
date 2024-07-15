using BuildingBlocks.CQRS;
using Marten.Linq.SoftDeletes;

namespace Catalog.API.Features.Products.UpdateProduct;

public record UpdateProductCommand(Product Product) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Id is empty");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name cannot be empty");
        RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category should be specified");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
    }
}
public class UpdateProductHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Product.Id, cancellationToken);
        if(product == null)
        {
            throw new ProductNotFoundException(command.Product.Id);
        }

        session.Update(command.Product);
        await session.SaveChangesAsync();

        return new UpdateProductResult(true);
    }
}
