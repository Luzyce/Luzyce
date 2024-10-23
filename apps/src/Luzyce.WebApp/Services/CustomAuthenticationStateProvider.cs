using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Luzyce.WebApp.AppModels;

namespace Luzyce.WebApp.Services;

public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var stringToken = await localStorageService.GetItemAsStringAsync("token") ?? "";

            if (string.IsNullOrWhiteSpace(stringToken))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claims = GetClaimsFromToken(stringToken);

            var claimsPrincipal = SetClaimPrincipal(claims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(anonymous));
        }
    }

    public async Task UpdateAuthenticationState(string? token)
    {
        ClaimsPrincipal claimsPrincipal;
        if (!string.IsNullOrWhiteSpace(token))
        {
            var userSession = GetClaimsFromToken(token);
            claimsPrincipal = SetClaimPrincipal(userSession);
            await localStorageService.SetItemAsStringAsync("token", token);
        }
        else
        {
            claimsPrincipal = anonymous;
            await localStorageService.RemoveItemAsync("token");
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }


    private static ClaimsPrincipal SetClaimPrincipal(UserSession model)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
            new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, model.Id!),
                new(ClaimTypes.Email, model.Email!),
                new(ClaimTypes.Role, model.Role!),
                new(ClaimTypes.GivenName, model.Name!),
            }, "JwtAuth"));
    }

    private static UserSession GetClaimsFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        var claims = token.Claims.ToList();

        var Id = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var Email = claims.First(c => c.Type == ClaimTypes.Email).Value;
        var Role = claims.First(c => c.Type == ClaimTypes.Role).Value;
        var Name = claims.First(c => c.Type == ClaimTypes.GivenName).Value;

        return new UserSession() { Id = Id, Name = Name, Email = Email, Role = Role };
    }

    private static JsonSerializerOptions JsonOptions()
    {
        return new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
        };
    }

    public static string SerializeObj<T>(T modelObject) => JsonSerializer.Serialize(modelObject, JsonOptions());

    public static T DeserializeJsonString<T>(string jsonString) =>
        JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;

    public static IList<T> DeserializeJsonStringList<T>(string jsonString) =>
        JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;

    public static StringContent GenerateStringContent(string serializedObj) =>
        new(serializedObj, System.Text.Encoding.UTF8, "application/json");
}