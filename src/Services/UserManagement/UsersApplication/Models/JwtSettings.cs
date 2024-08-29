using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UsersApplication.Models
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpireMinutes { get; set; } = default!;

        public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(SecretKey));
    }
}
