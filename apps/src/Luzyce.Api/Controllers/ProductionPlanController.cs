using System.Globalization;
using System.Text.Json;
using ClosedXML.Excel;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.ProductionPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using iTextLayout = iText.Layout;
using iTextVerticalAlignment=iText.Layout.Properties.VerticalAlignment;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Svg.Converter;
using iText.Svg.Processors;
using iText.Svg.Processors.Impl;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/productionPlan")]
public class ProductionPlanController(ProductionPlanRepository productionPlanRepository, EventRepository eventRepository) : Controller
{
    [HttpPost]
    [Authorize]
    public IActionResult GetProductionPlans(GetMonthProductionPlanRequest request)
    {
        eventRepository.AddLog(User, "Pobrano plany produkcji", JsonSerializer.Serialize(request));
        return Ok(productionPlanRepository.GetProductionPlans(request));
    }

    [HttpPost("addPositions")]
    [Authorize]
    public IActionResult AddPositionsToProductionPlan(AddPositionsToProductionPlan request)
    {
        var resp = productionPlanRepository.AddPositionsToProductionPlan(request);

        if (resp == 0)
        {
            eventRepository.AddLog(User, "Nie udało się dodać pozycji do planu produkcji", JsonSerializer.Serialize(request));
            return Conflict();
        }

        eventRepository.AddLog(User, "Dodano pozycje do planu produkcji", JsonSerializer.Serialize(request));

        return Ok();
    }

    [HttpPost("getProductionPlan")]
    [Authorize]
    public IActionResult GetProductionPlan(GetProductionPlanPositionsRequest request)
    {
        eventRepository.AddLog(User, "Pobrano plan produkcji", JsonSerializer.Serialize(request));
        return Ok(productionPlanRepository.GetProductionPlan(request));
    }

    [HttpDelete("delPosition/{id:int}")]
    [Authorize]
    public IActionResult DeletePosition(int id)
    {
        eventRepository.AddLog(User, "Usunięto pozycję z planu produkcji", JsonSerializer.Serialize(new
        {
            id
        }));
        productionPlanRepository.DeletePosition(id);
        return Ok();
    }

    [HttpGet("getShiftSupervisor")]
    [Authorize]
    public IActionResult GetShiftSupervisor()
    {
        eventRepository.AddLog(User, "Pobrano Kierowników Zmian", null);
        return Ok(productionPlanRepository.ShiftSupervisor());
    }

    [HttpGet("headsOfMetallurgicalTeams")]
    [Authorize]
    public IActionResult GetHeadsOfMetallurgicalTeams()
    {
        eventRepository.AddLog(User, "Pobrano Hutników", null);
        return Ok(productionPlanRepository.GetHeadsOfMetallurgicalTeams());
    }

    [HttpPut("updateProductionPlan")]
    [Authorize]
    public IActionResult UpdatePositions(UpdateProductionPlan request)
    {
        productionPlanRepository.UpdateProductionPlan(request);

        eventRepository.AddLog(User, "Zaktualizowano pozycje w planie produkcji", JsonSerializer.Serialize(request));

        return Ok();
    }
    
    [HttpGet("refreshProductionPlan/{strDate}")]
    [Authorize]
    public IActionResult RefreshProductionPlan(string strDate)
    {
        var prodPlans = productionPlanRepository.RefreshProductionPlan(DateOnly.ParseExact(strDate, "yyyy-MM-dd"));
        eventRepository.AddLog(User, "Odświeżono plan produkcji", JsonSerializer.Serialize(new
        {
            date = strDate
        }));
        return Ok(prodPlans);
    }
    
    [HttpGet("kwit-{id:int}.pdf")]
    public IResult GetKwitPdf(int id)
    {
        var kwit = productionPlanRepository.GetKwit(id);
        
        if (kwit == null)
        {
            eventRepository.AddLog(User, "Nie udało się pobrać pliku pdf kwitu - kwit nie został znaleziony", JsonSerializer.Serialize(new
            {
                id
            }));
            return Results.File(Array.Empty<byte>(), "application/pdf");
        }
        
        var url = $"http://localhost:5132/api/productionPlan/kwit-{id}.pdf";
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrSvg = new SvgQRCode(qrCodeData);
        var qrSvgString = qrSvg.GetGraphic(3);
        
        using var stream = new MemoryStream();
        using var pdfReader = new PdfReader("Resources/kwit-template.pdf");
        using var pdfWriter = new PdfWriter(stream);
        using var pdfDoc = new PdfDocument(pdfReader, pdfWriter);
        var doc = new iTextLayout.Document(pdfDoc);
        
        var svgStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(qrSvgString));
        ISvgConverterProperties properties = new SvgConverterProperties();

        SvgConverter.DrawOnDocument(svgStream, pdfDoc, 1, 480, 680, properties);
        
        decimal totalHours = 0;
        
        if (kwit.ProductionPlanPositions?.Quantity != null &&
            kwit.ProductionPlanPositions.DocumentPosition?.LampshadeNorm?.QuantityPerChange != null &&
            kwit.ProductionPlanPositions.DocumentPosition.LampshadeNorm.QuantityPerChange != 0)
        {
            totalHours = kwit.ProductionPlanPositions.Quantity / 
                (decimal)kwit.ProductionPlanPositions.DocumentPosition.LampshadeNorm.QuantityPerChange * 8;
        }
    
