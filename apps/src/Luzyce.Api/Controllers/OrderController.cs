using System.Text.Json;
using Luzyce.Api.Mappers;
using Luzyce.Api.Repositories;
using Luzyce.Shared.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luzyce.Api.Controllers;

[ApiController]
[Route("api/order")]
public class OrderController(OrderRepository orderRepository, LogRepository logRepository) : Controller
{
    private readonly OrderRepository orderRepository = orderRepository;
    private readonly LogRepository logRepository = logRepository;

    [HttpPost("{offset}")]
    [Authorize]
    public IActionResult Get(int offset, GetOrdersDto getOrdersDto)
    {
        logRepository.AddLog(User, "Pobrano zamówienia z Subiekta", JsonSerializer.Serialize(new {offset, getOrdersDto}));
        var response = orderRepository.GetOrders(offset: offset, ordersFilters: getOrdersDto.ToOrdersFiltersFromDto());
        return Ok(new GetOrdersResponseDto
        {
            CurrentPage = response.CurrentPage,
            TotalPages = response.TotalPages,
            TotalOrders = response.TotalOrders,
            Orders = response.Orders.Select(x => new GetOrderResponseDto
            {
                Id = x.Id,
                Date = x.Date,
                Number = x.Number,
                OriginalNumber = x.OriginalNumber,
                CustomerId = x.CustomerId,
                CustomerSymbol = x.CustomerSymbol,
                CustomerName = x.CustomerName,
                DeliveryDate = x.DeliveryDate,
                Status = x.Status,
                Remarks = x.Remarks,
                Positions = x.Positions.Select(y => new GetOrderPositionResponseDto
                {
                    Id = y.Id,
                    OrderId = y.OrderId,
                    OrderNumber = y.OrderNumber,
                    Symbol = y.Symbol,
                    ProductId = y.ProductId,
                    Description = y.Description,
                    OrderPositionLp = y.OrderPositionLp,
                    Quantity = y.Quantity,
                    QuantityInStock = y.QuantityInStock,
                    Unit = y.Unit,
                    SerialNumber = y.SerialNumber,
                    ProductSymbol = y.ProductSymbol,
                    ProductName = y.ProductName,
                    ProductDescription = y.ProductDescription

                }).ToList()
            }).ToList()
        });
    }

    [HttpGet("{offset}/{limit}")]
    [Authorize]
    public IActionResult Get(int offset, int limit)
    {
        logRepository.AddLog(User, "Pobrano zamówienia z Subiekta", JsonSerializer.Serialize(new {offset, limit}));
        return Ok(orderRepository.GetOrders(offset: offset, limit: limit));
    }

    [HttpGet("positions/{orderId}")]
    [Authorize]
    public IActionResult GetPositions(int orderId)
    {
        logRepository.AddLog(User, "Pobrano pozycje zamówienia z Subiekta", JsonSerializer.Serialize(orderId));
        return Ok(orderRepository.GetOrderPositions(orderId));
    }
    
    [HttpPost("stock")]
    [Authorize]
    public IActionResult GetWarehousesLevels(StockRequest stockRequest)
    {
        logRepository.AddLog(User, "Pobrano stany magazynowe", JsonSerializer.Serialize(stockRequest));
        var response = orderRepository.GetWarehousesLevels(stockRequest);
        return Ok(response);
    }
}
