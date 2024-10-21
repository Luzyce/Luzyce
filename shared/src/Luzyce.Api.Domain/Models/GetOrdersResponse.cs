namespace Luzyce.Api.Domain.Models;

public class GetOrdersResponse
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalOrders { get; set; }
    public List<Order> Orders { get; set; } = [];
}
