using System.Text.Json;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.ProductionPriority;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/productionPriority")]
public class ProductionPriorityController(ProductionPriorityRepository productionPriorityRepository, EventRepository eventRepository) : Controller
{
    private readonly ProductionPriorityRepository productionPriorityRepository = productionPriorityRepository;
    private readonly EventRepository _eventRepository = eventRepository;
    
    [HttpPost("updatePriorities")]
    [Authorize]
    public IActionResult UpdatePriorities([FromBody] UpdateProductionPrioritiesRequest updateProductionPrioritiesRequest)
    {
        var status = productionPriorityRepository.UpdatePriorities(updateProductionPrioritiesRequest);
        
        if (status == 0)
        {
            _eventRepository.AddLog(User, "Nie udało się zaktualizować priorytetów - wystąpił błąd podczas aktualizacji", JsonSerializer.Serialize(updateProductionPrioritiesRequest));
            return Conflict();
        }

        _eventRepository.AddLog(User, "Zaktualizowano priorytety", JsonSerializer.Serialize(updateProductionPrioritiesRequest));

        return Ok();
    }
}