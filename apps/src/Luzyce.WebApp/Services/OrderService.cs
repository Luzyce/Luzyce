using System.Net;
using System.Net.Http.Json;
using Luzyce.Shared.Models.Lampshade;
using Luzyce.Shared.Models.Order;

namespace Luzyce.WebApp.Services;

public class OrderService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetOrdersResponseDto?> GetOrdersAsync(int pageNumber, GetOrdersDto getOrdersDto,
        CancellationToken cancellationToken)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        var response = await httpClient.PostAsJsonAsync($"/api/order/{pageNumber}", getOrdersDto, cancellationToken);
        return await response.Content.ReadFromJsonAsync<GetOrdersResponseDto>(cancellationToken) ??
               new GetOrdersResponseDto();
    }
    
    public async Task<GetVariantsResponseDto?> GetVariantsAsync()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return new GetVariantsResponseDto();
        }
        var response = await httpClient.GetAsync("api/lampshade/variants");
        return await response.Content.ReadFromJsonAsync<GetVariantsResponseDto>();
    }
    
    public async Task<StockResponse?> GetStockAsync(StockRequest stockRequest)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return new StockResponse();
        }
        var response = await httpClient.PostAsJsonAsync("api/order/stock", stockRequest);
        return await response.Content.ReadFromJsonAsync<StockResponse>();
    }
}