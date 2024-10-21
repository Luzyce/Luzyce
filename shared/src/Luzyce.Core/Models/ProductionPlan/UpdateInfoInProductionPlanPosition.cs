namespace Luzyce.Shared.Models.ProductionPlan;

public class UpdateInfoInProductionPlanPosition
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int? NumberOfHours { get; set; }
    public decimal? WeightNetto { get; set; }
    public decimal? WeightBrutto { get; set; }
    public int? QuantityPerChange { get; set; }
}