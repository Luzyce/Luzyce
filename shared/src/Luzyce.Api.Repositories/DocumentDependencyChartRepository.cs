using Luzyce.Shared.Models.DocumentDependencyChart;
using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Api.Db.AppDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Luzyce.Api.Repositories;

public class DocumentDependencyChartRepository(ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

    public GetDocumentDependencyChart? GetDocumentDependencyChart(GetDocumentDependencyChartRequest getDocumentDependencyChartRequest)
    {
        var documentId = getDocumentDependencyChartRequest.DocumentId;
        int? orderId;

        switch (getDocumentDependencyChartRequest.DocumentType)
        {
            case "Zamówienie":
                orderId = documentId;
                break;
            case "Zlecenie produkcji":
                orderId = applicationDbContext.OrdersForProduction
                    .FirstOrDefault(x => x.Documents.Any(y => y.Id == documentId))?.Id;
                break;
            case "Plan produkcji":
                orderId = applicationDbContext.ProductionPlans
                    .Include(productionPlan => productionPlan.Positions)
                    .ThenInclude(ppp => ppp.DocumentPosition)
                    .ThenInclude(dp => dp!.Document)
                    .FirstOrDefault(x => x.Id == documentId)?.Positions.First().DocumentPosition?.Document?.OrderId;
                break;
            case "Kwit":
                orderId = applicationDbContext.Documents
                    .Include(document => document.ProductionPlanPositions)
                    .ThenInclude(ppp => ppp!.DocumentPosition)
                    .ThenInclude(dp => dp!.Document)
                    .FirstOrDefault(x => x.Id == documentId)?
                    .ProductionPlanPositions?.DocumentPosition?.Document?.OrderId;
                break;
            default:
                return null;
        }

        var order = applicationDbContext.OrdersForProduction
            .FirstOrDefault(x => x.Id == orderId);

        if (order == null)
        {
            return null;
        }

        var documentDependencyChart = CreateOrderChart(order);
        
        var relevantDocuments = applicationDbContext.Documents
            .Include(doc => doc.DocumentPositions)
            .ThenInclude(pos => pos.Lampshade)
            .Include(doc => doc.DocumentPositions)
            .ThenInclude(pos => pos.LampshadeNorm)
            .ThenInclude(norm => norm!.Variant)
            .Include(doc => doc.DocumentPositions)
            .ThenInclude(pos => pos.ProductionPlanPositions)
            .ThenInclude(planPos => planPos.ProductionPlan)
            .ThenInclude(plan => plan!.Shift)
            .Include(doc => doc.DocumentPositions)
            .ThenInclude(pos => pos.ProductionPlanPositions)
            .ThenInclude(planPos => planPos.ProductionPlan)
            .ThenInclude(plan => plan!.Positions)
            .ThenInclude(pp => pp.Kwit)
            .ThenInclude(document => document.DocumentPositions)
            .ThenInclude(documentPositions => documentPositions.Lampshade)
            .Include(doc => doc.DocumentPositions)
            .ThenInclude(documentPositions => documentPositions.LampshadeNorm)
            .ThenInclude(lampshadeNorm => lampshadeNorm!.Variant)
            .Include(document => document.DocumentPositions)
            .ThenInclude(documentPositions => documentPositions.ProductionPlanPositions).ThenInclude(productionPlanPositions => productionPlanPositions.ProductionPlan).ThenInclude(productionPlan => productionPlan.Positions).ThenInclude(productionPlanPositions => productionPlanPositions.DocumentPosition)
            .Where(doc => doc.Order != null && doc.Order.SubiektId == order.SubiektId)
            .ToList();

        foreach (var productionOrder in relevantDocuments)
        {
            var productionOrderChart = CreateProductionOrderChart(productionOrder);
            documentDependencyChart.Derivatives?.Add(productionOrderChart);

            foreach (var productionPlan in productionOrder.DocumentPositions.SelectMany(x => x.ProductionPlanPositions)
                .Select(x => x.ProductionPlan))
            {
                if (productionPlan == null)
                {
                    continue;
                }

                var productionPlanChart = CreateProductionPlanChart(productionPlan);
                productionOrderChart.Derivatives?.Add(productionPlanChart);

                foreach (var productionPlanPosition in productionPlan.Positions)
                {
                    if (productionPlanPosition.DocumentPosition?.Document == productionOrder)
                    {
                        productionPlanChart.Derivatives?.Add(CreateKwitChart(productionPlanPosition));
                    }
                }
            }
        }

        return documentDependencyChart;
    }

    private static GetDocumentDependencyChart CreateOrderChart(OrderForProduction order) => new GetDocumentDependencyChart
    {
        Id = order.Id,
        DocumentType = "Zamówienie",
        Name = order.Number,
        AddrToRedirect = string.Empty,
        Positions = order.OrderPosition.Select(x => (int)x.Quantity + " " + x.ProductName).ToList(),
        Derivatives = []
    };


    private static GetDocumentDependencyChart CreateProductionOrderChart(Document document)
    {
        return new GetDocumentDependencyChart
        {
            Id = document.Id,
            DocumentType = "Zlecenie produkcji",
            Name = document.Number,
            AddrToRedirect = "/productionOrder/edit/" + document.Id,
            Positions = document.DocumentPositions.Select(
                x => x.QuantityNetto + " " +
                     x.Lampshade?.Code + " " + x.LampshadeNorm?.Variant?.Name +
                     (!x.LampshadeDekor.IsNullOrEmpty() ? " " + x.LampshadeDekor : null)).ToList(),
            Derivatives = []
        };
    }

    private static GetDocumentDependencyChart CreateProductionPlanChart(ProductionPlan productionPlan) => new()
    {
        Id = productionPlan.Id,
        DocumentType = "Plan produkcji",
        Name = $"PP {productionPlan.Date:dd.MM.yyyy} " +
               $"Zmiana: {productionPlan.Shift?.ShiftNumber} " +
               $"Zespół: {productionPlan.Team}",
        AddrToRedirect = $"/productionPlan/edit?dateString={productionPlan.Date.ToString("yyyy-MM-dd")}&" +
                         $"shift={productionPlan.Shift?.ShiftNumber}&" +
                         $"team={productionPlan.Team}",
        Positions = productionPlan.Positions.Select(
                x => x.Quantity + " " +
                     x.DocumentPosition?.Lampshade?.Code + " " +
                     x.DocumentPosition?.LampshadeNorm?.Variant?.Name +
                     (!string.IsNullOrEmpty(x.DocumentPosition?.LampshadeDekor) ? " " +
                         x.DocumentPosition?.LampshadeDekor : string.Empty)).ToList(),
        Derivatives = []
    };

    private static GetDocumentDependencyChart CreateKwitChart(ProductionPlanPositions productionPlanPosition) => new()
    {
        Id = productionPlanPosition.Kwit.First().Id,
        DocumentType = "Kwit",
        Name = productionPlanPosition.Kwit.First().Number,
        AddrToRedirect = $"/productionPlan/editKwit?dateString={productionPlanPosition.ProductionPlan?.Date.ToString("yyyy-MM-dd")}&" +
                         $"shift={productionPlanPosition.ProductionPlan?.Shift?.ShiftNumber}&" +
                         $"team={productionPlanPosition.ProductionPlan?.Team}&" +
                         $"kwitId={productionPlanPosition.Kwit.First().Id}",
        Positions = productionPlanPosition.Kwit.First().DocumentPositions.Select(
                x => x.QuantityNetto + " " +
                     x.Lampshade?.Code + " " +
                     x.LampshadeNorm?.Variant?.Name +
                     (!x.LampshadeDekor.IsNullOrEmpty() ? " " + x.LampshadeDekor : null)).ToList()
    };
}
