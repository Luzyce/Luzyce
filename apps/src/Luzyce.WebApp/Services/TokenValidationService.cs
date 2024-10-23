using Blazored.LocalStorage;

namespace Luzyce.WebApp.Services;

public class TokenValidationService(HttpClient httpClient, ILocalStorageService localStorageService)
{
    public async Task<bool> IsTokenValid()
    {
        var token = await localStorageService.GetItemAsStringAsync("token");
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return true;
    }
}