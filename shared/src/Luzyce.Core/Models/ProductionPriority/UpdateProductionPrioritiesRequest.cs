using Luzyce.Shared.Models.ProductionOrder;

namespace Luzyce.Shared.Models.ProductionPriority;

public class UpdateProductionPrioritiesRequest
{
    public List<GetProductionOrderPosition> Positions { get; set; } = [];
}