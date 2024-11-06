using System.Net;
using System.Net.Http.Json;
using Luzyce.Shared.Models.ProductionOrder;

namespace Luzyce.WebApp.Services;

public class ProductionOrderService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetProductionOrdersResponse?> GetProductionOrders()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetProductionOrdersResponse>($"/api/productionOrder");
    }
    
    public async Task<GetProductionOrder?> GetProductionOrder(int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetProductionOrder>($"/api/productionOrder/{id}");
    }
    
    public async Task<int?> CreateProductionOrderAsync(CreateProductionOrderRequest createProductionOrderDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        
        var response = await httpClient.PostAsJsonAsync("/api/productionOrder/new", createProductionOrderDto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<int>();
        }

        return null;
    }
    
    public async Task<bool> UpdateProductionOrderAsync(int id, UpdateProductionOrder updateProductionOrderDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        var response = await httpClient.PostAsJsonAsync($"/api/productionOrder/update/{id}", updateProductionOrderDto);
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }
    
    public async Task<GetNormsResponse?> GetNormsAsync(GetNorms getNorms)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }

        var response = await httpClient.PostAsJsonAsync("/api/productionOrder/getNorms", getNorms);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GetNormsResponse>();
        }

        return null;
    }

    public async Task<bool> ArchiveProductionOrder(int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        var response = await httpClient.GetAsync($"/api/productionOrder/archive/{id}");
        return response.IsSuccessStatusCode;
    }
}
