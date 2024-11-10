namespace Luzyce.Shared.Models.ProductionPlan;

public class UpdateProductionPlan
{
    public int Id { get; set; }
    public int? ShiftSupervisorId { get; set; }
    public int? HeadsOfMetallurgicalTeamsId { get; set; }
    public string? Remarks { get; set; }
    public List<UpdateInfoInProductionPlanPosition> ProductionPlanPositions { get; set; } = [];
}