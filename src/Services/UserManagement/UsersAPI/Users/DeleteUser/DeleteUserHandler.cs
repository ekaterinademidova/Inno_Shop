namespace UsersAPI.Users.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : ICommand<DeleteUserResult>;
    public record DeleteUserResult(bool IsSuccess);
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("User ID is required");
        }
    }
    internal class DeleteUserCommandHandler
        (IDocumentSession session)
        : ICommandHandler<DeleteUserCommand, DeleteUserResult>
    {
        public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            session.Delete<User>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteUserResult(true);
        }
    }
}
