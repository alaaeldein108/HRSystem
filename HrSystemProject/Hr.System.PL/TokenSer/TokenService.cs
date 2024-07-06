using HrSystem.DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hr.System.PL.TokenSer
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey key;
        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]));
        }
        public string GenerateToken(ApplicationUser appUser)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,appUser.UserName),
                new Claim(ClaimTypes.Email,appUser.Email),
                new Claim(ClaimTypes.MobilePhone,appUser.PhoneNumber),

            };
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = configuration["Token:Issuer"],
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
