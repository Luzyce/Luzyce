﻿using System.Text.RegularExpressions;
using Luzyce.Api.Core.Dictionaries;
using Luzyce.Shared.Models.Document;
using Luzyce.Shared.Models.Lampshade;
using Luzyce.Shared.Models.ProductionOrder;
using Luzyce.Shared.Models.User;
using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Api.Db.AppDb.Models;
using Luzyce.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Document = Luzyce.Api.Db.AppDb.Models.Document;
using DocumentPositions = Luzyce.Api.Db.AppDb.Models.DocumentPositions;
using Lampshade = Luzyce.Api.Db.AppDb.Models.Lampshade;
using LampshadeNorm = Luzyce.Api.Db.AppDb.Models.LampshadeNorm;
using OrderForProduction = Luzyce.Api.Db.AppDb.Models.OrderForProduction;
using OrderPositionForProduction = Luzyce.Api.Db.AppDb.Models.OrderPositionForProduction;

namespace Luzyce.Api.Repositories;

public class ProductionOrderRepository(ApplicationDbContext applicationDbContext)
{
    private readonly ApplicationDbContext applicationDbContext = applicationDbContext;

    public GetProductionOrdersResponse GetProductionOrders(int status = 0)
    {
        var statusesToShow = status switch {
            0 =>
            [
                1, 2, 3, 4, 5, 6
            ],
            _ => new List<int> {status}
        };
        
        return new GetProductionOrdersResponse
        {
            ProductionOrders = applicationDbContext.Documents
                .Where(d => d.DocumentsDefinitionId == DocumentsDefinitions.ZP_ID && statusesToShow.Contains(d.StatusId))
                .Include(d => d.Warehouse)
                .Include(d => d.DocumentsDefinition)
                .Include(d => d.Operator)
                .Include(d => d.Status)
                .Include(d => d.Order)
                .Include(d => d.Order!.Customer)
                .Select(d => new GetProductionOrderForList
                {
                    Id = d.Id,
                    OrderDate = d.CreatedAt,
                    OrderNumber = d.Order!.Number,
                    CustomerName = d.Order!.Customer!.Name,
                    ProdOrderDate = d.CreatedAt,
                    ProdOrderNumber = d.Number,
                    DeliveryDate = d.Order!.DeliveryDate,
                    Status = new GetStatusResponseDto
                    {
                        Id = d.Status!.Id,
                        Name = d.Status.Name,
                        Priority = d.Status.Priority
                    }
                })
                .ToList()
        };
    }

