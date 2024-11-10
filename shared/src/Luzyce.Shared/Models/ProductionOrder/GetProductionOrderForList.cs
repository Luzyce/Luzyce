using Luzyce.Shared.Models.Document;

namespace Luzyce.Shared.Models.ProductionOrder;

public class GetProductionOrderForList
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string? OriginalOrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime ProdOrderDate { get; set; }
    public string ProdOrderNumber { get; set; } = string.Empty;
    public DateTime? DeliveryDate { get; set; }
    public GetStatusResponseDto Status { get; set; } = new();
}