
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGoncharovFileSharingService.JwtTokenProvider
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        public string GetJwtToken(Guid id, string name)
        {
            var claims = new[] 
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier,id.ToString())
            };
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_KEY"))),
                SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Env.GetString("JWT_ISSUER"),
                audience: Env.GetString("JWT_AUDIENCE"),
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(Env.GetInt("JWT_EXPIRE"))
                );
            return string.Empty;

        }
    }
}
