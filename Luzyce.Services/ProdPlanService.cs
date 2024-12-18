using Luzyce.Objects.ProdPlans.Detail;
using Luzyce.Objects.ProdPlans.List;

namespace Luzyce.Services;

public class ProdPlanService : IProdPlanService
{

    public IEnumerable<ProdPlanPositionDetail> GetProdPlanDetailList()
    {
        throw new NotImplementedException();
    }
    
    public ProdPlanPositionDetail GetProdPlanDetail()
    {
        return new ProdPlanPositionDetail()
        {
            Position = GetProdPlanPosition(),
            Parent = GetProdPlan()
        };
    }
    public ProdPlansPositionListItem GetProdPlanPosition()
    {
        throw new NotImplementedException();
    }
    public ProdPlansListItem GetProdPlan()
    {
        throw new NotImplementedException();
    }
}
