using Luzyce.Api.Domain.Models;
using Luzyce.Shared.Models.ProductionOrder;

namespace Luzyce.Api.Mappers;

public static class ProductionOrderMappers
{
    public static Order ToOrderFromCreateDto(this CreateProductionOrderRequest dto)
    {
        return new Order()
        {
            Id = dto.Order.Id,
            Date = dto.Order.Date,
            Number = dto.Order.Number,
            CustomerId = dto.Order.CustomerId,
            CustomerSymbol = dto.Order.CustomerSymbol,
            CustomerName = dto.Order.CustomerName,
            DeliveryDate = dto.Order.DeliveryDate,
            Status = dto.Order.Status,
            Positions = dto.Order.Positions.Select(x => new OrderPosition
            {
                Id = x.Id,
                OrderId = x.OrderId,
                OrderNumber = x.OrderNumber,
                Symbol = x.Symbol,
                ProductId = x.ProductId,
                Description = x.Description,
                OrderPositionLp = x.OrderPositionLp,
                Quantity = x.Quantity,
                QuantityInStock = x.QuantityInStock,
                Unit = x.Unit,
                SerialNumber = x.SerialNumber,
                ProductSymbol = x.ProductSymbol,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription
            }).ToList()
        };
    } 
    public static ProductionOrder ToProductionOrderFromCreateDto(this CreateProductionOrderRequest dto)
    {
        return new ProductionOrder()
        {
            OperatorId = 0,
            ProductionOrderPositions = dto.ProductionOrderPositions.Select(x => new ProductionOrderPosition
            {
                DocumentPositionId = x.DocumentPositionId,
                Symbol = x.Symbol,
                VariantId = x.VariantId,
                Dekor = x.Dekor,
                Gross = x.Gross,
                Net = x.Net,
                SubiektProductId = x.SubiektProductId
            }).ToList()
        };
    }
}