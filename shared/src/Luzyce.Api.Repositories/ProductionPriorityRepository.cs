using Luzyce.Shared.Models.ProductionPriority;
using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Api.Db.AppDb.Models;

namespace Luzyce.Api.Repositories;

public class ProductionPriorityRepository(ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext applicationDbContext = applicationDbContext;
    
    public int UpdatePriorities(UpdateProductionPrioritiesRequest createProductionPriorityRequest)
    {
        using var transaction = applicationDbContext.Database.BeginTransaction();

        try
        {
            var requestIds = createProductionPriorityRequest.Positions.Select(p => p.Id).ToHashSet();
            
            var allDocumentPositions = applicationDbContext.DocumentPositions.ToList();
            
            for (var i = 0; i < createProductionPriorityRequest.Positions.Count; i++)
            {
                var documentPosition = allDocumentPositions.FirstOrDefault(x => x.Id == createProductionPriorityRequest.Positions[i].Id);

                if (documentPosition == null)
                {
                    transaction.Rollback();
                    return 0;
                }

                documentPosition.Priority = (i + 1) * 10;
                applicationDbContext.DocumentPositions.Update(documentPosition);
            }

            var missingPositions = allDocumentPositions.Where(x => !requestIds.Contains(x.Id) && !x.IsDeleted).ToList();
            foreach (var position in missingPositions)
            {
                position.IsDeleted = true;
                position.EndTime = DateTime.Now;
            }

            applicationDbContext.SaveChanges();
            transaction.Commit();

            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }
}
