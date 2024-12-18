using Luzyce.Objects.ProdOrders.List;
using Luzyce.Objects.Statuses;
using Luzyce.Objects.Users;

namespace Luzyce.Objects.ProdPlans.List;

public class ProdPlansListItem
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int ShiftNumber => Shift.ShiftNumber;
    public int Team { get; set; }
    public Status Status { get; set; } = new();
    public User? ShiftSupervisor => Shift.ShiftSupervisor;
    public User HeadsOfMetallurgicalTeams { get; set; } = new();
    public string Remarks { get; set; } = string.Empty;
    public IEnumerable<ProdOrdersListItem> Parents => Positions.Select(x => x.ProdOrderPosition.Parent).Distinct();
    
    public IEnumerable<ProdPlansPositionListItem> Positions { get; set; } = new List<ProdPlansPositionListItem>();
    public Shift Shift { get; set; } = new();
}