        doc.ShowTextAligned(new Paragraph(kwit.Number).SetFontSize(20),
            15, 780, 1, TextAlignment.LEFT, iTextVerticalAlignment.TOP, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.ProductionPlanPositions?.ProductionPlan?.Team.ToString()).SetFontSize(16),
            85, 610, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftNumber.ToString()).SetFontSize(16),
            138, 610, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(totalHours.ToString(CultureInfo.CurrentCulture)).SetFontSize(16),
            195, 610, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.DocumentPositions[0].LampshadeNorm?.WeightBrutto.ToString()).SetFontSize(14),
            300, 610, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.DocumentPositions[0].LampshadeNorm?.WeightNetto.ToString()).SetFontSize(14),
            342, 610, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.Number).SetFontSize(10),
            407, 610, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.ProductionPlanPositions?.ProductionPlan?.Date.ToString("dd.MM.yyyy")).SetFontSize(10),
            523, 610, 1, TextAlignment.LEFT, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.DocumentPositions[0].OrderPositionForProduction?.Order?.Customer?.Name).SetFontSize(8),
            358, 591, 1, TextAlignment.LEFT, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.Name + " " +
                                          kwit.ProductionPlanPositions?.ProductionPlan?.Shift?.ShiftSupervisor?.LastName).SetFontSize(8),
            500, 591, 1, TextAlignment.LEFT, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.DocumentPositions[0].Lampshade?.Code).SetFontSize(10),
            140, 553, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.DocumentPositions[0].LampshadeNorm?.Variant?.Name).SetFontSize(10),
            195, 553, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.DocumentPositions[0].LampshadeNorm?.QuantityPerChange.ToString()).SetFontSize(16),
            253, 553, 1, TextAlignment.CENTER, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.ShowTextAligned(new Paragraph(kwit.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.Name + " " +
                                          kwit.ProductionPlanPositions?.ProductionPlan?.HeadsOfMetallurgicalTeams?.LastName).SetFontSize(12),
            65, 516, 1, TextAlignment.LEFT, iTextVerticalAlignment.MIDDLE, 0);
        
        doc.Close();
        return Results.File(stream.ToArray(), "application/pdf");
    }

    [HttpGet("ProdPlanExcel/{data}")]
    public IActionResult ProdPlanExcel(DateOnly data)
    {
        var productionPlans = productionPlanRepository.GetProductionPlanPdf(data);
        var shiftsSupervisors = productionPlanRepository.GetShiftsSupervisors(data);

        var templatePath = Path.Combine("Resources", "prod-plan-template.xlsx");
        
        if (!System.IO.File.Exists(templatePath))
        {
            return NotFound("Template file not found.");
        }
        
        using var workbook = new XLWorkbook(templatePath);
        var worksheet = workbook.Worksheet(1);

        for (var x = 0; x < shiftsSupervisors.Count; x++)
        {
            worksheet.Cell(x + 1, "E").Value = $"{shiftsSupervisors[x]?.Name} {shiftsSupervisors[x]?.LastName}";
        }
        
        worksheet.Cell("R2").Value = data.ToString("dd.MM.yyyy");

        for (var shiftNumber = 0; shiftNumber < 4; shiftNumber++)
        {
            var positions = productionPlans
                .Where(p => p.Shift!.ShiftNumber == shiftNumber + 1)
                .SelectMany(p => p.Positions)
                .OrderBy(p => p.ProductionPlan?.Team)
                .ToList();
            
            var shiftBaseRow = 5 + shiftNumber * 7;
            
            foreach (var position in positions)
            {
                var row = positions.IndexOf(position) + shiftBaseRow;
                worksheet.Cell(row, "E").Value = position.DocumentPosition?.Lampshade?.Code;
                worksheet.Cell(row, "F").Value = $"{position.DocumentPosition?.LampshadeNorm?.Variant?.Name} {position.DocumentPosition?.LampshadeDekor}";
                worksheet.Cell(row, "G").Value = $"{position.ProductionPlan?.HeadsOfMetallurgicalTeams?.Name} {position.ProductionPlan?.HeadsOfMetallurgicalTeams?.LastName}";
                worksheet.Cell(row, "H").Value = GetTeamRomanNumeral(position.ProductionPlan?.Team);
                
                if (position.DocumentPosition?.LampshadeNorm?.QuantityPerChange != null && position.DocumentPosition?.LampshadeNorm?.QuantityPerChange != 0)
                {
                    var totalHours = position.Quantity / (decimal)position.DocumentPosition?.LampshadeNorm?.QuantityPerChange! * 8;

                    var hours = (int)totalHours;
                    var minutes = (int)((totalHours - hours) * 60);

                    worksheet.Cell(row, "I").Value = hours switch
                    {
                        > 0 when minutes > 0 => $"{hours}h {minutes}m",
                        > 0 => $"{hours}h",
                        _ => $"{minutes}m"
                    };
                }

                worksheet.Cell(row, "J").Value = position.DocumentPosition?.LampshadeNorm?.WeightBrutto;
                worksheet.Cell(row, "K").Value = position.DocumentPosition?.LampshadeNorm?.WeightNetto;
                worksheet.Cell(row, "L").Value = position.DocumentPosition?.LampshadeNorm?.QuantityPerChange;
                worksheet.Cell(row, "Q").Value = position.DocumentPosition?.OrderPositionForProduction?.Order?.Customer?.Name;
            }
            
            worksheet.Cell(shiftBaseRow, "R").Value = worksheet.Cell(shiftBaseRow, "R").Value = string.Join("\n", positions
                .Select(p => new { Team = GetTeamRomanNumeral(p.ProductionPlan?.Team), p.ProductionPlan?.Remarks })
                .Where(r => !string.IsNullOrEmpty(r.Remarks))
                .Distinct()
                .Select(r => $"Zespół {r.Team}: {r.Remarks}"));
        }
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var fileContent = stream.ToArray();
        
        return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"Plan Produkcji.xlsx");

        string GetTeamRomanNumeral(int? team) => team switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            _ => ""
        };
    }
}
