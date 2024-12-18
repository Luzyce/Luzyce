using Luzyce.Objects.Lampshades.Detail;
using Luzyce.Objects.ProdPlans.Detail;
using Luzyce.Objects.Statuses;
using Luzyce.Objects.Users;

namespace Luzyce.Objects.Kwits.List;

public class KwitsListItem
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int QuantityNetto { get; set; }
    public int QuantityLoss { get; set; }
    public int QuantityToImprove { get; set; }
    public int QuantityGross { get; set; }
    public LampshadeDetail Lampshade { get; set; } = new();
    public User? Operator { get; set; }
    public DateOnly CreatedAt { get; set; }
    public DateOnly UpdatedAt { get; set; }
    public DateOnly? ClosedAt { get; set; }
    public Status Status { get; set; } = new();
    public Client LockedBy { get; set; } = new();
    public ProdPlanPositionDetail ProdPlanPosition { get; set; } = new();
}
