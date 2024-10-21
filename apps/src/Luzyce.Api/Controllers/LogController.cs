using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[Route("api/log")]
public class LogController(LogRepository logRepository) : Controller
{
    private readonly LogRepository logRepository = logRepository;

    [HttpGet("{offset:int}/{limit:int}")]
    [Authorize]
    public IActionResult Get(int offset, int limit)
    {
        return Ok(logRepository.GetLogs(offset, limit));
    }

    [HttpGet("unidentified/{offset:int}/{limit:int}")]
    [Authorize]
    public IActionResult GetUnidentified(int offset, int limit)
    {
        return Ok(logRepository.GetUnidentifiedLogs(offset, limit));
    }

    [HttpPut("assignUser")]
    [Authorize]
    public IActionResult AssignUser([FromBody] AssignUserDto assignUserDto)
    {
        logRepository.AssignUser(assignUserDto);
        return Ok();
    }

}