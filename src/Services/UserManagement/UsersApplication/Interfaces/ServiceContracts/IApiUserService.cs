namespace UsersApplication.Interfaces.ServiceContracts;

public interface IApiUserService
{
    Guid GetUserId();    
    string GetUserEmail();
    UserRole GetUserRole();
}