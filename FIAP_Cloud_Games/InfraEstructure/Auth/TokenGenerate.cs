using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InfraEstructure.Auth
{
    public  class TokenGenerate
    {
        private readonly string privateKey = "ADIOJSOdaijodaioAOA@#!@!oAOISOSOADASsaosasd((*12232AAAASSADDSSAASCSASXSASSASCXA";

        public string GenerateToken(string userName,string role)
        {
          
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(privateKey);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = generateClaims(userName, role),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(3)
            };
            var token = handler.CreateToken(tokenDescriptor);
            var strToken = handler.WriteToken(token);

            return strToken;
        }

        public static ClaimsIdentity generateClaims(string name, string role) 
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.Name, name));
            ci.AddClaim(new Claim(ClaimTypes.Role, role));
            return ci;
        }
    }
}
