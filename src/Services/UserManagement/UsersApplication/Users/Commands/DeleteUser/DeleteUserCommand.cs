namespace UsersApplication.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserId) : ICommand<DeleteUserResult>;
    public record DeleteUserResult(bool IsSuccess);
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(command => command.UserId).NotEmpty().WithMessage("User ID is required");
        }
    }
}
