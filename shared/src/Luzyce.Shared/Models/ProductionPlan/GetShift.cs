using Luzyce.Shared.Models.User;

namespace Luzyce.Shared.Models.ProductionPlan;

public class GetShift
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int ShiftNumber { get; set; }
    public GetUserResponseDto? ShiftSupervisor { get; set; }
}