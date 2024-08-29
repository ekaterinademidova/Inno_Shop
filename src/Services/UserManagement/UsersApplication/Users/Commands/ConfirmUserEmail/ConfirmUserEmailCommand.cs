namespace UsersApplication.Users.Commands.ConfirmUserEmail
{
    public record ConfirmUserEmailCommand(Guid Token) : ICommand<ConfirmUserEmailResult>;
    public record ConfirmUserEmailResult(bool IsSuccess);
    public class ConfirmUserEmailCommandValidator : AbstractValidator<ConfirmUserEmailCommand>
    {
        public ConfirmUserEmailCommandValidator()
        {
            RuleFor(command => command.Token).NotNull().WithMessage("Token is required.");
        }
    }
}
