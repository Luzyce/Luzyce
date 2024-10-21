using System.Text.Json;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.ProductionPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/productionPlan")]
public class ProductionPlanController(ProductionPlanRepository productionPlanRepository, LogRepository logRepository) : Controller
{
    [HttpPost]
    [Authorize]
    public IActionResult GetProductionPlans(GetMonthProductionPlanRequest request)
    {
        logRepository.AddLog(User, "Pobrano plany produkcji", JsonSerializer.Serialize(request));
        return Ok(productionPlanRepository.GetProductionPlans(request));
    }

    [HttpPost("addPositions")]
    [Authorize]
    public IActionResult AddPositionsToProductionPlan(AddPositionsToProductionPlan request)
    {
        var resp = productionPlanRepository.AddPositionsToProductionPlan(request);

        if (resp == 0)
        {
            logRepository.AddLog(User, "Nie udało się dodać pozycji do planu produkcji", JsonSerializer.Serialize(request));
            return Conflict();
        }

        logRepository.AddLog(User, "Dodano pozycje do planu produkcji", JsonSerializer.Serialize(request));

        return Ok();
    }

    [HttpPost("getProductionPlan")]
    [Authorize]
    public IActionResult GetProductionPlan(GetProductionPlanPositionsRequest request)
    {
        logRepository.AddLog(User, "Pobrano plan produkcji", JsonSerializer.Serialize(request));
        return Ok(productionPlanRepository.GetProductionPlan(request));
    }

    [HttpDelete("delPosition/{id:int}")]
    [Authorize]
    public IActionResult DeletePosition(int id)
    {
        logRepository.AddLog(User, "Usunięto pozycję z planu produkcji", JsonSerializer.Serialize(new {id}));
        productionPlanRepository.DeletePosition(id);
        return Ok();
    }

    [HttpGet("getShiftSupervisor")]
    [Authorize]
    public IActionResult getShiftSupervisor()
    {
        logRepository.AddLog(User, "Pobrano Kierowników Zmian", null);
        return Ok(productionPlanRepository.ShiftSupervisor());
    }

    [HttpGet("headsOfMetallurgicalTeams")]
    [Authorize]
    public IActionResult GetHeadsOfMetallurgicalTeams()
    {
        logRepository.AddLog(User, "Pobrano Hutników", null);
        return Ok(productionPlanRepository.GetHeadsOfMetallurgicalTeams());
    }

    [HttpPut("updateProductionPlan")]
    [Authorize]
    public IActionResult UpdatePositions(UpdateProductionPlan request)
    {
        productionPlanRepository.UpdateProductionPlan(request);

        logRepository.AddLog(User, "Zaktualizowano pozycje w planie produkcji", JsonSerializer.Serialize(request));

        return Ok();
    }

