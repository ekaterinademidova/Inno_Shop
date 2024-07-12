namespace UsersAPI.Users.GetUserById
{
    public record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResult>;
    public record GetUserByIdResult(User User);

    internal class GetUserByIdQueryHandler
        (IDocumentSession session, ILogger<GetUserByIdQueryHandler> logger)
        : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
    {
        public async Task<GetUserByIdResult> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetUserByIdQueryHandler.Handle called with {@Query}", query);
            var user = await session.LoadAsync<User>(query.Id, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(query.Id);
            }

            return new GetUserByIdResult(user);
        }
    }
}
