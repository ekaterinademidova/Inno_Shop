namespace UsersApplication.Users.Commands.UpdateUser
{
    public class UpdateUserHandler(IApplicationDbContext dbContext)
        : ICommandHandler<UpdateUserCommand, UpdateUserResult>
    {
        public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            // update User entity from command object
            var userId = UserId.Of(command.User.Id);
            var user = await dbContext.Users
                .FindAsync([userId], cancellationToken: cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(command.User.Id);
            }

            UpdateUserWithNewValues(user, command.User);

            // save to database
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            // return result
            return new UpdateUserResult(true);
        }

        public void UpdateUserWithNewValues(User user, UserDto userDto)
        {
            user.Update(
                firstName: userDto.FirstName,
                lastName: userDto.LastName,
                email: userDto.Email,
                password: userDto.Password,
                role: userDto.Role
            );
        }
    }
}