    public GetProductionOrder? GetProductionOrder(int Id)
    {
        var document = applicationDbContext.Documents
            .Where(d => d.DocumentsDefinitionId == DocumentsDefinitions.ZP_ID && d.Id == Id)
            .Include(d => d.Warehouse)
            .Include(d => d.DocumentsDefinition)
            .Include(d => d.Operator)
            .Include(d => d.Status).Include(document => document.Order!).ThenInclude(orderForProduction => orderForProduction.Customer!)
            .FirstOrDefault();

        if (document == null)
        {
            return null;
        }

        var positions = applicationDbContext.DocumentPositions
            .Where(dp => dp.DocumentId == Id)
            .Include(dp => dp.Lampshade)
            .Include(dp => dp.LampshadeNorm)
            .ThenInclude(ln => ln!.Variant)
            .Include(dp => dp.OrderPositionForProduction)
            .Select(dp => new GetProductionOrderPosition
            {
                Id = dp.Id,
                QuantityNetto = dp.QuantityNetto,
                QuantityGross = dp.QuantityGross,
                QuantityOnPlans = dp.ProductionPlanPositions.Sum(pp => pp.Quantity),
                QuantitiesOnPlans = dp.ProductionPlanPositions
                    .Select(pp => new GetQuantityOnPlan
                    {
                        Quantity = pp.Quantity,
                        QuantityNetto = pp.Kwit.First().DocumentPositions.First().QuantityNetto,
                        QuantityLoss = pp.Kwit.First().DocumentPositions.First().QuantityLoss,
                        QuantityToImprove = pp.Kwit.First().DocumentPositions.First().QuantityToImprove,
                        Date = pp.ProductionPlan!.Date,
                        Shift = pp.ProductionPlan.Shift!.ShiftNumber,
                        Team = pp.ProductionPlan.Team,
                        KwitId = pp.Kwit.First().Id.ToString(),
                        KwitName = pp.Kwit.First().Number,
                        KwitNumber = pp.Kwit.First().DocNumber.ToString()
                    })
                    .ToList(),
                ExecutionDate = dp.EndTime,
                Lampshade = new GetLampshade
                {
                    Id = dp.Lampshade!.Id,
                    Code = dp.Lampshade.Code
                },
                LampshadeNorm = new GetLampshadeNorm
                {
                    Id = dp.LampshadeNorm!.Id,
                    Lampshade = new GetLampshade
                    {
                        Id = dp.Lampshade.Id,
                        Code = dp.Lampshade.Code
                    },
                    Variant = new GetVariantResponseDto
                    {
                        Id = dp.LampshadeNorm.Variant!.Id,
                        Name = dp.LampshadeNorm.Variant.Name,
                        ShortName = dp.LampshadeNorm.Variant.ShortName
                    },
                    QuantityPerChange = dp.LampshadeNorm.QuantityPerChange ?? 0,
                    MethodOfPackaging = dp.LampshadeNorm.MethodOfPackaging,
                    QuantityPerPack = dp.LampshadeNorm.QuantityPerPack ?? 0,
                },
                LampshadeDekor = dp.LampshadeDekor,
                Remarks = dp.Remarks,
                CustomerLampshadeNumber =
                    applicationDbContext
                        .CustomerLampshades
                        .FirstOrDefault(cl =>
                            cl.Customer == dp.OrderPositionForProduction!.Order!.Customer &&
                            cl.Lampshade == dp.Lampshade &&
                            cl.LampshadeNorm == dp.LampshadeNorm &&
                            cl.LampshadeDekor == dp.LampshadeDekor)!.CustomerSymbol,
                NumberOfChanges = dp.po_NumberOfChanges,
                QuantityMade = dp.po_QuantityMade,
                ProductId = dp.SubiektProductId ?? 0,
                Unit = dp.OrderPositionForProduction!.Unit!
            })
            .ToList();

        return new GetProductionOrder
        {
            Id = document.Id,
            DocNumber = document.DocNumber,
            Warehouse = new GetWarehouseResponseDto
            {
                Id = document.Warehouse!.Id,
                Code = document.Warehouse.Code
            },
            Year = document.Year,
            Number = document.Number,
            DocumentsDefinition = new GetDocumentsDefinitionResponseDto
            {
                Id = document.DocumentsDefinition!.Id,
                Code = document.DocumentsDefinition.Code
            },
            User = new GetUserResponseDto
            {
                Id = document.Operator!.Id,
                Name = document.Operator.Name
            },
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt,
            ClosedAt = document.ClosedAt,
            Status = new GetStatusResponseDto
            {
                Id = document.Status!.Id,
                Name = document.Status.Name,
                Priority = document.Status.Priority
            },
            ClientName = document.Order?.Customer?.Name ?? "",
            Positions = positions
        };
    }
        
