namespace UsersApplication.Users.Queries.GetUsers
{
    public record GetUsersQuery(PaginationRequest PaginationRequest) : IQuery<GetUsersResult>;
    public record GetUsersResult(PaginatedResult<UserDto> Users);
}
