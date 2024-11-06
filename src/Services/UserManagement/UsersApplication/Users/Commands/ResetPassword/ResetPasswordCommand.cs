namespace UsersApplication.Users.Commands.ResetPassword
{
    public record ResetPasswordCommand(Guid Token, string NewPassword) : ICommand<ResetPasswordResult>;
    public record ResetPasswordResult(bool IsSuccess);
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            AddRuleForToken();
            AddRuleForNewPassword();
        }

        private void AddRuleForToken()
        {
            RuleFor(cmd => cmd.Token)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.OperationToken.EmptyCode)
                .WithMessage("Token may not be empty");
        }

        private void AddRuleForNewPassword()
        {
            RuleFor(cmd => cmd.NewPassword)
                .NotEmpty()
                .WithErrorCode(DomainErrorCodes.User.EmptyPassword)
                .WithMessage("Password may not be empty")
                .MinimumLength(8)
                .WithErrorCode(DomainErrorCodes.User.ShortPassword)
                .WithMessage($"Password may not be shorter than {8} characters")
                .MaximumLength(MaxLengths.User.Password)
                .WithErrorCode(DomainErrorCodes.User.LongPassword)
                .WithMessage($"Password may not be longer than {MaxLengths.User.Password} characters");
        }
    }
}
