using Luzyce.Objects.ProdOrders.Detail;

namespace Luzyce.Objects.ProdPlans.List;

public class ProdPlansPositionListItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int NumberOfHours { get; set; }
    public ProdOrderPositionDetail ProdOrderPosition { get; set; } = new();
}
