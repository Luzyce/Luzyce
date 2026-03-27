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

    public async Task<List<GetProduct>?> SearchProductsAsync(string searchTerm, DateOnly? selectedMonth = null)
    {
        var searchDto = new GetProductionDto
        {
            SearchTerm = searchTerm,
            SelectedMonth = selectedMonth ?? new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 1)
        };

        return await GetProductsAsync(searchDto);
    }

    public async Task<byte[]?> ExportProductsAsync(GetProductionDto getProductionDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }   

        var response = await httpClient.PostAsJsonAsync("/api/production/downloadExcel", getProductionDto);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }

        return null;
    }
}
