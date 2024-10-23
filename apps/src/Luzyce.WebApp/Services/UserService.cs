using System.Net.Http.Json;
using Luzyce.Shared.Models.Event;
using Luzyce.Shared.Models.User;

namespace Luzyce.WebApp.Services;

public class UserService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<List<GetUserResponseDto>?> GetUsersAsync()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<List<GetUserResponseDto>>($"/api/user") ??
               [];
    }

    public async Task<GetUserForUpdateDto?> GetUserByIdAsync(int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetUserForUpdateDto>($"api/user/{id}") ?? new GetUserForUpdateDto();
    }

    public async Task UpdateUserAsync(UpdateUserDto user, int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }
        await httpClient.PutAsJsonAsync($"api/user/{id}", user);
    }

    public async Task<List<GetRoleDto>?> GetRolesAsync()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<List<GetRoleDto>>("api/user/roles") ?? [];
    }

    public async Task ResetPasswordAsync(int userId, UpdatePasswordDto newPassword)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }
        await httpClient.PutAsJsonAsync($"api/user/{userId}/password", newPassword);
    }

    public async Task<GetUserResponseDto?> CreateUserAsync(CreateUserDto user)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        var response = await httpClient.PostAsJsonAsync("/api/user", user);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GetUserResponseDto>();
        }

        return null;
    }

    public async Task<GetEvents> GetLogsAsync(int offset, int limit)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return new GetEvents();
        }
        return await httpClient.GetFromJsonAsync<GetEvents>($"api/log/unidentified/{offset}/{limit}") ?? new GetEvents();
    }

    public async Task AssignUserAsync(AssignUserDto assignUserDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }
        await httpClient.PutAsJsonAsync("api/log/assignUser", assignUserDto);
    }
}
