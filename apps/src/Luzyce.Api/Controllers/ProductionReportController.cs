using System.Globalization;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.ProductionReport;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/productionRaport")]
public class ProductionReportController(ProductionReportRepository prRepo) : ControllerBase
{
    [HttpGet("downloadExcel/{date}/{shift:int}")]
    public IActionResult DownloadExcel(string date, int shift)
    {
        var (kwits, lacks) = prRepo.GetKwits(date, shift);

        if (kwits.Count == 0)
        {
            return NotFound("No data found.");
        }

        var templatePath = Path.Combine("Resources", "Report-prod-template.xlsx");

        if (!System.IO.File.Exists(templatePath))
        {
            return NotFound("Template file not found.");
        }

        string GetRomanNumeral(int? number) => number switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            _ => ""
        };
        
        using var workbook = new XLWorkbook(templatePath);
        var worksheet = workbook.Worksheet(1);
        
        worksheet.Cell("B2").Value = kwits.First().ProductionPlanPositions?.ProductionPlan?.Date.ToString("dd.MM.yyyy");
        worksheet.Cell("G2").Value = GetRomanNumeral(kwits.First().ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftNumber);
        worksheet.Cell("J2").Value = kwits.First().ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.Name + " " +
                                     kwits.First().ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.LastName;

        foreach (var kwit in kwits)
        {
            var row = kwits.IndexOf(kwit) * 3 + 3;

            worksheet.Cell(3, row).Value = kwit.ProductionPlanPositions?.ProductionPlan?.Team;
            worksheet.Cell(5, row).Value = kwit.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.Name + " " +
                                           kwit.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.LastName;
            worksheet.Cell(6, row).Value = kwit.DocumentPositions.First().LampshadeNorm?.Lampshade?.Code + " " +
                                           kwit.DocumentPositions.First().LampshadeNorm?.Variant?.Name;
            worksheet.Cell(7, row).Value = kwit.Number;

            worksheet.Cell(8, row).Value = kwit.DocumentPositions.First().QuantityGross;
            worksheet.Cell(9, row).Value = kwit.DocumentPositions.First().QuantityNetto;

            for (var i = 1; i < lacks[kwits.IndexOf(kwit)].Count; i++)
            {
                worksheet.Cell(11 + i, row).Value = lacks[kwits.IndexOf(kwit)][i].Quantity;
            }

            worksheet.Cell(45, row).Value = lacks[kwits.IndexOf(kwit)].FirstOrDefault(x => x.ErrorCode == "00")?.Quantity;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var fileContent = stream.ToArray();

        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"Raport produkcji {DateOnly.ParseExact(date, "yyyy-MM-dd"):yyMMdd} zm {GetRomanNumeral(shift)}.xlsx");
    }
}
