using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Api.Db.AppDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Luzyce.Api.Repositories;

public class UsersRepository(ApplicationDbContext applicationDbContext, ILogger<UsersRepository> logger)
{
    private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

    public Luzyce.Api.Domain.Models.User? GetUserByHash(string hash)
    {
        logger.LogInformation($"Getting user by hash: {hash}");
        return applicationDbContext
            .Users
            .Include(d => d.Role)
            .Select(
                x => new Luzyce.Api.Domain.Models.User
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastName = x.LastName,
                    Email = x.Email,
                    Login = x.Login,
                    Password = x.Password,
                    Hash = x.Hash,
                    CreatedAt = x.CreatedAt,
                    RoleId = x.RoleId,
                    Role = RoleDomainFromDb(x.Role!)
                }
            )
            .FirstOrDefault(x => x.Hash == hash);
    }

    public Luzyce.Api.Domain.Models.User? GetUserByLoginAndPassword(string login, string password)
    {
        logger.LogInformation($"Getting user by login: {login}");

        var user = applicationDbContext.Users
                .Include(d => d.Role)
                .FirstOrDefault(x => x.Login == login);

        if (user == null)
        {
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }

        return new Luzyce.Api.Domain.Models.User
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Login = user.Login,
            Password = user.Password,
            Hash = user.Hash,
            CreatedAt = user.CreatedAt,
            RoleId = user.RoleId,
            Role = RoleDomainFromDb(user.Role!)
        };
    }

    public IEnumerable<Luzyce.Api.Domain.Models.User> GetUsers()
    {
        logger.LogInformation("Getting all users");
        return applicationDbContext
            .Users
            .Include(d => d.Role)
            .Select(
                x => new Luzyce.Api.Domain.Models.User
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastName = x.LastName,
                    Email = x.Email,
                    Login = x.Login,
                    Hash = x.Hash,
                    CreatedAt = x.CreatedAt,
                    RoleId = x.RoleId,
                    Role = RoleDomainFromDb(x.Role!)
                }
            )
            .ToList();
    }

    public Luzyce.Api.Domain.Models.User? GetUserById(int id)
    {
        logger.LogInformation($"Getting user by id: {id}");
        return applicationDbContext
            .Users
            .Include(d => d.Role)
            .Select(
                x => new Luzyce.Api.Domain.Models.User
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastName = x.LastName,
                    Email = x.Email,
                    Login = x.Login,
                    Hash = x.Hash,
                    CreatedAt = x.CreatedAt,
                    RoleId = x.RoleId,
                    Role = RoleDomainFromDb(x.Role!)
                }
            )
            .FirstOrDefault(x => x.Id == id);
    }

    public bool IsUserLocked(int id)
    {
        logger.LogInformation($"Checking if user is locked: {id}");
        // is this user in any document
        return
            applicationDbContext.Documents.Any(x => x.OperatorId == id) ||
            applicationDbContext.ProductionPlans.Any(x => x.HeadsOfMetallurgicalTeamsId == id) ||
            applicationDbContext.Operations.Any(x => x.OperatorId == id) ||
            applicationDbContext.Shifts.Any(x => x.ShiftSupervisorId == id);
    }

    public void AddUser(Luzyce.Api.Domain.Models.User user)
    {
        logger.LogInformation($"Adding user: {user.Login}");

        if (user.RoleId == 0)
        {
            user.RoleId = 2;
        }

        var dbUser = new User
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Login = user.Login,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Hash = user.Hash,
            CreatedAt = user.CreatedAt,
            RoleId = user.RoleId
        };

        applicationDbContext.Users.Add(dbUser);
        applicationDbContext.SaveChanges();

        user.Id = dbUser.Id;
    }

    public void UpdateUser(Luzyce.Api.Domain.Models.User user)
    {
        logger.LogInformation($"Updating user: {user.Login}");

        var userToUpdate = applicationDbContext.Users
                    .Include(d => d.Role)
                    .FirstOrDefault(x => x.Id == user.Id);

        if (userToUpdate == null)
        {
            return;
        }

        userToUpdate.Name = user.Name;
        userToUpdate.LastName = user.LastName;
        userToUpdate.Email = user.Email;
        userToUpdate.Login = user.Login;
        userToUpdate.Hash = user.Hash;
        userToUpdate.RoleId = user.RoleId;

        applicationDbContext.SaveChanges();
    }

    public void UpdatePassword(Luzyce.Api.Domain.Models.User user)
    {
        logger.LogInformation($"Updating password for user: {user.Login}");

        var userToUpdate = applicationDbContext.Users
                    .Include(d => d.Role)
                    .FirstOrDefault(x => x.Id == user.Id);

        if (userToUpdate == null)
        {
            return;
        }

        userToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        applicationDbContext.SaveChanges();
    }

    public void DeleteUser(Luzyce.Api.Domain.Models.User user)
    {
        logger.LogInformation($"Deleting user: {user.Login}");

        var userToDelete = applicationDbContext.Users.FirstOrDefault(x => x.Id == user.Id);

        if (userToDelete == null)
        {
            return;
        }

        applicationDbContext.Users.Remove(userToDelete);
        applicationDbContext.SaveChanges();
    }
    public IEnumerable<Luzyce.Api.Domain.Models.Role> GetRoles()
    {
        logger.LogInformation("Getting all roles");
        return applicationDbContext.Roles
            .Select(x => RoleDomainFromDb(x))
            .ToList();
    }

    public Luzyce.Api.Domain.Models.Role? GetRole(int id)
    {
        logger.LogInformation($"Getting role by id: {id}");
        var role = applicationDbContext.Roles.FirstOrDefault(x => x.Id == id);
        if (role == null)
        {
            return null;
        }
        return RoleDomainFromDb(role);
    }

    public Luzyce.Api.Domain.Models.Client? GetClientByIp(string ip, string type)
    {
        var client = applicationDbContext.Clients.FirstOrDefault(x => x.IpAddress == ip && x.Type == type);

        if (client == null)
        {
            return null;
        }

        return new Luzyce.Api.Domain.Models.Client
        {
            Id = client.Id,
            Name = client.Name,
            IpAddress = client.IpAddress
        };
    }

    public Luzyce.Api.Domain.Models.Client? GetClientById(int id)
    {
        var client = applicationDbContext.Clients.FirstOrDefault(x => x.Id == id);

        if (client == null)
        {
            return null;
        }

        return new Luzyce.Api.Domain.Models.Client
        {
            Id = client.Id,
            Name = client.Name,
            IpAddress = client.IpAddress
        };
    }
    public Luzyce.Api.Domain.Models.Client AddClient(Luzyce.Api.Domain.Models.Client client)
    {
        var dbClient = new Client
        {
            Name = client.Name,
            IpAddress = client.IpAddress,
            Type = client.Type
        };

        applicationDbContext.Clients.Add(dbClient);
        applicationDbContext.SaveChanges();

        client.Id = dbClient.Id;
        return client;
    }

    private static Luzyce.Api.Domain.Models.Role RoleDomainFromDb(Luzyce.Api.Db.AppDb.Models.Role role)
    {
        return new Luzyce.Api.Domain.Models.Role
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}