    public GetOrdersPositionsResponse GetPositions()
    {
        return new GetOrdersPositionsResponse
        {
            OrdersPositions = new List<GetProductionOrderPosition>(applicationDbContext.DocumentPositions
                .Include(dp => dp.Document)
                .Include(dp => dp.Lampshade)
                .Include(dp => dp.LampshadeNorm)
                    .ThenInclude(ln => ln!.Variant)
                .Include(dp => dp.OrderPositionForProduction)
                .Include(dp => dp.OrderPositionForProduction!.Order)
                .Include(dp => dp.OrderPositionForProduction!.Order!.Customer)
                .Where(dp => dp.Document!.DocumentsDefinitionId == DocumentsDefinitions.ZP_ID && dp.IsDeleted == false)
                .Select(dp => new GetProductionOrderPosition
                {
                    Id = dp.Id,
                    QuantityNetto = dp.QuantityNetto,
                    QuantityGross = dp.QuantityGross,
                    QuantityOnPlans = dp.ProductionPlanPositions.Sum(pp => pp.Quantity),
                    QuantitiesOnPlans = dp.ProductionPlanPositions
                        .Select(pp => new GetQuantityOnPlan
                        {
                            Quantity = pp.Quantity,
                            QuantityNetto = pp.Kwit.First().DocumentPositions.First().QuantityNetto,
                            QuantityLoss = pp.Kwit.First().DocumentPositions.First().QuantityLoss,
                            QuantityToImprove = pp.Kwit.First().DocumentPositions.First().QuantityToImprove,
                            Date = pp.ProductionPlan!.Date,
                            Shift = pp.ProductionPlan.Shift!.ShiftNumber,
                            Team = pp.ProductionPlan.Team,
                            KwitId = pp.Kwit.First().Id.ToString(),
                            KwitName = pp.Kwit.First().Number,
                            KwitNumber = pp.Kwit.First().DocNumber.ToString()
                        })
                        .ToList(),
                    ExecutionDate = dp.EndTime,
                    Lampshade = new GetLampshade
                    {
                        Id = dp.Lampshade!.Id,
                        Code = dp.Lampshade.Code
                    },
                    LampshadeNorm = new GetLampshadeNorm
                    {
                        Id = dp.LampshadeNorm!.Id,
                        Lampshade = new GetLampshade
                        {
                            Id = dp.Lampshade.Id,
                            Code = dp.Lampshade.Code
                        },
                        Variant = new GetVariantResponseDto
                        {
                            Id = dp.LampshadeNorm.Variant!.Id,
                            Name = dp.LampshadeNorm.Variant.Name,
                            ShortName = dp.LampshadeNorm.Variant.ShortName
                        },
                        QuantityPerChange = dp.LampshadeNorm.QuantityPerChange ?? 0,
                        MethodOfPackaging = dp.LampshadeNorm.MethodOfPackaging,
                        QuantityPerPack = dp.LampshadeNorm.QuantityPerPack ?? 0
                    },
                    LampshadeDekor = dp.LampshadeDekor,
                    Remarks = dp.Remarks,
                    NumberOfChanges = dp.po_NumberOfChanges,
                    QuantityMade = dp.po_QuantityMade,
                    ProductId = dp.SubiektProductId ?? 0,
                    Unit = dp.OrderPositionForProduction!.Unit!,
                    ProductionOrderNumber = dp.Document!.Number,
                    Client = dp.OrderPositionForProduction.Order!.Customer!.Name,
                    Priority = dp.Priority ?? 0
                })
                .ToList()
                .OrderBy(x => x.Priority))
        };
    }
    
    public GetOrdersPositionsResponse GetDeletedPositions()
    {
        return new()
        {
            OrdersPositions = new(applicationDbContext.DocumentPositions
                .Include(dp => dp.Document)
                .Include(dp => dp.Lampshade)
                .Include(dp => dp.LampshadeNorm)
                    .ThenInclude(ln => ln!.Variant)
                .Include(dp => dp.OrderPositionForProduction)
                .Include(dp => dp.OrderPositionForProduction!.Order)
                .Include(dp => dp.OrderPositionForProduction!.Order!.Customer)
                .Where(dp => dp.Document!.DocumentsDefinitionId == DocumentsDefinitions.ZP_ID && dp.IsDeleted == true)
                .Select(dp => new GetProductionOrderPosition
                {
                    Id = dp.Id,
                    QuantityNetto = dp.QuantityNetto,
                    QuantityGross = dp.QuantityGross,
                    QuantityOnPlans = dp.ProductionPlanPositions.Sum(pp => pp.Quantity),
                    QuantitiesOnPlans = dp.ProductionPlanPositions
                        .Select(pp => new GetQuantityOnPlan
                        {
                            Quantity = pp.Quantity,
                            QuantityNetto = pp.Kwit.First().DocumentPositions.First().QuantityNetto,
                            QuantityLoss = pp.Kwit.First().DocumentPositions.First().QuantityLoss,
                            QuantityToImprove = pp.Kwit.First().DocumentPositions.First().QuantityToImprove,
                            Date = pp.ProductionPlan!.Date,
                            Shift = pp.ProductionPlan.Shift!.ShiftNumber,
                            Team = pp.ProductionPlan.Team,
                            KwitId = pp.Kwit.First().Id.ToString(),
                            KwitName = pp.Kwit.First().Number,
                            KwitNumber = pp.Kwit.First().DocNumber.ToString()
                        })
                        .ToList(),
                    ExecutionDate = dp.EndTime,
                    Lampshade = new()
                    {
                        Id = dp.Lampshade!.Id,
                        Code = dp.Lampshade.Code
                    },
                    LampshadeNorm = new()
                    {
                        Id = dp.LampshadeNorm!.Id,
                        Lampshade = new()
                        {
                            Id = dp.Lampshade.Id,
                            Code = dp.Lampshade.Code
                        },
                        Variant = new()
                        {
                            Id = dp.LampshadeNorm.Variant!.Id,
                            Name = dp.LampshadeNorm.Variant.Name,
                            ShortName = dp.LampshadeNorm.Variant.ShortName
                        },
                        QuantityPerChange = dp.LampshadeNorm.QuantityPerChange ?? 0,
                        MethodOfPackaging = dp.LampshadeNorm.MethodOfPackaging,
                        QuantityPerPack = dp.LampshadeNorm.QuantityPerPack ?? 0
                    },
                    LampshadeDekor = dp.LampshadeDekor,
                    Remarks = dp.Remarks,
                    NumberOfChanges = dp.po_NumberOfChanges,
                    QuantityMade = dp.po_QuantityMade,
                    ProductId = dp.SubiektProductId ?? 0,
                    Unit = dp.OrderPositionForProduction!.Unit!,
                    ProductionOrderNumber = dp.Document!.Number,
                    Client = dp.OrderPositionForProduction.Order!.Customer!.Name,
                    Priority = dp.Priority ?? 0
                })
                .ToList()
                .OrderByDescending(x => x.ExecutionDate)
                .ThenBy(x => x.Priority))
        };
    }
    
