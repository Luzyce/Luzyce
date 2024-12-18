using Luzyce.Objects.ProdPlans.Detail;
using Luzyce.Objects.ProdPlans.List;

namespace Luzyce.Services;

public interface IProdPlanService
{
    public IEnumerable<ProdPlanPositionDetail> GetProdPlanDetailList();
    public ProdPlanPositionDetail GetProdPlanDetail();
    public ProdPlansPositionListItem GetProdPlanPosition();
    public ProdPlansListItem GetProdPlan();
}
