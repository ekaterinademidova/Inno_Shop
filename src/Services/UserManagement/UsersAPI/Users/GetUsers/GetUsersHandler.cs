namespace UsersAPI.Users.GetUsers
{
    public record GetUsersQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetUsersResult>;
    public record GetUsersResult(IEnumerable<User> Users);
    internal class GetUsersQueryHandler
        (IDocumentSession session)
        : IQueryHandler<GetUsersQuery, GetUsersResult>
    {
        public async Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await session.Query<User>()
                .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

            return new GetUsersResult(users);
        }
    }
}

