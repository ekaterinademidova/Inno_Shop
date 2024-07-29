namespace UsersApplication.Users.Commands.CreateUser
{
    public record CreateUserCommand(UserDto User)
        : ICommand<CreateUserResult>;
    public record CreateUserResult(Guid Id);
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(command => command.User.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .Length(2, 100).WithMessage("FirstName must be between 2 and 100 characters");
            RuleFor(command => command.User.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .Length(2, 100).WithMessage("LastName must be between 2 and 100 characters");
            RuleFor(command => command.User.Email)
                .NotEmpty().WithMessage("Email is required")
                .Length(2, 255).WithMessage("Email must be between 2 and 255 characters");
            RuleFor(command => command.User.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(2, 100).WithMessage("Password must be between 2 and 100 characters");
        }
    }
}
