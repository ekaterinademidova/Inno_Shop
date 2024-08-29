using UsersApplication.Models;

namespace UsersApplication.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Token Authenticate(User user);
    }
}
