using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Extensions.Authentication;

public class AuthenticationService(IConfiguration config, DataContext dbContext) : IAuthenticationService
{
    public async Task<string> GenerateTokenAsync(User user)
    {
        string key = config["JWT:key"]!;

        SigningCredentials credentials =
            new SigningCredentials(GetSymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new(CustomClaimTypes.Id, user.Id.ToString()),
            new(CustomClaimTypes.UserName, user.UserName),
            new(CustomClaimTypes.Email, user.Email),
            new(CustomClaimTypes.Phone, user.PhoneNumber),
            new(CustomClaimTypes.FirstName, user.FirstName),
            new(CustomClaimTypes.LastName, user.LastName),
        ];

        claims.AddRange(await (from u in dbContext.Users
            join ur in dbContext.UserRoles on u.Id equals ur.UserId
            join r in dbContext.Roles on ur.RoleId equals r.Id
            where u.Id == user.Id
            select new Claim(CustomClaimTypes.Role, r.Name)).ToListAsync());


        JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: config["JWT:issuer"],
            audience: config["JWT:audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public static SymmetricSecurityKey GetSymmetricSecurityKey(string key) =>
        new(Encoding.UTF8.GetBytes(key));
}