namespace Luzyce.Objects.Orders.List;

public class OrdersList
{
    public IEnumerable<OrdersListItem> Orders { get; set; } = new List<OrdersListItem>();
}
