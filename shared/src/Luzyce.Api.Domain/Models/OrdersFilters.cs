namespace Luzyce.Api.Domain.Models;

public class OrdersFilters
{
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? CustomerName { get; set; }
    public int? Status { get; set; }
}
