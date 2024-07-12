namespace UsersAPI.Users.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : ICommand<DeleteUserResult>;
    public record DeleteUserResult(bool IsSuccess);
    internal class DeleteUserCommandHandler
        (IDocumentSession session, ILogger<DeleteUserCommandHandler> logger)
        : ICommandHandler<DeleteUserCommand, DeleteUserResult>
    {
        public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);
            session.Delete<User>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteUserResult(true);
        }
    }
}
