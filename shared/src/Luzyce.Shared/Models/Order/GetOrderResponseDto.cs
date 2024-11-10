namespace Luzyce.Shared.Models.Order;

public class GetOrderResponseDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Number { get; set; } = string.Empty;
    public string? OriginalNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerSymbol { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime? DeliveryDate { get; set; }
    public int Status { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public List<GetOrderPositionResponseDto> Positions { get; set; } = [];
}
