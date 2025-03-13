namespace Luzyce.Api.Domain.Models;

public class Order
{
    public int Id { get; set; }
    public int SubiektId { get; set; }
    public int Version { get; set; }
    public DateTime Date { get; set; }
    public string Number { get; set; } = string.Empty;
    public string? OriginalNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerSymbol { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime? DeliveryDate { get; set; }
    public int Status { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public List<OrderPosition> Positions { get; set; } = [];
}
