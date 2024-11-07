using Luzyce.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/workflow")]
public class WorkflowController(WorkflowRepository workflowRepo, EventRepository eventRepository) : Controller
{
    [HttpGet("getIps")]
    public IActionResult GetIps()
    {
        var ips = workflowRepo.GetIps();
        return Ok(ips);
    }
}
