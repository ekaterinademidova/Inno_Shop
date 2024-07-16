namespace ProductsAPI.Products.GetProductsByCreator
{
    public record GetProductByCreatorQuery(Guid CreatedByUserId) : IQuery<GetProductByCreatorResult>;
    public record GetProductByCreatorResult(IEnumerable<Product> Products);

    internal class GetProductByCreatorQueryHandler
        (IDocumentSession session)
        : IQueryHandler<GetProductByCreatorQuery, GetProductByCreatorResult>
    {
        public async Task<GetProductByCreatorResult> Handle(GetProductByCreatorQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>()
                .Where(p => p.CreatedByUserId.Equals(query.CreatedByUserId))
                .ToListAsync(cancellationToken);

            return new GetProductByCreatorResult(products);
        }
    }
}
