using Luzyce.Api.Domain.Models;
using Luzyce.Shared.Models.Order;

namespace Luzyce.Api.Mappers;

public static class OrderMappers
{
    public static OrdersFilters ToOrdersFiltersFromDto(this GetOrdersDto dto)
    {
        return new OrdersFilters
        {
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            CustomerName = dto.CustomerName,
            Status = dto.Status
        };
    }
}
