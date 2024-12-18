using Luzyce.Objects.Productions.List;

namespace Luzyce.Services;

public class ProductionService(KwitService kwitService) : IProductionService
{
    public PositionsList GetPositionsList()
    {
        return new PositionsList
        {
            Positions = kwitService.GetKwitList().Kwits.Select(kwit => new PositionsListItem()
            {
                Kwit = kwit
            })
        };
    }
}
