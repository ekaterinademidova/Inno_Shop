using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UsersApplication.Interfaces.Services;
using UsersApplication.Models;

namespace UsersInfrastucture.Services
{
    public class AuthenticationService(JwtSettings jwtSettings) : IAuthenticationService
    {
        public Token Authenticate(User user)
        {
            var token = GenerateToken(user);
            return new Token { Value = token };
        }

        private string GenerateToken(User user)
        {                                     
            var key = jwtSettings.GetSymmetricSecurityKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(jwtSettings.ExpireMinutes);
            var claims = new List<Claim>
            {
                new("userId", user.Id.Value.ToString()), // change
                new("userEmail", user.Email),
                new("userRole", user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
