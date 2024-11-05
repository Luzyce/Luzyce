using Luzyce.Api.Core.Dictionaries;
using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Api.Db.AppDb.Models;
using Luzyce.Shared.Models.Kwit;
using Microsoft.EntityFrameworkCore;

namespace Luzyce.Api.Repositories;

public class ProductionReportRepository(ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

    public (List<Document>, List<List<GetLacks>>) GetKwits(string date, int shift)
    {
        var kwits = applicationDbContext.Documents
            .Include(d => d.Warehouse)
            .Include(d => d.Operator)
            .Include(d => d.Status)
            .Include(d => d.DocumentsDefinition)
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.ProductionPlan)
            .ThenInclude(pp => pp!.HeadsOfMetallurgicalTeams)
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.ProductionPlan)
            .ThenInclude(pp => pp!.Shift)
            .ThenInclude(s => s!.ShiftSupervisor)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.Lampshade)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.LampshadeNorm)
            .ThenInclude(ln => ln!.Variant)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.LampshadeNorm)
            .ThenInclude(ln => ln!.Lampshade)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.OrderPositionForProduction)
            .ThenInclude(op => op!.Order)
            .ThenInclude(order => order!.Customer)
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.DocumentPosition)
            .ThenInclude(dp => dp!.LampshadeNorm)
            .Where(x => x.DocumentsDefinitionId == DocumentsDefinitions.KW_ID &&
                        x.ProductionPlanPositions != null &&
                        x.ProductionPlanPositions.ProductionPlan != null &&
                        x.ProductionPlanPositions.ProductionPlan.Shift != null &&
                        x.ProductionPlanPositions.ProductionPlan.Date == DateOnly.ParseExact(date, "yyyy-MM-dd") &&
                        x.ProductionPlanPositions.ProductionPlan.Shift.ShiftNumber == shift)
            .ToList();

        var allLacks = applicationDbContext.Errors
            .OrderBy(ec => ec.Id)
            .Select(ec => new GetLacks
            {
                ErrorName = ec.Name, ErrorCode = ec.Code, Quantity = 0
            })
            .ToList();

        var kwitsLacks = new List<List<GetLacks>>();
        foreach (var kwit in kwits)
        {
            var operationWithErrorsInKwit = applicationDbContext.Operations
                .Include(x => x.ErrorCode)
                .Where(x => x.DocumentId == kwit.Id &&
                            x.QuantityLossDelta > 0 &&
                            x.IsCancelled == false)
                .ToList();

            var lacks = new List<GetLacks>();
            foreach (var error in allLacks)
            {
                var totalQuantity = operationWithErrorsInKwit
                    .Where(op => op.ErrorCode?.Code == error.ErrorCode)
                    .Sum(op => op.QuantityLossDelta);

                lacks.Add(new GetLacks
                {
                    ErrorName = error.ErrorName, ErrorCode = error.ErrorCode, Quantity = totalQuantity > 0 ? totalQuantity : null
                });
            }

            kwitsLacks.Add(lacks);
        }

        return (kwits, kwitsLacks);
    }
}
