using ProductsAPI.Enums;

namespace ProductsAPI.Interfaces.ServiceContracts;

public interface IApiUserService
{
    Guid GetUserId();
    string GetUserEmail();
    UserRole GetUserRole();
}