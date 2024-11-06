using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UsersApplication.Interfaces.ServiceContracts;

namespace UsersApplication.Services;

public sealed class ApiUserService(IHttpContextAccessor httpContextAccessor) : IApiUserService
{
    public Guid GetUserId()
    {
        var userIdClaim = (httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier))
            ?? throw new ArgumentException("User ID not found in token.");

        if (Guid.TryParse(userIdClaim.Value, out var userId)) return userId;

        throw new ArgumentException("Could not parse User ID to Guid.");
    }

    public string GetUserEmail()
    {
        var userEmailClaim = (httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email))
            ?? throw new ArgumentException("User Email not found in token.");

        if (!string.IsNullOrWhiteSpace(userEmailClaim.Value)) return userEmailClaim.Value;

        throw new ArgumentException("User Email is empty.");
    }

    public UserRole GetUserRole()
    {
        var userRoleClaim = (httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role))
            ?? throw new ArgumentException("User Role not found in token.");

        if (Enum.TryParse(userRoleClaim.Value, out UserRole userRole)) return userRole;

        throw new ArgumentException("Could not parse User Role.");
    }
}