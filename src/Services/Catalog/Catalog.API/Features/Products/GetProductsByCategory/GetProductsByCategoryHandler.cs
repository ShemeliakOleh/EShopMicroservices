using BuildingBlocks.CQRS;

namespace Catalog.API.Features.Products.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category): IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);
public class GetProductsByCategoryHandler(IDocumentSession session) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = session.Query<Product>().Where(product => product.Category.Contains(query.Category));
        return new GetProductsByCategoryResult(products);
    }
}
