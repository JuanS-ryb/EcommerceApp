using Azure.Core;
using Ecommerce.server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.server.Utils
{
    public class JwtUtils(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        private readonly IConfiguration _config = config;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int? GetIdByToken()
        {
            var token = GetTokenFromRequest();
            return GetIdByToken(token);
        }

        public int? GetIdByToken(string? token)
        {
            var principal = GetPrincipalFromToken(token);
            var id = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(id, out var userId) ? userId : null;
        }

        public string HashMyPassword(User usr, string pass)
        {
            return new PasswordHasher<User>().HashPassword(usr, pass);
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string? token = null)
        {
            token ??= GetTokenFromRequest();
            if (string.IsNullOrWhiteSpace(token))
                return null;

            // Quita el prefijo "Bearer "
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token["Bearer ".Length..].Trim();

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["AppSettings:Token"]!);

            try
            {
                return handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _config["AppSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["AppSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);
            }
            catch
            {
                return null;
            }
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config.GetValue<string>("AppSettings:Issuer"),
                audience: _config.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );


            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        private string? GetTokenFromRequest()
        {
            var header = _httpContextAccessor.HttpContext?
                .Request.Headers.Authorization.FirstOrDefault();

            return string.IsNullOrWhiteSpace(header) ? null : header;
        }
    }
}