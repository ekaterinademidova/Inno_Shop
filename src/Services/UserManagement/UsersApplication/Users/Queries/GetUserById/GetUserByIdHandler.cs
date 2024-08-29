using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UsersApplication.Users.Queries.GetUserById
{
    internal class GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
        : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
    {
        public async Task<GetUserByIdResult> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            // get user by id
            var userId = UserId.Of(query.Id);
            var user = await unitOfWork.User
                .GetAsync(filter: u => u.Id == userId, cancellationToken: cancellationToken)
                ?? throw new UserNotFoundException(query.Id);

            // return result
            return new GetUserByIdResult(user.ToUserDto());
        }
    }
}
