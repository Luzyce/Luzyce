namespace Luzyce.Shared.Models.Order;

public class GetOrdersResponseDto
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalOrders { get; set; }
    public List<GetOrderResponseDto> Orders { get; set; } = [];
}
