namespace Luzyce.Objects.Orders.List;

public class OrdersListItem
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Number { get; set; } = string.Empty;
    public Client Customer { get; set; } = new();
    public DateOnly? DeliveryDate { get; set; }
    public IEnumerable<OrdersPositionsListItem> Positions { get; set; } = new List<OrdersPositionsListItem>();
}
