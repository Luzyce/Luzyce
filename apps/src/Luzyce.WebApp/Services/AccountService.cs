using System.Net;
using Luzyce.Shared.Models.User;

namespace Luzyce.WebApp.Services;

public class AccountService(HttpClient httpClient)
{
    public async Task<LoginResponseDto> LoginAccount(LoginDto loginDTO)
    {
        var response = await httpClient
            .PostAsync("/api/login",
                CustomAuthenticationStateProvider
                    .GenerateStringContent(CustomAuthenticationStateProvider.SerializeObj(loginDTO)));

        if (!response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new LoginResponseDto()
            {
                Result = new GetUserResponseDto(),
                Token = ""
            };
        }

        var apiResponse = await response.Content.ReadAsStringAsync();
        return CustomAuthenticationStateProvider.DeserializeJsonString<LoginResponseDto>(apiResponse);
    }
}