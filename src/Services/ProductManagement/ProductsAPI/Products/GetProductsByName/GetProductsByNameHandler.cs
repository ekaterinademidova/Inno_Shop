namespace ProductsAPI.Products.GetProductsByName
{
    public record GetProductByNameQuery(string Name) : IQuery<GetProductByNameResult>;
    public record GetProductByNameResult(IEnumerable<Product> Products);

    internal class GetProductByNameQueryHandler
        (IDocumentSession session, ILogger<GetProductByNameQueryHandler> logger)
        : IQueryHandler<GetProductByNameQuery, GetProductByNameResult>
    {
        public async Task<GetProductByNameResult> Handle(GetProductByNameQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByNameQueryHandler.Handle called with {@Query}", query);
            var products = await session.Query<Product>()
                .Where(p => p.Name.Contains(query.Name))
                .ToListAsync(cancellationToken);

            return new GetProductByNameResult(products);
        }
    }
}
