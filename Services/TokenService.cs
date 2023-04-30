using LocalStore.Domain.DTO;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace LocalStore.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IdentityUser user, List<string> roles)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Secrets:TokenSecret"]);
            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("Email", user.Email),
                    new Claim(ClaimTypes.Role, roles[0]),

                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var generatedToken = handler.CreateToken(descriptor);
            return handler.WriteToken(generatedToken);

        }


    }
}
