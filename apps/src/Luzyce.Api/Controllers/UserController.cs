using System.Text.Json;
using Luzyce.Api.Mappers;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(UsersRepository usersRepository, EventRepository eventRepository) : ControllerBase
{
    private readonly UsersRepository usersRepository = usersRepository;
    private readonly EventRepository _eventRepository = eventRepository;

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        _eventRepository.AddLog(User, "Get users", null);
        return Ok(
            usersRepository.GetUsers()
                .Select(x => new GetUserResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastName = x.LastName,
                    Login = x.Login
                })
                .ToList());
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult Get(int id)
    {
        var user = usersRepository.GetUserById(id);
        if (user == null)
        {
            _eventRepository.AddLog(User, "Failed to get user - user not found", JsonSerializer.Serialize(new {id}));
            return NotFound();
        }

        var isUserLocked = usersRepository.IsUserLocked(user.Id);

        _eventRepository.AddLog(User, "Get user", JsonSerializer.Serialize(new {id}));

        return Ok(
            new GetUserForUpdateDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? "",
                Login = user.Login,
                Hash = user.Hash,
                RoleId = user.Role!.Id,
                IsLocked = isUserLocked
            });
    }

    [HttpPost]
    [Authorize]
    public IActionResult Post([FromBody] CreateUserDto dto)
    {
        var user = dto.ToUserFromCreateDto();
        usersRepository.AddUser(user);

        dto.Password = "";

        _eventRepository.AddLog(User, "Create user", JsonSerializer.Serialize(dto));

        return CreatedAtAction(
            nameof(Get),
            new { id = user.Id },
            new GetUserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Login = user.Login
            });
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult Put(int id, [FromBody] UpdateUserDto dto)
    {
        var user = usersRepository.GetUserById(id);
        if (user == null)
        {
            _eventRepository.AddLog(User, "Failed to update user - user not found", JsonSerializer.Serialize(new {id}));
            return NotFound();
        }

        user = UserMappers.UpdateUserFromDto(dto, user);

        usersRepository.UpdateUser(user);

        _eventRepository.AddLog(User, "Update user", JsonSerializer.Serialize(new {id, dto}));

        return Ok(new GetUserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Login = user.Login
        });
    }

    [HttpPut("{id}/password")]
    [Authorize]
    public IActionResult PutPassword(int id, [FromBody] UpdatePasswordDto dto)
    {
        var user = usersRepository.GetUserById(id);
        if (user == null)
        {
            _eventRepository.AddLog(User, "Nie udało się zaktualizować hasła użytkownika — użytkownik nie został znaleziony", JsonSerializer.Serialize(new {id}));
            return NotFound();
        }

        user = UserMappers.UpdateUserPasswordFromDto(dto, user);

        usersRepository.UpdatePassword(user);

        _eventRepository.AddLog(User, "Zaktualizowano hasło użytkownika", JsonSerializer.Serialize(new {id}));

        return Ok(new GetUserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Login = user.Login
        });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Delete(int id)
    {
        var user = usersRepository.GetUserById(id);
        if (user == null)
        {
            _eventRepository.AddLog(User, "Nie udało się usunąć użytkownika – użytkownik nie został znaleziony", JsonSerializer.Serialize(new {id}));
            return NotFound();
        }

        usersRepository.DeleteUser(user);

        _eventRepository.AddLog(User, "Usunięto użytkownika", JsonSerializer.Serialize(new {id}));

        return Ok();
    }

    [HttpGet("roles")]
    [Authorize]
    public IActionResult GetRoles()
    {
        _eventRepository.AddLog(User, "Pobrano role", null);

        return Ok(
            usersRepository.GetRoles()
                .Select(x => new GetRoleDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList());
    }

    [HttpGet("roles/{id}")]
    [Authorize]
    public IActionResult GetRole(int id)
    {
        var role = usersRepository.GetRole(id);
        if (role == null)
        {
            _eventRepository.AddLog(User, "Nie udało się pobrać roli – rola nie została znaleziona", JsonSerializer.Serialize(new {id}));
            return NotFound();
        }

        _eventRepository.AddLog(User, "Pobrano role", JsonSerializer.Serialize(new {id}));

        return Ok(new GetRoleDto
        {
            Id = role.Id,
            Name = role.Name
        });
    }
}
