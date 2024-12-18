using Luzyce.Objects.ProdOrders.List;

namespace Luzyce.Objects.ProdOrders.Detail;

public class ProdOrderPositionDetail
{
    public ProdOrdersPositionListItem Position { get; set; } = new();
    public ProdOrdersListItem Parent { get; set; } = new();
}
