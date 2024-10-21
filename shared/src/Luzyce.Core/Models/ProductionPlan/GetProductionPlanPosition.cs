using Luzyce.Shared.Models.Document;
using Luzyce.Shared.Models.ProductionOrder;

namespace Luzyce.Shared.Models.ProductionPlan;

public class GetProductionPlanPosition
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public GetProductionOrderPosition DocumentPosition { get; set; } = new GetProductionOrderPosition();
    public int? NumberOfHours { get; set; }
    public GetDocumentWithPositions? Kwit { get; set; }
}