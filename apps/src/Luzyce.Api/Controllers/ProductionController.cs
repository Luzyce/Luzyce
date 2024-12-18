using Luzyce.Objects.Orders.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/prod")]
public class ProductionController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        try 
        {
           return Ok(new OrdersPositionsListItem(
               ));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}
