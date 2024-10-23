using System.Net;
using System.Net.Http.Json;
using Luzyce.Core.Models.ProductionOrder;
using Luzyce.Core.Models.ProductionPlan;
using Luzyce.Core.Models.ProductionPriority;

namespace Luzyce.WebApp.Services;

public class ProdPrioritiesService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetOrdersPositionsResponse?> GetOrdersPositions()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetOrdersPositionsResponse>($"/api/productionOrder/positions");
        
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