    [HttpGet("kwit-{id:int}.pdf")]
    public IResult GetKwitPdf(int id)
    {
        var kwit = productionPlanRepository.GetKwit(id);

        if (kwit == null)
        {
            logRepository.AddLog(User, "Nie udało się pobrać pliku pdf kwitu - kwit nie został znaleziony", JsonSerializer.Serialize(new {id}));
            return Results.File(Array.Empty<byte>(), "application/pdf");
        }

        var url = $"http://localhost:5132/api/productionPlan/kwit-{id}.pdf";
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrSVG = new SvgQRCode(qrCodeData);
        var qrSVGString = qrSVG.GetGraphic(100);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header().Layers(layers =>
                {
                    layers.PrimaryLayer()
                        .TranslateX(-1, Unit.Centimetre)
                        .AlignRight()
                        .Width(2, Unit.Centimetre)
                        .Svg(qrSVGString);

                    layers.Layer()
                        .Text(kwit.Number)
                        .SemiBold()
                        .FontSize(16);
                });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item()
                            .Text(
                                $"Asortyment: {kwit.DocumentPositions[0].Lampshade?.Code} {kwit.DocumentPositions[0].LampshadeNorm?.Variant?.Name} {kwit.DocumentPositions[0].LampshadeDekor}")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Ilość: {kwit.ProductionPlanPositions?.Quantity} {kwit.DocumentPositions[0].OrderPositionForProduction?.Unit}")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Nazwa klienta: {kwit.DocumentPositions[0].OrderPositionForProduction?.Order?.Customer?.Name}")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Zmiana: {kwit.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftNumber}")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Zespół: {kwit.ProductionPlanPositions?.ProductionPlan?.Team}")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Hutmistrz: {kwit.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.Name} {kwit.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.LastName}")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Hutnik: {kwit.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.Name} {kwit.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.LastName}")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Norma: {kwit.DocumentPositions[0].LampshadeNorm?.QuantityPerChange} {kwit.DocumentPositions[0].OrderPositionForProduction?.Unit}")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Waga netto: {kwit.DocumentPositions[0].LampshadeNorm?.WeightNetto} kg")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Waga brutto: {kwit.DocumentPositions[0].LampshadeNorm?.WeightBrutto} kg")
                            .FontSize(16);
                        x.Item()
                            .Text(
                                $"Planowany wyciąg z wanny: {kwit.DocumentPositions[0].LampshadeNorm?.WeightBrutto * kwit.ProductionPlanPositions?.Quantity} kg")
                            .FontSize(16);
                    });
            });
        });

        var pdf = document.GeneratePdf();

        return Results.File(pdf, "application/pdf");
    }

    [HttpGet("productionPlan-{data}.pdf")]
    public IResult GetProductionPlanPdf(DateOnly data)
    {
        var productionPlans = productionPlanRepository.GetProductionPlanPdf(data);
        var shiftsSupervisors = productionPlanRepository.GetShiftsSupervisors(data);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text($"Plan produkcji na {data.ToString("d")}")
                    .SemiBold().FontSize(36);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(0.2f);

                                if (productionPlans.Any(p => p.Shift!.ShiftNumber == 1))
                                {
                                    columns.RelativeColumn();
                                }

                                if (productionPlans.Any(p => p.Shift!.ShiftNumber == 2))
                                {
                                    columns.RelativeColumn();
                                }

                                if (productionPlans.Any(p => p.Shift!.ShiftNumber == 3))
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Padding(5).Text("").FontSize(16);
                                if (productionPlans.Any(p => p.Shift!.ShiftNumber == 1))
                                {
                                    header.Cell().Element(CellStyle).Padding(5)
                                        .Text($"Zmiana 1\nHutmistrz:\n{shiftsSupervisors[0]?.Name} {shiftsSupervisors[0]?.LastName}").FontSize(16);
                                }

                                if (productionPlans.Any(p => p.Shift!.ShiftNumber == 2))
                                {
                                    header.Cell().Element(CellStyle).Padding(5)
                                        .Text($"Zmiana 2\nHutmistrz:\n{shiftsSupervisors[1]?.Name} {shiftsSupervisors[1]?.LastName}").FontSize(16);
                                }

                                if (productionPlans.Any(p => p.Shift!.ShiftNumber == 3))
                                {
                                    header.Cell().Element(CellStyle).Padding(5)
                                        .Text($"Zmiana 3\nHutmistrz:\n{shiftsSupervisors[2]?.Name} {shiftsSupervisors[2]?.LastName}").FontSize(16);
                                }
                            });

                            for (var x = 1; x <= 3; x++)
                            {
                                table.Cell().Element(CellStyle).Padding(5).AlignCenter().RotateLeft().Width(90).Text("Zespół " + x).AlignCenter().FontSize(16);

                                for (var y = 1; y <= 3; y++)
                                {
                                    if (productionPlans.All(p => p.Shift!.ShiftNumber != y))
                                    {
                                        continue;
                                    }

                                    var plan = productionPlans
                                        .Find(p => p.Team == x && p.Shift!.ShiftNumber == y);

                                    if (plan != null)
                                    {
                                        var cellText = $"Hutnik: {plan.HeadsOfMetallurgicalTeams?.Name} {plan.HeadsOfMetallurgicalTeams?.LastName}\n";
                                        cellText += $"Uwagi: {plan.Remarks}\n";
                                        
                                        for (var i = 0; i < plan.Positions.Count; i++)
                                        {
                                            if (i != 0)
                                            {
                                                cellText += "\n";
                                            }

                                            var position = plan.Positions[i];
                                            cellText += $"{i + 1}. " +
                                                        $"{position.Kwit.First().DocumentPositions.First().Lampshade!.Code} " +
                                                        $"{position.Kwit.First().DocumentPositions.First().LampshadeNorm!.Variant!.Name} " +
                                                        $"{position.Kwit.First().DocumentPositions.First().LampshadeDekor}\n" +
                                                        $"Ilość: {position.Quantity}\n" +
                                                        $"Waga Netto: {position.Kwit.First().DocumentPositions.First().LampshadeNorm?.WeightNetto}\n" +
                                                        $"Waga Brutto: {position.Kwit.First().DocumentPositions.First().LampshadeNorm?.WeightBrutto}\n" +
                                                        $"Norma: {position.Kwit.First().DocumentPositions.First().LampshadeNorm?.QuantityPerChange}\n" +
                                                        $"Kwit: {position.Kwit.First().Number}\n" +
                                                        $"Firma: {position.Kwit.First().DocumentPositions.First().OrderPositionForProduction?.Order?.Customer?.Name}\n";
                                        }

                                        table.Cell().Element(CellStyle).Padding(5).Text(cellText).FontSize(11);
                                    }
                                    else
                                    {
                                        table.Cell().Element(CellStyle).Text("-").AlignCenter();
                                    }
                                }
                            }
                            table.Cell().Element(CellStyle);

                            for (var y = 1; y <= 3; y++)
                            {
                                if (productionPlans.All(p => p.Shift!.ShiftNumber != y))
                                {
                                    continue;
                                }

                                var sum = productionPlans
                                    .Where(p => p.Shift!.ShiftNumber == y)
                                    .Sum(p => p.Positions.Sum(x => x.Quantity * x.DocumentPosition?.LampshadeNorm?.WeightBrutto));

                                table.Cell().Element(CellStyle).Padding(5)
                                    .Text($"Sumaryczna masa: {sum} kg").FontSize(11);
                            }
                        });
                    });
            });
        });

        var pdf = document.GeneratePdf();

        return Results.File(pdf, "application/pdf");
    }

    IContainer CellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Black);
    }
}