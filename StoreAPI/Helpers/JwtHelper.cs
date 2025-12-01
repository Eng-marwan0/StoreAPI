using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreAPI.Helpers
{
    public static class JwtHelper
    {
        // دالة أساسية لتوليد توكن لأي نوع مستخدم (User / Admin)
        public static string GenerateToken(
            int id,
            string fullName,
            string email,
            string role,
            string key,
            string issuer,
            string audience,
            int expireMinutes)
        {
            var claims = new List<Claim>
            {
                new Claim("id", id.ToString()),
                new Claim(ClaimTypes.Name, fullName ?? string.Empty),
                new Claim(ClaimTypes.Email, email ?? string.Empty),
                new Claim(ClaimTypes.Role, role) // مهم لسياسة AdminOnly
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}