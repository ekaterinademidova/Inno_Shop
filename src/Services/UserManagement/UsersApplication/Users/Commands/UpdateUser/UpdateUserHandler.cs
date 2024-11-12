namespace UsersApplication.Users.Commands.UpdateUser
{
    public class UpdateUserHandler(IUnitOfWork unitOfWork)
        : ICommandHandler<UpdateUserCommand, UpdateUserResult>
    {
        public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.User.Id);
            var user = await unitOfWork.User
                .GetAsync(filter: u => u.Id == userId, cancellationToken: cancellationToken)
                ?? throw new UserNotFoundException(command.User.Id);

            UpdateUserWithNewValues(user, command.User);

            unitOfWork.User.Update(user);
            await unitOfWork.SaveAsync(cancellationToken);

            return new UpdateUserResult(true);
        }

        public static void UpdateUserWithNewValues(User user, UserDto userDto)
        {
            user.Update(
                firstName: userDto.FirstName,
                lastName: userDto.LastName,
                email: userDto.Email,
                password: userDto.Password
            );
        }
    }
}
