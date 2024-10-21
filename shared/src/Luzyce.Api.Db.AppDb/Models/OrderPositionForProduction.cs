namespace Luzyce.Api.Db.AppDb.Models;

public class OrderPositionForProduction
{
    public int Id { get; set; }
    public int? OrderId { get; set; }
    public OrderForProduction? Order { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public Lampshade? Product { get; set; }
    public string? Description { get; set; }
    public int? OrderPositionLp { get; set; }
    public decimal Quantity { get; set; }
    public decimal QuantityInStock { get; set; }
    public string? Unit { get; set; }
    public string? SerialNumber { get; set; }
    public string ProductSymbol { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
}