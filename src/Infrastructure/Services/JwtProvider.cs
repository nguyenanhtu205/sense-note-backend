using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtProvider(IConfiguration configuration) : IJwtProvider
{
    public string Generate(Teacher teacher)
    {
        string? issuer = configuration["Api:Authority"];
        string? audience = configuration["Api:Audience"];
        string? key = configuration["Api:SigningKey"];

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, teacher.Id.ToString()),
            new(ClaimTypes.NameIdentifier, teacher.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, teacher.Email),
            new("FullName", teacher.FullName)
        ];

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key!));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
