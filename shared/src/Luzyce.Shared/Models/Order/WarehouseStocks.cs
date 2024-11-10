namespace Luzyce.Shared.Models.Order;

public class WarehouseStocks
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int QuantityMin { get; set; }
    public int QuantityRes { get; set; }
    public int QuantityMax { get; set; }
}