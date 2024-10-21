using Luzyce.Shared.Models.Order;

namespace Luzyce.Shared.Models.ProductionOrder;

public class CreateProductionOrderRequest
{
    public GetOrderResponseDto Order { get; set; } = new GetOrderResponseDto();
    public List<CreateProductionOrder> ProductionOrderPositions { get; set; } = [];
}