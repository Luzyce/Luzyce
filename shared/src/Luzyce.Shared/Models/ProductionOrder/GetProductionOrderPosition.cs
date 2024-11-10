using Luzyce.Shared.Models.Lampshade;

namespace Luzyce.Shared.Models.ProductionOrder;

public class GetProductionOrderPosition
{ 
    public int Id { get; set; }
    public int QuantityNetto { get; set; }
    public int QuantityGross { get; set; }
    public int QuantityOnPlans { get; set; }
    public List<GetQuantityOnPlan> QuantitiesOnPlans { get; set; } = [];
    public DateTime? ExecutionDate { get; set; }
    public GetLampshade Lampshade { get; set; } = new GetLampshade();
    public GetLampshadeNorm LampshadeNorm { get; set; } = new GetLampshadeNorm();
    public string LampshadeDekor { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public string? CustomerLampshadeNumber { get; set; }
    public decimal? NumberOfChanges { get; set; }
    public int? QuantityMade { get; set; }
    public int ProductId { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? ProductionOrderNumber { get; set; }
    public string Client { get; set; } = string.Empty;
    public int Priority { get; set; }
}