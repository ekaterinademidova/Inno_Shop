namespace UsersApplication.Users.Commands.ConfirmUserEmail
{
    public record ConfirmUserEmailCommand(Guid Token) : ICommand<ConfirmUserEmailResult>;
    public record ConfirmUserEmailResult(bool IsSuccess);
    public class ConfirmUserEmailCommandValidator : AbstractValidator<ConfirmUserEmailCommand>
    {
        public ConfirmUserEmailCommandValidator()
        {
            AddRuleForToken();
        }

        private void AddRuleForToken()
        {
            RuleFor(cmd => cmd.Token)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.OperationToken.EmptyCode)
                .WithMessage("Token may not be empty");
        }
    }
}
