using Luzyce.Objects.Lampshades.Detail;

namespace Luzyce.Objects.Orders.List;

public class OrdersPositionsListItem
{
    public int Id { get; set; }
    public LampshadeDetail? Product { get; set; }
    public string? Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal QuantityInStock { get; set; }
    public string? Unit { get; set; }
    public string? SerialNumber { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
}
