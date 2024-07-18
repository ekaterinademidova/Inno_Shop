namespace UsersAPI.Users.CreateUser
{
    public record CreateUserCommand(string Name, string Email)
        : ICommand<CreateUserResult>;
    public record CreateUserResult(Guid Id);
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        }
    }
    internal class CreateUserCommandHandler
        (IDocumentSession session)
        : ICommandHandler<CreateUserCommand, CreateUserResult>
    {
        public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            // create User entity from command object
            var user = new User
            {
                Name = command.Name,
                Email = command.Email,
            };

            // save to database
            session.Store(user);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateUserResult result
            return new CreateUserResult(user.Id);
        }
    }
}
