namespace Luzyce.Api.Domain.Models;

public class ProductionOrder
{
    public int OperatorId { get; set; }
    public List<ProductionOrderPosition> ProductionOrderPositions { get; set; } = [];
}