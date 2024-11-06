namespace UsersApplication.Users.Commands.SeekPasswordReset
{
    public record SeekPasswordResetCommand(string Email) : ICommand<SeekPasswordResetResult>;
    public record SeekPasswordResetResult(bool IsSuccess);
    public class SeekPasswordResetCommandValidator : AbstractValidator<SeekPasswordResetCommand>
    {
        public SeekPasswordResetCommandValidator()
        {
            AddRuleForEmail();
        }

        private void AddRuleForEmail()
        {
            RuleFor(cmd => cmd.Email)
                .EmailAddress()
                .WithErrorCode(DomainErrorCodes.User.InvalidEmail)
                .WithMessage("Email is not a valid email address")
                .MaximumLength(MaxLengths.User.Email)
                .WithErrorCode(DomainErrorCodes.User.EmailExceedsMaxLength)
                .WithMessage($"Email may not be longer than {MaxLengths.User.Email} characters");
        }
    }
}
