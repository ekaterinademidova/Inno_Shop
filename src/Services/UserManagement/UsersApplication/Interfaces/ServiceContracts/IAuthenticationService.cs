using UsersApplication.ValueObjects;

namespace UsersApplication.Interfaces.ServiceContracts
{
    public interface IAuthenticationService
    {
        JwtToken Authenticate(User user);
    }
}
