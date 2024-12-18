using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Api.Db.AppDb.Models;
using Luzyce.Objects.Kwits.List;
using Luzyce.Api.Core.Dictionaries;

namespace Luzyce.Services;

public class KwitService(ApplicationDbContext appDbContext, ProdPlanService prodPlanService) : IKwitService
{
    public KwitsList GetKwitList()
    {
        var kwits = appDbContext.Documents
            .Where(d => d.DocumentsDefinition != null && d.DocumentsDefinition.Code == DocumentsDefinitions.KW_CODE)
            .ToList();
        
        return new KwitsList()
        {
            // Kwits = prodPlanService.GetProdPlanDetailList().Select(prodPlanDetail => new KwitsListItem()
            // {
            //     ProdPlanPosition = prodPlanDetail
            // })
            Kwits = kwits.Select(kwit => new KwitsListItem
            {
                ProdPlanPosition = prodPlanService.GetProdPlanDetail()
            })
        };
    }
}
