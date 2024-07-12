namespace ProductsAPI.Products.GetProductsByCreator
{
    public record GetProductByCreatorQuery(Guid CreatedByUserId) : IQuery<GetProductByCreatorResult>;
    public record GetProductByCreatorResult(IEnumerable<Product> Products);

    internal class GetProductByCreatorQueryHandler
        (IDocumentSession session, ILogger<GetProductByCreatorQueryHandler> logger)
        : IQueryHandler<GetProductByCreatorQuery, GetProductByCreatorResult>
    {
        public async Task<GetProductByCreatorResult> Handle(GetProductByCreatorQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByCreatorQueryHandler.Handle called with {@Query}", query);
            var products = await session.Query<Product>()
                .Where(p => p.CreatedByUserId.Equals(query.CreatedByUserId))
                .ToListAsync(cancellationToken);

            return new GetProductByCreatorResult(products);
        }
    }
}
