using Luzyce.Shared.Models.Document;
using Luzyce.Shared.Models.User;

namespace Luzyce.Shared.Models.ProductionPlan;

public class GetProductionPlan
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public GetShift Shift { get; set; } = new();
    public int Team { get; set; }
    public GetUserResponseDto? HeadsOfMetallurgicalTeams { get; set; }
    public string? Remarks { get; set; }
    public GetStatusResponseDto? Status { get; set; }
    public List<GetProductionPlanPosition> ProductionPlanPositions { get; set; } = [];
}