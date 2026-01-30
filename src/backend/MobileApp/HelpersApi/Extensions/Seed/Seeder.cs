using Application.Extensions.Algorithms;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace MobileApp.HelpersApi.Extensions.Seed;

public class Seeder(DataContext dbContext)
{
    public async Task Initial()
    {
        await InitUserAsync();
        await InitRoleAsync();
        await InitUserRoleAsync();
    }

    private async Task InitUserAsync()
    {
        foreach (User user in SeedData.GetSeedUsers())
        {
            User? existingUser = await dbContext.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == user.Id);
            if (existingUser is null)
            {
                await dbContext.Users.AddAsync(user);
            }
            else if (existingUser.IsDeleted)
            {
                existingUser.IsDeleted = false;
                existingUser.IsActive = true;
            }
        }

        await dbContext.SaveChangesAsync();
    }


    private async Task InitRoleAsync()
    {
        foreach (Role role in SeedData.GetSeedRoles())
        {
            Role? existingRole = await dbContext.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == role.Id);
            if (existingRole is null)
            {
                await dbContext.Roles.AddAsync(role);
            }
            else if (existingRole.IsDeleted)
            {
                existingRole.IsDeleted = false;
                existingRole.IsActive = true;
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task InitUserRoleAsync()
    {
        foreach (UserRole userRole in SeedData.GetSeedUserRoles())
        {
            UserRole? existingUserRole =
                await dbContext.UserRoles.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == userRole.Id);
            if (existingUserRole is null)
            {
                await dbContext.UserRoles.AddAsync(userRole);
            }
            else if (existingUserRole.IsDeleted)
            {
                existingUserRole.IsDeleted = false;
                existingUserRole.IsActive = true;
            }
        }

        await dbContext.SaveChangesAsync();
    }
}

file class SeedData
{
    public static List<User> GetSeedUsers()
        => Users;

    public static List<Role> GetSeedRoles()
        => Roles;

    public static List<UserRole> GetSeedUserRoles()
        => UserRoles;

    private static readonly List<User> Users =
    [
        new User()
        {
            Id = 1,
            UserName = "Admin",
            FirstName = "Admin",
            LastName = "Adminov",
            Email = "admin@gmail.com",
            PhoneNumber = "+992933448829",
            PasswordHash = HashAlgorithms.ConvertToHash("123456")
        },

        new User()
        {
            Id = 2,
            UserName = "User",
            FirstName = "User",
            LastName = "Userov",
            Email = "user@gmail.com",
            PhoneNumber = "+992222222222",
            PasswordHash = HashAlgorithms.ConvertToHash("123456")
        }
    ];

    private static readonly List<Role> Roles =
    [
        new()
        {
            Id = 1,
            Name = DefaultRoles.Admin,
            Description = "Admin is a good  person"
        },
        new()
        {
            Id = 2,
            Name = DefaultRoles.User,
            Description = "User is not a good  person"
        }
    ];

    private static readonly List<UserRole> UserRoles =
    [
        new()
        {
            Id = 1,
            RoleId = 1,
            UserId = 1
        },
        new()
        {
            Id = 2,
            RoleId = 2,
            UserId = 2
        },
    ];
}