    public int RestorePosition(int id)
    {
        var position = applicationDbContext.DocumentPositions
            .FirstOrDefault(dp => dp.Id == id);

        if (position == null)
        {
            return 0;
        }

        position.IsDeleted = false;
        position.EndTime = null;
        position.Priority = null;
        applicationDbContext.SaveChanges();

        return 1;
    }
    
    public int DeletePosition(int id)
    {
        var position = applicationDbContext.DocumentPositions
            .FirstOrDefault(dp => dp.Id == id);

        if (position == null)
        {
            return 0;
        }

        position.IsDeleted = true;
        applicationDbContext.SaveChanges();

        return 1;
    }

    public int? SaveProdOrder(Order order, ProductionOrder productionOrder)
    {
        using var transaction = applicationDbContext.Database.BeginTransaction();

        try
        {
            var existingOrder = applicationDbContext.OrdersForProduction
                .Include(orderForProduction => orderForProduction.Customer)
                .FirstOrDefault(o => o.SubiektId == order.Id);

            var customer = existingOrder?.Customer ?? applicationDbContext.Customers
                .FirstOrDefault(c => c.Id == order.CustomerId);

            if (customer == null)
            {
                customer = new Customer
                {
                    Id = order.CustomerId,
                    Name = order.CustomerName,
                    Symbol = order.CustomerSymbol
                };

                applicationDbContext.Customers.Add(customer);
            }
            
            var version = applicationDbContext.OrdersForProduction
                .Where(o => o.SubiektId == order.SubiektId)
                .Select(o => o.Version)
                .ToList()
                .DefaultIfEmpty(0)
                .Max() + 1;

            var orderForProduction = new OrderForProduction
            {
                Date = order.Date,
                Number = order.Number,
                OriginalNumber = order.OriginalNumber,
                CustomerId = customer.Id,
                DeliveryDate = order.DeliveryDate,
                SubiektId = order.SubiektId,
                Version = version,
            };
            applicationDbContext.OrdersForProduction.Add(orderForProduction);

            foreach (var position in order.Positions)
            {
                var lampshadeCode = string.IsNullOrEmpty(Regex.Match(position.Symbol, @"^[A-Z]{2}\d{3,4}").Value)
                    ? position.Symbol
                    : Regex.Match(position.Symbol, @"^[A-Z]{2}\d{3,4}").Value;

                var lampshade = applicationDbContext.Lampshades
                    .FirstOrDefault(l => l.Code == lampshadeCode);

                if (lampshade == null)
                {
                    lampshade = new Lampshade
                    {
                        Code = lampshadeCode
                    };
                    applicationDbContext.Lampshades.Add(lampshade);
                }

                var orderPositionForProduction = new OrderPositionForProduction
                {
                    Order = orderForProduction,
                    OrderNumber = order.Number,
                    SubiektId = position.Id,
                    Symbol = position.Symbol,
                    Product = lampshade,
                    Description = position.Description,
                    OrderPositionLp = position.OrderPositionLp,
                    Quantity = position.Quantity,
                    QuantityInStock = position.QuantityInStock,
                    Unit = position.Unit,
                    SerialNumber = position.SerialNumber,
                    ProductSymbol = position.ProductSymbol,
                    ProductName = position.ProductName,
                    ProductDescription = position.ProductDescription
                };
                applicationDbContext.OrderPositionsForProduction.Add(orderPositionForProduction);
            }

            applicationDbContext.SaveChanges();

            return createProdOrder(productionOrder, orderForProduction, transaction);
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }

    public int UpdateProdOrder(int id, UpdateProductionOrder updateProductionOrderDto)
    {
        using var transaction = applicationDbContext.Database.BeginTransaction();
        try
        {
            foreach (var position in updateProductionOrderDto.Positions)
            {
                var existingPosition = applicationDbContext.DocumentPositions
                    .Include(dp => dp.LampshadeNorm)
                    .Include(documentPositions => documentPositions.OrderPositionForProduction!)
                    .ThenInclude(orderPositionForProduction => orderPositionForProduction.Order!)
                    .ThenInclude(orderForProduction => orderForProduction.Customer!)
                    .Include(documentPositions => documentPositions.Lampshade!)
                    .Where(dp => dp.DocumentId == id)
                    .FirstOrDefault(dp => dp.Id == position.Id);
            
                if (existingPosition?.LampshadeNorm == null)
                {
                    transaction.Rollback();
                    return 0;
                }

                // CustomerLampshadeNumber =
                //     applicationDbContext
                //         .CustomerLampshades
                //         .FirstOrDefault(cl =>
                //             cl.Customer == dp.OrderPositionForProduction!.Order!.Customer &&
                //             cl.Lampshade == dp.Lampshade &&
                //             cl.LampshadeNorm == dp.LampshadeNorm &&
                //             cl.LampshadeDekor == dp.LampshadeDekor)!.CustomerSymbol,

                var customerLampshade = applicationDbContext
                    .CustomerLampshades
                    .FirstOrDefault(cl =>
                        cl.Customer == existingPosition.OrderPositionForProduction!.Order!.Customer &&
                        cl.Lampshade == existingPosition.Lampshade &&
                        cl.LampshadeNorm == existingPosition.LampshadeNorm &&
                        cl.LampshadeDekor == existingPosition.LampshadeDekor);

                if (position.CustomerLampshadeNumber is not null or "" &&
                    customerLampshade is not null &&
                    customerLampshade.CustomerSymbol != position.CustomerLampshadeNumber)
                {
                    customerLampshade.CustomerSymbol = position.CustomerLampshadeNumber;
                    applicationDbContext.SaveChanges();
                }

                if (position.CustomerLampshadeNumber is not null or "" &&
                    customerLampshade is null)
                {
                    customerLampshade = new CustomerLampshade
                    {
                        CustomerId = existingPosition.OrderPositionForProduction!.Order!.Customer!.Id,
                        LampshadeId = existingPosition.Lampshade!.Id,
                        LampshadeNormId = existingPosition.LampshadeNorm!.Id,
                        LampshadeDekor = existingPosition.LampshadeDekor,
                        CustomerSymbol = position.CustomerLampshadeNumber
                    };
                    applicationDbContext.CustomerLampshades.Add(customerLampshade);
                    applicationDbContext.SaveChanges();
                }

                if (position.CustomerLampshadeNumber is null or "" &&
                    customerLampshade is not null)
                {
                    applicationDbContext.CustomerLampshades.Remove(customerLampshade);
                    applicationDbContext.SaveChanges();
                }
            
                existingPosition.QuantityNetto = position.QuantityNetto;
                existingPosition.po_NumberOfChanges = position.NumberOfChanges;
                existingPosition.LampshadeNorm.QuantityPerChange = position.QuantityPerChange;
                existingPosition.LampshadeNorm.MethodOfPackaging = position.MethodOfPackaging;
                existingPosition.LampshadeNorm.QuantityPerPack = position.QuantityPerPack;
                existingPosition.EndTime = position.ExecutionDate;
                existingPosition.po_QuantityMade = position.QuantityMade;
                // existingPosition.CustomerLampshadeNumber = position.CustomerLampshadeNumber;
                existingPosition.Remarks = position.Remarks;
            }

            applicationDbContext.SaveChanges();

            transaction.Commit();

            return 1;
        }
        catch
        {
            transaction.Rollback();
            return 0;
        }
    }
    
    public GetNormsResponse GetNorms(GetNorms getNormsRequest)
    {
        var norms = new GetNormsResponse();
        
        foreach (var norm in getNormsRequest.Norms)
        {
            var lampshade = applicationDbContext.Lampshades
                .FirstOrDefault(l => l.Code == norm.Lampshade);
            var variant = applicationDbContext.LampshadeVariants
                .FirstOrDefault(l => l.Name == norm.Variant);

            if (lampshade == null || variant == null)
            {
                norms.Norms.Add(new GetNormResponse
                {
                    Norm = 0
                });
                continue;
            }

            var lampshadeNorms = applicationDbContext.LampshadeNorms
                .FirstOrDefault(l => l.LampshadeId == lampshade.Id
                                     && l.VariantId == variant.Id);

            norms.Norms.Add(new GetNormResponse
            {
                Norm = lampshadeNorms?.QuantityPerChange ?? 0
            });
        }
        
        return norms;
    }
    
    public int ArchiveProductionOrder(int id)
    {
        var document = applicationDbContext.Documents
            .FirstOrDefault(d => d.Id == id);

        if (document == null)
        {
            return 0;
        }

        document.StatusId = 6;
        
        foreach (var position in applicationDbContext.DocumentPositions
            .Where(dp => dp.DocumentId == id))
        {
            position.IsDeleted = true;
        }
        
        applicationDbContext.SaveChanges();

        return 1;
    }
    
    private int? createProdOrder(ProductionOrder productionOrder, OrderForProduction orderForProduction, IDbContextTransaction transaction)
    {
        try
        {
            var currentYear = DateTime.Now.ConvertToEuropeWarsaw().Year;
            var docNumber = applicationDbContext.Documents
                .Where(d => d.WarehouseId == Warehouses.PROD_ID
                            && d.Year == currentYear
                            && d.DocumentsDefinitionId == DocumentsDefinitions.ZP_ID)
                .Select(d => d.DocNumber)
                .ToList()
                .DefaultIfEmpty(0)
                .Max() + 1;

            var document = new Document
            {
                DocNumber = docNumber,
                WarehouseId = Warehouses.PROD_ID,
                Year = currentYear,
                Number = $"{Warehouses.PROD_CODE}/{docNumber:D4}/{DocumentsDefinitions.ZP_CODE}/{currentYear}",
                DocumentsDefinitionId = DocumentsDefinitions.ZP_ID,
                OperatorId = productionOrder.OperatorId,
                CreatedAt = DateTime.Now.ConvertToEuropeWarsaw(),
                UpdatedAt = DateTime.Now.ConvertToEuropeWarsaw(),
                ClosedAt = null,
                StatusId = 1,
                Order = orderForProduction,
            };
                
            applicationDbContext.Documents.Add(document);

            foreach (var position in productionOrder.ProductionOrderPositions)
            {
                var lampshade = applicationDbContext.Lampshades
                    .FirstOrDefault(l => l.Code == position.Symbol);
                
                if (lampshade == null)
                {
                    var code = Regex.Match(position.Symbol, @"^[A-ZĄĆĘŁŃÓŚŹŻ]{2}\d{3,4}").Value;
                    
                    if (code == "")
                    {
                        transaction.Rollback();
                        return null;
                    }
                    
                    lampshade = new()
                    {
                        Code = code
                    };
                    applicationDbContext.Lampshades.Add(lampshade);
                }
                
                var variant = applicationDbContext.LampshadeVariants
                    .FirstOrDefault(l => l.Id == position.VariantId);

                if (variant == null)
                {
                    transaction.Rollback();
                    return null;
                }
                    
                var lampshadeNorms = applicationDbContext.LampshadeNorms
                    .FirstOrDefault(l => l.Lampshade == lampshade
                                         && l.Variant == variant);
                    
                if (lampshadeNorms == null)
                {
                    lampshadeNorms = new()
                    {
                        Lampshade = lampshade,
                        Variant = variant,
                    };
                    applicationDbContext.LampshadeNorms.Add(lampshadeNorms);
                }
                
                var orderPositionForProduction = applicationDbContext.OrderPositionsForProduction
                    .FirstOrDefault(op => op.SubiektId == position.DocumentPositionId);

                var documentPosition = new DocumentPositions
                {
                    Document = document,
                    QuantityNetto = position.Net,
                    QuantityGross = position.Gross,
                    OperatorId = productionOrder.OperatorId,
                    StartTime = DateTime.Now.ConvertToEuropeWarsaw(),
                    Lampshade = lampshade,
                    LampshadeNorm = lampshadeNorms,
                    LampshadeDekor = position.Dekor,
                    OrderPositionForProductionId = orderPositionForProduction?.Id,
                    SubiektProductId = position.SubiektProductId,
                };
                applicationDbContext.DocumentPositions.Add(documentPosition);
            }

            applicationDbContext.SaveChanges();

            transaction.Commit();
            return document.Id;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }
}
