using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public class Jwt
    {
        public static string GenerateJwtToken<T>(string secretKey, T value)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("User", JsonConvert.SerializeObject(value)),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public static T ReadJwtToken<T>(string jwtToken, string secretKey, T value)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                // Lee y valida el token JWT
                jwtToken = jwtToken.Replace("Bearer ", "");
                var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out _);
                foreach (var item in principal.Claims)
                {
                    value = JsonConvert.DeserializeObject<T>(item.Value);
                    break;
                }
                return value;
            }
            catch (Exception ex)
            {
                // El token no es válido
                return value;
            }
        }
    }
}
