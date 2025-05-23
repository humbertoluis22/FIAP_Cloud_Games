using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InfraEstructure.Auth
{
    public  class TokenGenerate
    {
        
        private readonly JwtSettings _jwtSettings;
        public TokenGenerate(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(int id, string userName,string role)
        {
          
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = generateClaims(id,userName, role),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(3)
            };
            var token = handler.CreateToken(tokenDescriptor);
            var strToken = handler.WriteToken(token);

            return strToken;
        }

        public static ClaimsIdentity generateClaims(int id ,string name , string role) 
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, name));
            ci.AddClaim(new Claim(ClaimTypes.Role, role));
            return ci;
        }
    }
}
