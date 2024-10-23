using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[Route("api/log")]
public class EventController(EventRepository eventRepository) : Controller
{
    private readonly EventRepository _eventRepository = eventRepository;

    [HttpGet("{offset:int}/{limit:int}")]
    [Authorize]
    public IActionResult Get(int offset, int limit)
    {
        return Ok(_eventRepository.GetLogs(offset, limit));
    }

    [HttpGet("unidentified/{offset:int}/{limit:int}")]
    [Authorize]
    public IActionResult GetUnidentified(int offset, int limit)
    {
        return Ok(_eventRepository.GetUnidentifiedLogs(offset, limit));
    }

    [HttpPut("assignUser")]
    [Authorize]
    public IActionResult AssignUser([FromBody] AssignUserDto assignUserDto)
    {
        _eventRepository.AssignUser(assignUserDto);
        return Ok();
    }

}