using System.Net.Http.Json;
using Luzyce.Shared.Models.Production;

namespace Luzyce.WebApp.Services;

public class ProductionService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<List<GetProduct>?> GetProductsAsync(GetProductionDto getProductionDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }   

        var response = await httpClient.PostAsJsonAsync("/api/production", getProductionDto);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<GetProduct>>();
        }

        return null;
    }
}
