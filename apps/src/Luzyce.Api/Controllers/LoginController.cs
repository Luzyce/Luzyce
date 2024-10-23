using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Client = Luzyce.Api.Domain.Models.Client;
using User = Luzyce.Api.Domain.Models.User;

namespace Luzyce.Api.Controllers;
[Route("api/login")]
[ApiController]
public class LoginController(IConfiguration config, UsersRepository usersRepository, EventRepository eventRepository) : Controller
{
    private readonly IConfiguration config = config;
    private readonly UsersRepository usersRepository = usersRepository;
    private readonly EventRepository _eventRepository = eventRepository;

    private string generateJSONWebToken(User user, bool isHashLogin, Client client)
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, string.IsNullOrEmpty(user.Email) ? "" : user.Email),
                new Claim(ClaimTypes.Role, usersRepository.GetRole(user.RoleId)?.Name ?? ""),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.PrimarySid, client.Id.ToString()),
                new Claim(ClaimTypes.Hash, string.IsNullOrEmpty(user.Hash) ? "" : user.Hash),
                new Claim(ClaimTypes.SerialNumber, string.IsNullOrEmpty(user.Hash) ? "" : user.Hash),
        };

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: isHashLogin ? DateTime.Now.AddHours(13) : DateTime.Now.AddHours(1),
            notBefore: DateTime.Now,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SigningKey"] ?? "")),
                SecurityAlgorithms.HmacSha256
                )
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var isHashLogin = !string.IsNullOrEmpty(dto.Hash);
        var user = isHashLogin ? usersRepository.GetUserByHash(dto.Hash)
            : usersRepository.GetUserByLoginAndPassword(dto.Login, dto.Password);
        var clientType = isHashLogin ? "Terminal" : "Web";
        var ipAddr = dto.IpAddress;
        var name = "";

        if (string.IsNullOrEmpty(ipAddr))
        {
            if (clientType == "Web")
            {
                ipAddr = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            }
            else
            {
                return Unauthorized("Brak adresu IP");
            }
        }

        try
        {
            var hostEntry = Dns.GetHostEntry(ipAddr);
            name = hostEntry.HostName;
        }
        catch (SocketException)
        {
            name = null;
        }

        var client = usersRepository.GetClientByIp(ipAddr, clientType) ??
                     usersRepository.AddClient(new Client { Name = name, IpAddress = ipAddr, Type = clientType });

        dto.Password = "";

        if (user == null)
        {
            _eventRepository.AddLog(client.Id, dto.Hash, JsonSerializer.Serialize(dto));

            return Unauthorized();
        }

        var tokenString = generateJSONWebToken(user, isHashLogin, client);

        _eventRepository.AddLog(client.Id, user.Id,"Zalogowano pomyślnie", user.Hash, JsonSerializer.Serialize(dto));

        return Ok(
            new LoginResponseDto
            {
                Token = tokenString,
                Result = new GetUserResponseDto { Id = user.Id, Name = user.Name, LastName = user.LastName, Login = user.Login }
            });
    }
    
    [HttpGet("refreshToken")]
    [Authorize]
    public IActionResult RefreshToken()
    {
        var user = usersRepository.GetUserById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"));
        var client = usersRepository.GetClientById(int.Parse(User.FindFirstValue(ClaimTypes.PrimarySid) ?? "0"));
        
        if (user == null || client == null)
        {
            return Unauthorized();
        }
        
        var tokenString = generateJSONWebToken(user, false, client);

        return Ok(
            new LoginResponseDto
            {
                Token = tokenString,
                Result = new GetUserResponseDto { Id = user.Id, Name = user.Name, LastName = user.LastName, Login = user.Login }
            });
    }
}
