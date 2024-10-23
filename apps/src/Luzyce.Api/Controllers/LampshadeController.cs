using System.Text.Json;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.Lampshade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/lampshade")]
public class LampshadeController(LampshadeRepository lampshadeRepository, EventRepository eventRepository) : Controller
{
    private readonly LampshadeRepository lampshadeRepository = lampshadeRepository;
    private readonly EventRepository _eventRepository = eventRepository;
    
    [HttpGet("variants")]
    [Authorize]
    public IActionResult GetLampshadeVariants()
    {
        _eventRepository.AddLog(User, "Pobrano warianty kloszy", null);
        return Ok(new GetVariantsResponseDto
        {
            Variants = lampshadeRepository.GetLampshadeVariants().Select(x => new GetVariantResponseDto
                {
                    Id = x.Id,
                    ShortName = x.ShortName,
                    Name = x.Name
                }).ToList()
        });
    }
    
    [HttpGet("variants/{shortName}")]
    [Authorize]
    public IActionResult GetLampshadeVariant(string shortName)
    {
        var variant = lampshadeRepository.GetLampshadeVariant(shortName);
        
        if (variant == null)
        {
            _eventRepository.AddLog(User, "Nie udało się uzyskać wariantu klosza – wariant nie został znaleziony", JsonSerializer.Serialize(new {shortName}));
            return NotFound();
        }

        _eventRepository.AddLog(User, "Pobrano wariant klosza", JsonSerializer.Serialize(new {shortName}));

        return Ok(new GetVariantResponseDto()
        {
            Id = variant.Id,
            Name = variant.Name,
            ShortName = variant.ShortName
        });
    }
}

