using Luzyce.Objects.Lampshades.Detail;
using Luzyce.Objects.Productions.List;
using Luzyce.Objects.Statuses;

namespace Luzyce.Objects.ProdOrders.List;

public class ProdOrdersPositionListItem
{
    public int Id { get; set; }
    public int QuantityNetto { get; set; }
    public int QuantityGross { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Status? Status { get; set; }
    public LampshadeDetail Lampshade { get; set; } = new();
    public int? OrderPositionForProductionId { get; set; }
    public PositionsListItem OrderPositions { get; set; } = new();
}
