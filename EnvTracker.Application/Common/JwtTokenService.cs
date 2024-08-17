using EnvTracker.Domain;
using EnvTracker.Domain.Entities;
using EnvTracker.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EnvTracker.Application.Common
{
    public class JwtTokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _securityKey;
        private readonly SigningCredentials _credentials;

        public JwtTokenService()
        {
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfig.JwtSettings.SecretKey));
            _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
        }

        public string GenerateToken(GenerateTokenReq obj)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AppConfig.JwtSettings.Issuer,
                Audience = AppConfig.JwtSettings.Audience,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = _credentials,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, obj.user_name),
                    new Claim(ClaimTypes.Email, obj.email),
                    new Claim(ClaimTypes.Name, obj.full_name),
                    new Claim(ClaimTypes.MobilePhone, obj.phone),
                    new Claim(ClaimTypes.Sid, obj.user_id.ToString()),
                    //new Claim(ClaimTypes.Role, obj.role),
                    new Claim("/Permissions", JsonConvert.SerializeObject(obj.permissions)),
                    new Claim("/UserID", obj.user_id.ToString())
                    // Add other claims as needed
                }),
                IssuedAt = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IEnumerable<Claim> Decode(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims;
            }
            catch
            {
                return null;
            }
        }
    }
}
