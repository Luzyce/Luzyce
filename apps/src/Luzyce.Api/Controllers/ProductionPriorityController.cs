using System.Text.Json;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.ProductionPriority;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/productionPriority")]
public class ProductionPriorityController(ProductionPriorityRepository productionPriorityRepository,ProductionOrderRepository productionOrderRepository, EventRepository eventRepository) : Controller
{
    [HttpGet("getDeletedPositions")]
    [Authorize]
    public IActionResult GetDeletedPositions()
    {
        eventRepository.AddLog(User, "Pobrano usunięte pozycje zlecenia produkcji", null);
        return Ok(productionOrderRepository.GetDeletedPositions());
    }
    
    [HttpGet("restorePosition/{id:int}")]
    [Authorize]
    public IActionResult RestorePosition(int id)
    {
        var status = productionOrderRepository.RestorePosition(id);
        
        if (status == 0)
        {
            eventRepository.AddLog(User, "Nie udało się przywrócić pozycji zlecenia produkcji - wystąpił błąd podczas przywracania", JsonSerializer.Serialize(new {id}));
            return Conflict();
        }

        eventRepository.AddLog(User, "Przywrócono pozycję zlecenia produkcji", JsonSerializer.Serialize(new {id}));

        return Ok();
    }
    
    [HttpDelete("deletePosition/{id:int}")]
    [Authorize]
    public IActionResult DeletePosition(int id)
    {
        var status = productionOrderRepository.DeletePosition(id);
        
        if (status == 0)
        {
            eventRepository.AddLog(User, "Nie udało się usunąć pozycji zlecenia produkcji - wystąpił błąd podczas usuwania", JsonSerializer.Serialize(new {id}));
            return Conflict();
        }

        eventRepository.AddLog(User, "Usunięto pozycję zlecenia produkcji", JsonSerializer.Serialize(new {id}));

        return Ok();
    }

    [HttpPost("updatePriorities")]
    [Authorize]
    public IActionResult UpdatePriorities([FromBody] UpdateProductionPrioritiesRequest updateProductionPrioritiesRequest)
    {
        var status = productionPriorityRepository.UpdatePriorities(updateProductionPrioritiesRequest);
        
        if (status == 0)
        {
            eventRepository.AddLog(User, "Nie udało się zaktualizować priorytetów - wystąpił błąd podczas aktualizacji", JsonSerializer.Serialize(updateProductionPrioritiesRequest));
            return Conflict();
        }

        eventRepository.AddLog(User, "Zaktualizowano priorytety", JsonSerializer.Serialize(updateProductionPrioritiesRequest));

        return Ok();
    }
}
