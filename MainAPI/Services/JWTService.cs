using MainAPI.Models.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Services
{
    public class JWTService
    {
        public string GenerateSecurityToken(UserSession userSession)
        {
            var subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", userSession.UserID.ToString()),
                new Claim("emailAddress", userSession.EmailAddress.ToString(), ClaimValueTypes.Email),
                new Claim("userName", userSession.Username.ToString()),
            });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("spyderSecretKey");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "SPYDER",
                Subject = new ClaimsIdentity(subject),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
