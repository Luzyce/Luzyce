using Luzyce.Shared.Models.Document;

namespace Luzyce.Shared.Models.ProductionPlan;

public class GetProductionPlanForCalendar
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public GetShift Shift { get; set; } = new();
    public int Team { get; set; }
    public GetStatusResponseDto? Status { get; set; }
}