using System.Security.Claims;
using Luzyce.Api.Core.Dictionaries;
using Luzyce.Shared.Models.Log;
using Luzyce.Shared.Models.User;
using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Api.Db.AppDb.Models;
using Microsoft.EntityFrameworkCore;

namespace Luzyce.Api.Repositories;

public class LogRepository(ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

    public void AddLog(int clientId, string hash, string data)
    {
        applicationDbContext.Logs.Add(new Log
        {
            Timestamp = DateTime.Now.ConvertToEuropeWarsaw(),
            ClientId = clientId,
            Operation = "Nieudane logowanie",
            Hash = hash,
            Data = data
        });
        applicationDbContext.SaveChanges();
    }

    public void AddLog(int clientId, int userId, string operation, string hash, string data)
    {
        applicationDbContext.Logs.Add(new Log
        {
            Timestamp = DateTime.Now.ConvertToEuropeWarsaw(),
            ClientId = clientId,
            UserId = userId,
            Operation = operation,
            Hash = hash,
            Data = data
        });
        applicationDbContext.SaveChanges();
    }

    public void AddLog(ClaimsPrincipal? user, string operation, string? data)
    {
        applicationDbContext.Logs.Add(new Log
        {
            Timestamp = DateTime.Now.ConvertToEuropeWarsaw(),
            ClientId = GetId(ClaimTypes.PrimarySid),
            Operation = operation,
            UserId = GetId(ClaimTypes.NameIdentifier),
            Hash = user?.FindFirstValue(ClaimTypes.Hash) ?? "",
            Data = data
        });
        applicationDbContext.SaveChanges();
        return;

        int? GetId(string claimType) =>
            int.TryParse(user?.FindFirstValue(claimType), out var id) && id > 0 ? id : null;
    }


    public GetLogs GetLogs(int offset, int limit)
    {
        return new GetLogs
        {
            Logs = applicationDbContext.Logs
                .Include(x => x.User)
                .Include(x => x.Client)
                .OrderByDescending(x => x.Timestamp)
                .Skip((offset - 1) * limit)
                .Take(limit)
                .Select(x => new GetLog
                {
                    Id = x.Id,
                    Timestamp = x.Timestamp,
                    Client = x.Client == null
                        ? null
                        : new GetClient
                        {
                            Id = x.Client.Id,
                            Name = x.Client.Name,
                            IpAddress = x.Client.IpAddress,
                            Type = x.Client.Type
                        },
                    Operation = x.Operation,
                    User = x.User == null
                        ? null
                        : new GetUserResponseDto
                        {
                            Id = x.User.Id,
                            Name = x.User.Name,
                            LastName = x.User.LastName,
                            Login = x.User.Login
                        },
                    Hash = x.Hash
                })
                .ToList()
        };
    }

    public GetLogs GetUnidentifiedLogs(int offset, int limit)
    {
        return new GetLogs
        {
            Logs = applicationDbContext.Logs
                .Include(x => x.Client)
                .Where(x => x.UserId == null && x.ClientId != null && x.Hash != null)
                .OrderByDescending(x => x.Timestamp)
                .Skip((offset - 1) * limit)
                .Take(limit)
                .Select(x => new GetLog
                {
                    Id = x.Id,
                    Timestamp = x.Timestamp,
                    Client = x.Client == null
                        ? null
                        : new GetClient
                        {
                            Id = x.Client.Id,
                            Name = x.Client.Name,
                            IpAddress = x.Client.IpAddress,
                            Type = x.Client.Type
                        },
                    Operation = x.Operation,
                    User = x.User == null
                        ? null
                        : new GetUserResponseDto
                        {
                            Id = x.User.Id,
                            Name = x.User.Name,
                            LastName = x.User.LastName,
                            Login = x.User.Login
                        },
                    Hash = x.Hash
                })
                .ToList()
        };
    }

    public void AssignUser(AssignUserDto assignUserDto)
    {
        var user = applicationDbContext.Users.Find(assignUserDto.UserId);

        if (user == null)
        {
            return;
        }

        foreach (var log in assignUserDto.Logs)
        {
            var dbLog = applicationDbContext.Logs.Find(log.Id);

            if (dbLog == null)
            {
                continue;
            }

            dbLog.UserId = user.Id;
        }

        applicationDbContext.SaveChanges();
    }
}