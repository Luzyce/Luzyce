using System.Net;
using System.Net.Http.Json;
using Luzyce.Core.Models.Kwit;
using Luzyce.Core.Models.ProductionPlan;
using Luzyce.Core.Models.User;

namespace Luzyce.WebApp.Services;

public class ProductionPlanService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetProductionPlans?> GetProductionPlansAsync(GetMonthProductionPlanRequest getMonthProductionPlanRequest)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }   

        var response = await httpClient.PostAsJsonAsync("/api/productionPlan", getMonthProductionPlanRequest);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GetProductionPlans>();
        }

        return null;
    }
    
    public async Task<GetProductionPlan?> GetPositionsAsync(GetProductionPlanPositionsRequest getProductionPlanPositionsRequest)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }   

        var response = await httpClient.PostAsJsonAsync("/api/productionPlan/getProductionPlan", getProductionPlanPositionsRequest);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GetProductionPlan>();
        }
        
        return null;
    }
    
    public async Task<List<GetUserResponseDto>?> GetShiftSupervisorsAsync()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return [];
        }   

        var response = await httpClient.GetAsync("/api/productionPlan/getShiftSupervisor");
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<GetUserResponseDto>>();
        }
        
        return [];
    }
    
    public async Task<List<GetUserResponseDto>?> GetHeadsOfMetallurgicalTeamsAsync()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return [];
        }   

        var response = await httpClient.GetAsync("/api/productionPlan/headsOfMetallurgicalTeams");
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<GetUserResponseDto>>();
        }
        
        return [];
    }
    
    public async Task DeletePositionAsync(int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }   

        await httpClient.DeleteAsync($"/api/productionPlan/delPosition/{id}");
    }
    
    public async Task<bool> UpdateProductionPlan(UpdateProductionPlan request)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        
        var response = await httpClient.PutAsJsonAsync("/api/productionPlan/updateProductionPlan", request);
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }

    public async Task<GetKwit?> GetKwit(int kwitId)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }

        var response = await httpClient.GetAsync($"/api/document/{kwitId}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GetKwit>();
        }

        return null;
    }

    public async Task RevertKwit(int kwitId)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }

        await httpClient.GetAsync($"/api/document/{kwitId}/revert");
    }

    public async Task CloseKwit(int kwitId)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }

        await httpClient.GetAsync($"/api/document/{kwitId}/close");
    }

    public async Task UnlockKwit(int kwitId)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }

        await httpClient.GetAsync($"/api/document/{kwitId}/unlock");
    }

    public async Task UpdateKwit(UpdateKwit request)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }

        await httpClient.PutAsJsonAsync("/api/document/updateKwit", request);
    }
}