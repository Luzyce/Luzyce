using System.Net.Http.Json;
using Luzyce.Core.Models.DocumentDependencyChart;

namespace Luzyce.WebApp.Services;

public class DocumentDependencyChartService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetDocumentDependencyChart?> GetDocumentDependencyChart(GetDocumentDependencyChartRequest getDocumentDependencyChartRequest)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }

        var response = await httpClient.PostAsJsonAsync($"/api/documentDependencyChart", getDocumentDependencyChartRequest);
        return await response.Content.ReadFromJsonAsync<GetDocumentDependencyChart>();
    }
}