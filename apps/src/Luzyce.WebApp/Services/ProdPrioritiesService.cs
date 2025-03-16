using System.Net;
using System.Net.Http.Json;
using Luzyce.Shared.Models.ProductionOrder;
using Luzyce.Shared.Models.ProductionPlan;
using Luzyce.Shared.Models.ProductionPriority;

namespace Luzyce.WebApp.Services;

public class ProdPrioritiesService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetOrdersPositionsResponse?> GetOrdersPositions()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetOrdersPositionsResponse>("/api/productionOrder/positions");
        
    }
    
    public async Task<GetOrdersPositionsResponse?> GetDeletedPositions()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetOrdersPositionsResponse>("/api/productionPriority/getDeletedPositions");
    }
    
    public async Task<bool> RestorePosition(int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        var response = await httpClient.GetAsync($"/api/productionPriority/restorePosition/{id}");
        
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }
    
    public async Task<bool> SaveProductionPriority(UpdateProductionPrioritiesRequest request)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        var response = await httpClient.PostAsJsonAsync("/api/productionPriority/updatePriorities", request);
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }
    
    public async Task AddPositionsToProductionPlanAsync(AddPositionsToProductionPlan addPositionsToProductionPlan)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }   

        await httpClient.PostAsJsonAsync("/api/productionPlan/addPositions", addPositionsToProductionPlan);
    }
}
