namespace UsersAPI.Users.CreateUser
{
    public record CreateUserCommand(string Name)
        : ICommand<CreateUserResult>;
    public record CreateUserResult(Guid Id);
    internal class CreateUserCommandHandler
        (IDocumentSession session)
        : ICommandHandler<CreateUserCommand, CreateUserResult>
    {
        public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            // create User entity from command object
            var user = new User
            {
                Name = command.Name
            };

            // save to database
            session.Store(user);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateUserResult result
            return new CreateUserResult(user.Id);
        }
    }
}
