namespace UsersApplication.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler(IUnitOfWork unitOfWork)
        : IQueryHandler<GetUsersQuery, GetUsersResult>
    {
        public async Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            // get users with pagination
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var allUsers = unitOfWork.User.GetAll();

            //var totalCount = await allUsers.CountAsync(cancellationToken);
            var totalCount = await unitOfWork.User.GetTotalCountAsync(cancellationToken);

            var users = await allUsers
                .AsNoTracking()
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            // return result
            return new GetUsersResult(
                new PaginatedResult<UserDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    users.ToUserDtoList()
                )
            );
        }
    }
}

