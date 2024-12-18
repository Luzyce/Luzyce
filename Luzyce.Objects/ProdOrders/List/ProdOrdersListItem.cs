using Luzyce.Objects.Orders.List;

namespace Luzyce.Objects.ProdOrders.List;

public class ProdOrdersListItem
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateOnly CreatedAt { get; set; }
    public DateOnly UpdatedAt { get; set; }
    public DateOnly? ClosedAt { get; set; }
    public OrdersListItem? Parent { get; set; }
    public IEnumerable<ProdOrdersPositionListItem> Positions { get; set; } = new List<ProdOrdersPositionListItem>();
}
