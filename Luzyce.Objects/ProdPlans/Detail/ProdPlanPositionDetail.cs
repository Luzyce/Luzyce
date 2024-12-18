using Luzyce.Objects.ProdPlans.List;

namespace Luzyce.Objects.ProdPlans.Detail;

public class ProdPlanPositionDetail
{
    public ProdPlansPositionListItem Position { get; set; } = new();
    public ProdPlansListItem Parent { get; set; } = new();
}
