using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UsersApplication.Interfaces.ServiceContracts;
using UsersApplication.ValueObjects;

namespace UsersApplication.Services
{
    public class AuthenticationService(JwtSettings jwtSettings) : IAuthenticationService
    {
        public JwtToken Authenticate(User user)
        {
            var token = GenerateToken(user);
            return new JwtToken { Value = token };
        }

        private string GenerateToken(User user)
        {                                     
            var key = jwtSettings.GetSymmetricSecurityKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(jwtSettings.ExpireMinutes);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.ConcatenatedAudiences,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
