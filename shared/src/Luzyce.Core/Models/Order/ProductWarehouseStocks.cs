namespace Luzyce.Shared.Models.Order;

public class ProductWarehouseStocks
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int QuantityMin { get; set; }
    public int QuantityRes { get; set; }
    public int QuantityMax { get; set; }
    public List<WarehouseStocks> WarehouseStocks { get; set; } = [];
}