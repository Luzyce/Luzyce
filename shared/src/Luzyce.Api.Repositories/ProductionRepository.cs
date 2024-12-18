using Luzyce.Api.Core.Dictionaries;
using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Shared.Models.Production;
using Microsoft.EntityFrameworkCore;

namespace Luzyce.Api.Repositories;

public class ProductionRepository(ApplicationDbContext applicationDbContext)
{
    public List<GetProduct> GetProduction(GetProductionDto? productionFilter = null)
    {
        var query = applicationDbContext.Documents
            .Where(d => d.DocumentsDefinition != null && 
                        d.DocumentsDefinition.Code == DocumentsDefinitions.KW_CODE && 
                        d.ProductionPlanPositions != null && 
                        d.ProductionPlanPositions.ProductionPlan != null && 
                        d.ProductionPlanPositions.ProductionPlan.Date.Month == productionFilter!.SelectedMonth.Month)
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.ProductionPlan)
            .ThenInclude(pp => pp!.Shift)
            .ThenInclude(s => s!.ShiftSupervisor)
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.DocumentPosition)
            .ThenInclude(dp => dp!.OrderPositionForProduction)
            .ThenInclude(opfp => opfp!.Order)
            .ThenInclude(o => o!.Customer)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.Lampshade)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.LampshadeNorm)
            .ThenInclude(ln => ln!.Variant)
            .Include(document => document.ProductionPlanPositions)
            .ThenInclude(productionPlanPositions => productionPlanPositions!.ProductionPlan)
            .ThenInclude(productionPlan => productionPlan!.HeadsOfMetallurgicalTeams)
            .ToList();
        
        var production = query
            .OrderByDescending(d => d.ProductionPlanPositions?.ProductionPlan?.Date)
            .ThenByDescending(d => d.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftNumber)
            .ThenByDescending(d => d.ProductionPlanPositions?.ProductionPlan?.Team)
            .ThenByDescending(d => d.Id)
            .Select(d => new GetProduct()
            {
                Id = d.Id,
                Date = d.ProductionPlanPositions?.ProductionPlan?.Date,
                Shift = d.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftNumber ?? 0,
                Team = d.ProductionPlanPositions?.ProductionPlan?.Team ?? 0,
                ClientName = d.ProductionPlanPositions?.DocumentPosition?.OrderPositionForProduction?.Order?.Customer?.Name ?? string.Empty,
                OrderNumber = d.ProductionPlanPositions?.DocumentPosition?.OrderPositionForProduction?.Order?.Number ?? string.Empty,
                Lampshade = d.DocumentPositions.First().Lampshade?.Code ?? string.Empty,
                LampshadeVariant = d.DocumentPositions.First().LampshadeNorm?.Variant?.Name ?? string.Empty,
                ShiftSupervisor = d.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.Name + d.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.LastName,
                HeadsOfMetallurgicalTeams = d.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.Name + d.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.LastName,
                QuantityNetto = d.DocumentPositions.First().QuantityNetto,
                QuantityLoss = d.DocumentPositions.First().QuantityLoss,
                QuantityToImprove = d.DocumentPositions.First().QuantityToImprove,
                QuantityGross = d.DocumentPositions.First().QuantityGross,
                WeightGross = d.DocumentPositions.First().LampshadeNorm?.WeightBrutto,
                WeightNetto = d.DocumentPositions.First().LampshadeNorm?.WeightNetto
            })
            .ToList();
        
        return production;
    }
}
