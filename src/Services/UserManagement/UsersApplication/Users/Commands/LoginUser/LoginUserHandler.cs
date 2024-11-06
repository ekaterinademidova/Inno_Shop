using UsersApplication.Interfaces.ServiceContracts;

namespace UsersApplication.Users.Commands.LoginUser
{
    public class LoginUserHandler
        (IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        : ICommandHandler<LoginUserCommand, LoginUserResult>
    {
        public async Task<LoginUserResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.User
                .GetAsync(filter: u => u.Email == command.Email, cancellationToken: cancellationToken)
                ?? throw new UserNotFoundException(command.Email);

            if (user.Password != command.Password) 
                throw new UserInvalidDataException("Invalid password.");

            var token = authenticationService.Authenticate(user);

            // return result
            return new LoginUserResult(token);
        }
    }
}
