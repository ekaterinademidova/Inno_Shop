namespace UsersAPI.Users.UpdateUser
{
    public record UpdateUserCommand(Guid Id, string Name)
        : ICommand<UpdateUserResult>;
    public record UpdateUserResult(bool IsSuccess);
    internal class UpdateUserCommandHandler
        (IDocumentSession session, ILogger<UpdateUserCommandHandler> logger)
        : ICommandHandler<UpdateUserCommand, UpdateUserResult>
    {
        public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateUserCommandHandler.Handle called with {@Command}", command);
            var user = await session.LoadAsync<User>(command.Id, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(command.Id);
            }

            user.Name = command.Name;

            session.Update(user);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateUserResult(true);
        }
    }
}
