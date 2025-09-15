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
                        d.ProductionPlanPositions.ProductionPlan != null);

        // If no search term is provided, filter by month (original behavior)
        // If search term is provided, search across all data regardless of month
        if (string.IsNullOrEmpty(productionFilter?.SearchTerm))
        {
            query = query.Where(d => d.ProductionPlanPositions.ProductionPlan.Date.Month == productionFilter!.SelectedMonth.Month);
        }

        var finalQuery = query
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
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.Kwit)
            .ToList();
        
        var production = finalQuery
            .OrderByDescending(d => d.ProductionPlanPositions?.ProductionPlan?.Date)
            .ThenByDescending(d => d.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftNumber)
            .ThenByDescending(d => d.ProductionPlanPositions?.ProductionPlan?.Team)
            .ThenByDescending(d => d.Id)
            .Select(d => new GetProduct
            {
                Id = d.Id,
                KwitId = d.ProductionPlanPositions?.Kwit.First().Id ?? 0,
                KwitNumber = d.ProductionPlanPositions?.Kwit.First().Number ?? string.Empty,
                Date = d.ProductionPlanPositions?.ProductionPlan?.Date,
                Shift = d.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftNumber ?? 0,
                Team = d.ProductionPlanPositions?.ProductionPlan?.Team ?? 0,
                ClientName = d.ProductionPlanPositions?.DocumentPosition?.OrderPositionForProduction?.Order?.Customer?.Name ?? string.Empty,
                OrderNumber = d.ProductionPlanPositions?.DocumentPosition?.OrderPositionForProduction?.Order?.Number ?? string.Empty,
                Lampshade = d.DocumentPositions.First().Lampshade?.Code ?? string.Empty,
                LampshadeVariant = d.DocumentPositions.First().LampshadeNorm?.Variant?.Name ?? string.Empty,
                ShiftSupervisor = d.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.Name + " " + d.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.LastName,
                HeadsOfMetallurgicalTeams = d.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.Name + " " + d.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.LastName,
                QuantityNetto = d.DocumentPositions.First().QuantityNetto,
                QuantityLoss = d.DocumentPositions.First().QuantityLoss,
                QuantityToImprove = d.DocumentPositions.First().QuantityToImprove,
                QuantityGross = d.DocumentPositions.First().QuantityGross,
                WeightGross = d.DocumentPositions.First().LampshadeNorm?.WeightBrutto,
                WeightNetto = d.DocumentPositions.First().LampshadeNorm?.WeightNetto
            })
            .ToList();

        // Apply search filter if search term is provided
        if (!string.IsNullOrEmpty(productionFilter?.SearchTerm))
        {
            var searchTerm = productionFilter.SearchTerm.ToLower();
            production = production.Where(p => 
                (p.Date?.ToString("dd.MM.yyyy") ?? string.Empty).ToLower().Contains(searchTerm) ||
                p.ClientName.ToLower().Contains(searchTerm) ||
                p.OrderNumber.ToLower().Contains(searchTerm) ||
                p.KwitNumber.ToLower().Contains(searchTerm) ||
                p.Shift.ToString().Contains(searchTerm) ||
                p.Team.ToString().Contains(searchTerm) ||
                p.HeadsOfMetallurgicalTeams.ToLower().Contains(searchTerm) ||
                p.Lampshade.ToLower().Contains(searchTerm) ||
                p.LampshadeVariant.ToLower().Contains(searchTerm) ||
                p.QuantityGross.ToString().Contains(searchTerm) ||
                p.QuantityNetto.ToString().Contains(searchTerm) ||
                (p.WeightGross?.ToString() ?? string.Empty).Contains(searchTerm) ||
                (p.WeightNetto?.ToString() ?? string.Empty).Contains(searchTerm)
            ).ToList();
        }
        
        return production;
    }
}
