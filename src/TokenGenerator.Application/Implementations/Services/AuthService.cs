using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenGenerator.Application.Dtos;
using TokenGenerator.Application.Interfaces.IServices;

namespace TokenGenerator.Application.Implementations.Services
{
    public class AuthService: IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(UserDto user)
        {
            var key = GenerateSecurityKey();
        
            var signingCredentials = GenerateSigningCredentials(key);

            var token = new JwtSecurityToken(
               claims: GetClaims(user),
               expires: DateTime.Now.AddHours(24),
               signingCredentials: signingCredentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private IEnumerable<Claim> GetClaims(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim("id", user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return claims;
        }

        private SymmetricSecurityKey GenerateSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token")?.Value!));
        }

        private SigningCredentials GenerateSigningCredentials(SymmetricSecurityKey key)
        {
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        }
    }
}
