using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UsersApplication.Users.Queries.GetUserById
{
    internal class GetUserByIdQueryHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
    {
        public async Task<GetUserByIdResult> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            // get user by id
            var userId = UserId.Of(query.Id);
            var user = await dbContext.Users
                .FindAsync([userId], cancellationToken: cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(query.Id);
            }

            // return result
            return new GetUserByIdResult(user.ToUserDto());
        }
    }
}
