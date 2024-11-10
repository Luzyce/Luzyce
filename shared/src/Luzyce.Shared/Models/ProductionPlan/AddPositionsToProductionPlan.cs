namespace Luzyce.Shared.Models.ProductionPlan;

public class AddPositionsToProductionPlan
{
    public DateOnly Date { get; set; }
    public int Shift { get; set; }
    public int Team { get; set; }
    public Dictionary<int, int> Positions { get; set; } = new();
}