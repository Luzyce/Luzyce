using System.Text.Json;
using ClosedXML.Excel;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.Production;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/production")]
public class ProductionController(ProductionRepository prodRepository, EventRepository eventRepository) : Controller
{
    [HttpPost]
    [Authorize]
    public IActionResult Get(GetProductionDto getProductionDto)
    {
        eventRepository.AddLog(User, "Pobrano produkcje", JsonSerializer.Serialize(new {getProductionDto}));
        return Ok(prodRepository.GetProduction(getProductionDto));
    }
    
    [HttpGet("downloadExcel/{dateStr}")]
    public IActionResult DownloadExcel(string dateStr)
    {
        eventRepository.AddLog(User, "Pobrano produkcje w formacie Excel", JsonSerializer.Serialize(new {dateStr}));
        var production = prodRepository.GetProduction(new GetProductionDto {SelectedMonth = DateOnly.ParseExact(dateStr, "yyyy-MM-dd")});
        
        var templatePath = Path.Combine("Resources", "production-template.xlsx");
        
        if (!System.IO.File.Exists(templatePath))
        {
            return NotFound("Template file not found.");
        }
        
        using var workbook = new XLWorkbook(templatePath);
        var worksheet = workbook.Worksheet(1);
        
        worksheet.Cell("M1").Value = DateOnly.ParseExact(dateStr, "yyyy-MM-dd")
            .ToDateTime(TimeOnly.MinValue)
            .ToString("MM.yyyy");

        foreach (var product in production)
        {
            worksheet.Cell("A" + (production.IndexOf(product) + 4)).Value = product.Date?.ToString("dd.MM.yyyy");
            worksheet.Cell("B" + (production.IndexOf(product) + 4)).Value = product.ClientName;
            worksheet.Cell("C" + (production.IndexOf(product) + 4)).Value = product.OrderNumber;
            worksheet.Cell("D" + (production.IndexOf(product) + 4)).Value = product.Shift;
            worksheet.Cell("E" + (production.IndexOf(product) + 4)).Value = product.Team;
            worksheet.Cell("F" + (production.IndexOf(product) + 4)).Value = product.HeadsOfMetallurgicalTeams;
            worksheet.Cell("G" + (production.IndexOf(product) + 4)).Value = product.Lampshade;
            worksheet.Cell("H" + (production.IndexOf(product) + 4)).Value = product.LampshadeVariant;
            worksheet.Cell("I" + (production.IndexOf(product) + 4)).Value = product.QuantityGross;

            var percentage = 0;
            if (product.QuantityGross != 0)
            {
                var totalLossAndImprove = product.QuantityLoss + product.QuantityToImprove;
                percentage = totalLossAndImprove / product.QuantityGross * 100;
            }
            
            worksheet.Cell("J" + (production.IndexOf(product) + 4)).Value = percentage.ToString("F2") + "%";
            worksheet.Cell("K" + (production.IndexOf(product) + 4)).Value = product.QuantityNetto;
            worksheet.Cell("L" + (production.IndexOf(product) + 4)).Value = product.WeightGross;
            worksheet.Cell("M" + (production.IndexOf(product) + 4)).Value = product.WeightNetto;
        }
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var fileContent = stream.ToArray();
        
        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"Produkcja {DateOnly.ParseExact(dateStr, "yyyy-MM-dd"):yyMM}.xlsx");
    }
}
