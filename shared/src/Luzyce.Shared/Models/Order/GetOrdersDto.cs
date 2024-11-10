namespace Luzyce.Shared.Models.Order;

public class GetOrdersDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? CustomerName { get; set; }
    public int Status { get; set; }
}
