using System.Security.Claims;
using System.Text.Json;
using Luzyce.Api.Mappers;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.ProductionOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/productionOrder")]
public class ProductionOrderController(ProductionOrderRepository productionOrderRepository, EventRepository eventRepository) : Controller
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        eventRepository.AddLog(User, "Pobrano zlecenia produkcji", null);
        return Ok(productionOrderRepository.GetProductionOrders());
    }
    
    [HttpPost]
    [Authorize]
    public IActionResult GetProdOrder(GetProdOrdersRequest request)
    {
        eventRepository.AddLog(User, "Pobrano zlecenia produkcji", JsonSerializer.Serialize(request));
        return Ok(productionOrderRepository.GetProductionOrders(request.Status));
    }
    
    [HttpGet("{id:int}")]
    [Authorize]
    public IActionResult Get(int id)
    {
        eventRepository.AddLog(User, "Pobrano zlecenie produkcji", JsonSerializer.Serialize(new {id}));
        return Ok(productionOrderRepository.GetProductionOrder(id));
    }
    
    [HttpGet("positions")]
    [Authorize]
    public IActionResult GetPositions()
    {
        eventRepository.AddLog(User, "Pobrano pozycje zlecenia produkcji", null);
        return Ok(productionOrderRepository.GetPositions());
    }
    
    [HttpPost("new")]
    [Authorize]
    public IActionResult CreateProductionOrder(CreateProductionOrderRequest createProductionOrderDto)
    {
        var operatorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        
        if (operatorId == 0)
        {
            eventRepository.AddLog(User, "Nie udało się utworzyć zlecenia produkcyjnego – nieautoryzowany użytkownik", JsonSerializer.Serialize(createProductionOrderDto));
            return Unauthorized();
        }
        
        var order = createProductionOrderDto.ToOrderFromCreateDto();
        
        var productionOrder = createProductionOrderDto.ToProductionOrderFromCreateDto();
        productionOrder.OperatorId = operatorId;
        
        var status = productionOrderRepository.SaveProdOrder(order, productionOrder);
        
        if (status == null)
        {
            eventRepository.AddLog(User, "Nie udało się utworzyć zlecenia produkcyjnego - wystąpił błąd podczas zapisu", JsonSerializer.Serialize(createProductionOrderDto));
            return Conflict();
        }

        eventRepository.AddLog(User, "Utworzono zlecenie produkcji", JsonSerializer.Serialize(createProductionOrderDto));

        return Ok(status);
    }
    
    [HttpPost("update/{id:int}")]
    [Authorize]
    public IActionResult UpdateProductionOrder(int id, UpdateProductionOrder updateProductionOrderDto)
    {
        var resp = productionOrderRepository.UpdateProdOrder(id, updateProductionOrderDto);
        
        if (resp == 0)
        {
            eventRepository.AddLog(User, "Nie udało się zaktualizować zlecenia produkcyjnego", JsonSerializer.Serialize(updateProductionOrderDto));
            return Conflict();
        }

        eventRepository.AddLog(User, "Zaktualizowano zlecenie produkcji", JsonSerializer.Serialize(updateProductionOrderDto));

        return Ok();
    }
    
    [HttpPost("getNorms")]
    [Authorize]
    public IActionResult GetNorms(GetNorms getNorms)
    {
        eventRepository.AddLog(User, "Pobrano normy", JsonSerializer.Serialize(getNorms));
        return Ok(productionOrderRepository.GetNorms(getNorms));
    }
    
    [HttpGet("archive/{id:int}")]
    [Authorize]
    public IActionResult ArchiveProductionOrder(int id)
    {
        var resp = productionOrderRepository.ArchiveProductionOrder(id);
        
        if (resp == 0)
        {
            eventRepository.AddLog(User, "Nie udało się zarchiwizować zlecenia produkcyjnego", JsonSerializer.Serialize(new {id}));
            return Conflict();
        }

        eventRepository.AddLog(User, "Zarchiwizowano zlecenie produkcji", JsonSerializer.Serialize(new {id}));

        return Ok();
    }
}
