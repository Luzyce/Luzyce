using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.DocumentDependencyChart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/documentDependencyChart")]
public class DocumentDependencyChart(DocumentDependencyChartRepository documentDependencyChartRepository) : Controller
{
    [HttpPost]
    [Authorize]
    public IActionResult Get([FromBody] GetDocumentDependencyChartRequest getDocumentDependencyChartRequest)
    {
        var documentDependencyChart = documentDependencyChartRepository.GetDocumentDependencyChart(getDocumentDependencyChartRequest);
        return Ok(documentDependencyChart);
    }
}