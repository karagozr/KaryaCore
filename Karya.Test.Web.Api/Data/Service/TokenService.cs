using Karya.Test.Web.Api.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Karya.Test.Web.Api.Data.Service;

public class JwtOptions
{
    public string Key { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int ExpireMinutes { get; set; }
}

public interface ITokenService
{
    string CreateToken(UserAuthInfo info);
}

public class UserAuthInfo
{
    public string UserId { get;set; }
    public string TanentId { get; set; }
}

public class TokenService : ITokenService
{
    private readonly JwtOptions _options;

    public TokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string CreateToken(UserAuthInfo info)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, info.UserId),
            new Claim("TanentId", info.TanentId),

            // rol / permission ekleyebilirsin
            // new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_options.ExpireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}