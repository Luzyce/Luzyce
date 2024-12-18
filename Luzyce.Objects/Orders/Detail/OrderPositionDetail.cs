using Luzyce.Objects.Orders.List;

namespace Luzyce.Objects.Orders.Detail;

public class OrderPositionDetail
{
    public OrdersPositionsListItem Position { get; set; } = new();
    public OrdersListItem Parent { get; set; } = new();
}
