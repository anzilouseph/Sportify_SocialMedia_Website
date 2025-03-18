using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SportifyKerala.Dto;

namespace SportifyKerala.Utilitys
{
    public class Token
    {
        private readonly IConfiguration _config;
        public Token(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(UserToListDto user)
        {
            var issuer = _config["JWTConfig:Issuer"];
            var audience = _config["JWTConfig:Audience"];
            var key = _config["JWTConfig:Key"];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT configuration settings are missing or invalid.");
            }

            //create claims for the Token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid,user.idOfUser.ToString()),
            };

            // Configure token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            // Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }

}
