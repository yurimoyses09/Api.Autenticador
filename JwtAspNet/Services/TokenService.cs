using JwtAspNet.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAspNet.Services;

public class TokenService
{
    public string CreateToken(User user)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);

            var credential = new SigningCredentials
            (
                new SymmetricSecurityKey(key),
                algorithm: SecurityAlgorithms.HmacSha256
            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credential,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Subject = GenerateClaims(user)
            };

            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        try
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim("id", user.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            ci.AddClaim(new Claim("image", user.Image));

            foreach (string role in user.Roles)
                ci.AddClaim(new Claim(ClaimTypes.Role, role));

            return ci;
        }
        catch (Exception)
        {
            throw;
        }
    }
}