namespace UsersApplication.Users.Commands.CreateUser
{
    public class CreateUserHandler(IApplicationDbContext dbContext)
        : ICommandHandler<CreateUserCommand, CreateUserResult>
    {
        public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            // create User entity from command object
            var user = CreateNewUser(command.User);

            // save to database
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            // return result
            return new CreateUserResult(user.Id.Value);
        }

        private User CreateNewUser(UserDto userDto)
        {
            var newUser = User.Create(
                id: UserId.Of(Guid.NewGuid()),
                firstName: userDto.FirstName,
                lastName: userDto.LastName,
                email: userDto.Email,
                password: userDto.Password,
                role: userDto.Role
            );

            return newUser;
        }
    }
}
