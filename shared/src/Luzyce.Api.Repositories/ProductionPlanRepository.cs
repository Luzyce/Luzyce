using Luzyce.Api.Core.Dictionaries;
using Luzyce.Shared.Models.Document;
using Luzyce.Shared.Models.Lampshade;
using Luzyce.Shared.Models.ProductionOrder;
using Luzyce.Shared.Models.ProductionPlan;
using Luzyce.Shared.Models.User;
using Luzyce.Api.Db.AppDb.Data;
using Luzyce.Api.Db.AppDb.Models;
using Microsoft.EntityFrameworkCore;

namespace Luzyce.Api.Repositories;

public class ProductionPlanRepository(ApplicationDbContext applicationDbContext)
{
    public GetProductionPlans GetProductionPlans(GetMonthProductionPlanRequest request)
    {
        return new GetProductionPlans
        {
            ProductionPlans = applicationDbContext.ProductionPlans
                .Where(x => x.Date.Month == request.ProductionPlanDate.Month && x.Date.Year == request.ProductionPlanDate.Year)
                .Select(x => new GetProductionPlanForCalendar
                {
                    Id = x.Id,
                    Date = x.Date,
                    Shift = (x.Shift == null ? null : new GetShift
                    {
                        Id = x.Shift.Id,
                        Date = x.Shift.Date,
                        ShiftNumber = x.Shift.ShiftNumber,
                        ShiftSupervisor = x.Shift.ShiftSupervisor == null ? null : new GetUserResponseDto
                        {
                            Id = x.Shift.ShiftSupervisor.Id,
                            Name = x.Shift.ShiftSupervisor.Name,
                            LastName = x.Shift.ShiftSupervisor.LastName
                        }
                    })!,
                    Team = x.Team,
                    Status = x.Status == null ? null : new GetStatusResponseDto
                    {
                        Id = x.Status.Id,
                        Name = x.Status.Name
                    }
                })
                .ToList()
        };
    }
    
    public int AddPositionsToProductionPlan(AddPositionsToProductionPlan request)
    {
        using var transaction = applicationDbContext.Database.BeginTransaction();
        try
        {
            var shift = applicationDbContext.Shifts
                .FirstOrDefault(x => x.Date == request.Date && x.ShiftNumber == request.Shift);
            var productionPlan = applicationDbContext.ProductionPlans
                .FirstOrDefault(x => x.Date == request.Date && x.Team == request.Team && x.Shift == shift);
                
            
            if (productionPlan == null)
            {
                if (shift == null)
                {
                    shift = new Shift
                    {
                        Date = request.Date,
                        ShiftNumber = request.Shift
                    };
                
                    applicationDbContext.Shifts.Add(shift);
                    applicationDbContext.SaveChanges();
                }
                
                productionPlan = new ProductionPlan
                {
                    Date = request.Date,
                    ShiftId = shift.Id,
                    Team = request.Team,
                    StatusId = 1
                };
                applicationDbContext.ProductionPlans.Add(productionPlan);
                applicationDbContext.SaveChanges();
            }
                
            foreach (var position in request.Positions)
            {
                var productionPlanPositions = new ProductionPlanPositions
                {
                    ProductionPlanId = productionPlan.Id,
                    Quantity = position.Value,
                    DocumentPositionId = position.Key
                };
                    
                applicationDbContext.ProductionPlanPositions.Add(productionPlanPositions);
                applicationDbContext.SaveChanges();
                    
                var currentYear = DateTime.Now.ConvertToEuropeWarsaw().Year;
                var docNumber = applicationDbContext.Documents
                    .Where(d => d.WarehouseId == Warehouses.MAG_ID
                                && d.Year == currentYear
                                && d.DocumentsDefinitionId == DocumentsDefinitions.KW_ID)
                    .Select(d => d.DocNumber)
                    .ToList()
                    .DefaultIfEmpty(0)
                    .Max() + 1;
                    
                var kwit = new Document
                {
                    DocNumber = docNumber,
                    WarehouseId = Warehouses.MAG_ID,
                    Year = currentYear,
                    Number = $"{Warehouses.MAG_CODE}/{docNumber:D4}/{DocumentsDefinitions.KW_CODE}/{currentYear}",
                    DocumentsDefinitionId = DocumentsDefinitions.KW_ID,
                    OperatorId = 1,
                    CreatedAt = DateTime.Now.ConvertToEuropeWarsaw(),
                    UpdatedAt = DateTime.Now.ConvertToEuropeWarsaw(),
                    ClosedAt = null,
                    StatusId = 1,
                    ProductionPlanPositionsId = productionPlanPositions.Id
                };
                    
                applicationDbContext.Documents.Add(kwit);
                applicationDbContext.SaveChanges();
                    
                var documentPosition = applicationDbContext.DocumentPositions
                    .Include(dp => dp.Document)
                    .Include(dp => dp.Lampshade)
                    .Include(dp => dp.LampshadeNorm)
                    .ThenInclude(ln => ln!.Variant)
                    .Include(op => op.OrderPositionForProduction)
                    .FirstOrDefault(dp => dp.Id == position.Key);
                    
                if (documentPosition == null)
                {
                   transaction.Rollback();
                   return 0;
                }

                var newDocumentPosition = new DocumentPositions
                {
                    DocumentId = kwit.Id,
                    OperatorId = documentPosition.OperatorId,
                    StartTime = DateTime.Now.ConvertToEuropeWarsaw(),
                    LampshadeId = documentPosition.LampshadeId,
                    LampshadeNormId = documentPosition.LampshadeNormId,
                    Remarks = documentPosition.Remarks,
                    OrderPositionForProductionId = documentPosition.OrderPositionForProductionId
                };
                    
                applicationDbContext.DocumentPositions.Add(newDocumentPosition);
            }
                
            applicationDbContext.SaveChanges();
            transaction.Commit();
                
            return 1;
        }
        catch (Exception)
        {
            transaction.Rollback();
            return 0;
        }
    }
    
    public GetProductionPlan GetProductionPlan(GetProductionPlanPositionsRequest request)
    {
        var productionPlan = applicationDbContext.ProductionPlans
            .Include(x => x.Shift)
                .ThenInclude(x => x!.ShiftSupervisor)
            .Include(x => x.Status)
            .Include(x => x.HeadsOfMetallurgicalTeams)
            .FirstOrDefault(x => x.Date == request.Date && x.Shift!.ShiftNumber == request.Shift && x.Team == request.Team);
        
        if (productionPlan == null)
        {
            return new GetProductionPlan();
        }
        
        return new GetProductionPlan
        {
            Id = productionPlan.Id,
            Date = productionPlan.Date,
            Shift = (productionPlan.Shift == null ? null : new GetShift
            {
                Id = productionPlan.Shift.Id,
                Date = productionPlan.Shift.Date,
                ShiftNumber = productionPlan.Shift.ShiftNumber,
                ShiftSupervisor = productionPlan.Shift.ShiftSupervisor == null ? null : new GetUserResponseDto
                {
                    Id = productionPlan.Shift.ShiftSupervisor.Id,
                    Name = productionPlan.Shift.ShiftSupervisor.Name,
                    LastName = productionPlan.Shift.ShiftSupervisor.LastName
                }
            })!,
            Team = productionPlan.Team,
            Status = productionPlan.Status == null ? null : new GetStatusResponseDto
            {
                Id = productionPlan.Status.Id,
                Name = productionPlan.Status.Name
            },
            HeadsOfMetallurgicalTeams = productionPlan.HeadsOfMetallurgicalTeams == null ? null : new GetUserResponseDto
            {
                Id = productionPlan.HeadsOfMetallurgicalTeams.Id,
                Name = productionPlan.HeadsOfMetallurgicalTeams.Name,
                LastName = productionPlan.HeadsOfMetallurgicalTeams.LastName
            },
            Remarks = productionPlan.Remarks,
            ProductionPlanPositions = applicationDbContext.ProductionPlanPositions
                .Where(x => x.ProductionPlan!.Id == productionPlan.Id)
                .Include(x => x.DocumentPosition)
                .ThenInclude(dp => dp!.Document)
                .Include(x => x.DocumentPosition)
                .ThenInclude(dp => dp!.Lampshade)
                .Include(x => x.DocumentPosition)
                .ThenInclude(dp => dp!.LampshadeNorm)
                .ThenInclude(ln => ln!.Variant)
                .Include(x => x.DocumentPosition)
                .ThenInclude(dp => dp!.LampshadeNorm)
                .ThenInclude(ln => ln!.Lampshade)
                .Include(x => x.DocumentPosition)
                .ThenInclude(dp => dp!.OrderPositionForProduction)
                .ThenInclude(op => op!.Order)
                .ThenInclude(orderForProduction => orderForProduction!.Customer!)
                .Include(x => x.Kwit)
                .ThenInclude(k => k.DocumentPositions)
                .ToList()
                .Select(x => new GetProductionPlanPosition
                {
                    Id = x.Id,
                    Quantity = x.Quantity,
                    DocumentPosition = new GetProductionOrderPosition
                    {
                        Id = x.DocumentPosition!.Id,
                        QuantityNetto = x.DocumentPosition.QuantityNetto,
                        QuantityGross = x.DocumentPosition.QuantityGross,
                        ExecutionDate = x.DocumentPosition.EndTime,
                        Lampshade = new GetLampshade
                        {
                            Id = x.DocumentPosition.Lampshade!.Id,
                            Code = x.DocumentPosition.Lampshade.Code
                        },
                        LampshadeNorm = new GetLampshadeNorm()
                        {
                            Id = x.DocumentPosition.LampshadeNorm!.Id,
                            Lampshade = new GetLampshade
                            {
                                Id = x.DocumentPosition.LampshadeNorm.Lampshade!.Id,
                                Code = x.DocumentPosition.LampshadeNorm.Lampshade.Code
                            },
                            Variant = new GetVariantResponseDto
                            {
                                Id = x.DocumentPosition.LampshadeNorm.Variant!.Id,
                                Name = x.DocumentPosition.LampshadeNorm.Variant.Name,
                                ShortName = x.DocumentPosition.LampshadeNorm.Variant.ShortName
                            },
                            QuantityPerChange = x.DocumentPosition.LampshadeNorm.QuantityPerChange ?? 0,
                            WeightBrutto = x.DocumentPosition.LampshadeNorm.WeightBrutto,
                            WeightNetto = x.DocumentPosition.LampshadeNorm.WeightNetto,
                            MethodOfPackaging = x.DocumentPosition.LampshadeNorm.MethodOfPackaging,
                            QuantityPerPack = x.DocumentPosition.LampshadeNorm.QuantityPerPack,
                        },
                        LampshadeDekor = x.DocumentPosition.LampshadeDekor,
                        Remarks = x.DocumentPosition.Remarks,
                        NumberOfChanges = x.DocumentPosition.po_NumberOfChanges,
                        QuantityMade = x.DocumentPosition.po_QuantityMade,
                        ProductId = x.DocumentPosition.SubiektProductId ?? 0,
                        Unit = x.DocumentPosition.OrderPositionForProduction!.Unit!,
                        ProductionOrderNumber = x.DocumentPosition.Document!.Number,
                        Client = x.DocumentPosition.OrderPositionForProduction.Order!.Customer!.Name,
                        Priority = x.DocumentPosition.Priority ?? 0
                    },
                    NumberOfHours = x.NumberOfHours,
                    Kwit = x.Kwit.Count == 0 ? null : new GetDocumentWithPositions
                    {
                        Id = x.Kwit.First().Id,
                        Number = x.Kwit.First().Number,
                        DocumentPositions = x.Kwit.First().DocumentPositions
                            .Select(dp => new GetDocumentPositionResponseDto
                            {
                                Id = dp.Id,
                                QuantityNetto = dp.QuantityNetto,
                                QuantityLoss = dp.QuantityLoss,
                                QuantityToImprove = dp.QuantityToImprove
                            })
                            .ToList()
                    }
                })
                .ToList()
        };
    }
    
    public void DeletePosition(int id)
    {
        var position = applicationDbContext.ProductionPlanPositions
            .Include(productionPlanPositions => productionPlanPositions.Kwit)
            .ThenInclude(kwit => kwit.DocumentPositions)
            .FirstOrDefault(x => x.Id == id);

        if (position == null)
        {
            return;
        }
        
        var productionPlan = applicationDbContext.ProductionPlans.FirstOrDefault(x => x.Id == position.ProductionPlanId);
        
        if (productionPlan == null)
        {
            return;
        }
        
        var kwit = position.Kwit.FirstOrDefault();
        
        var kwitPosition = kwit?.DocumentPositions.FirstOrDefault();
        
        if (applicationDbContext.ProductionPlanPositions.Count(x => x.ProductionPlanId == productionPlan.Id) == 1)
        {
            applicationDbContext.ProductionPlans.Remove(productionPlan);
        }
        
        applicationDbContext.ProductionPlanPositions.Remove(position);
        if (kwitPosition != null) applicationDbContext.DocumentPositions.Remove(kwitPosition);
        if (kwit != null) applicationDbContext.Documents.Remove(kwit);
        applicationDbContext.SaveChanges();
    }

    public IEnumerable<GetUserResponseDto> ShiftSupervisor()
    {
        return applicationDbContext.Users
            .Where(x => x.Role!.Id == UserRoles.SHIFT_SUPERVISOR_ID)
            .Select(x => new GetUserResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                LastName = x.LastName
            })
            .ToList();
    }
    
    public IEnumerable<GetUserResponseDto> GetHeadsOfMetallurgicalTeams()
    {
        return applicationDbContext.Users
            .Where(x => x.Role!.Id == UserRoles.HEADS_OF_METALLURGICAL_TEAMS_ID)
            .Select(x => new GetUserResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                LastName = x.LastName
            })
            .ToList();
    }
    
    public void UpdateProductionPlan(UpdateProductionPlan request)
    {
        var productionPlan = applicationDbContext.ProductionPlans
            .Include(productionPlan => productionPlan.Shift)
            .Include(productionPlan => productionPlan.Positions)
            .ThenInclude(pp => pp.DocumentPosition!.LampshadeNorm)
            .Include(productionPlan => productionPlan.Positions).ThenInclude(productionPlanPositions => productionPlanPositions.Kwit)
            .FirstOrDefault(x => x.Id == request.Id);
    
        if (productionPlan == null)
        {
            return;
        }
    
        productionPlan.Shift!.ShiftSupervisorId = request.ShiftSupervisorId;
        productionPlan.HeadsOfMetallurgicalTeamsId = request.HeadsOfMetallurgicalTeamsId;
        productionPlan.Remarks = request.Remarks;
        //
        // if (request.ProductionPlanPositions.All(x => x.QuantityPerChange != null) &&
        //     request.ProductionPlanPositions.Sum(x => x.Quantity / (decimal)x.QuantityPerChange! * 8) == 8)
        // {
        //     productionPlan.StatusId = productionPlan.Positions.All(x => x.Kwit.All(k => k.StatusId == 3)) ? 5 : 3;
        // }
        // else
        // {
        //     productionPlan.StatusId = 1;
        // }

        
        var positionDictionary = productionPlan.Positions.ToDictionary(p => p.Id);
        
        foreach (var position in request.ProductionPlanPositions)
        {
            if (!positionDictionary.TryGetValue(position.Id, out var productionPlanPosition))
            {
                continue;
            }
            
            productionPlanPosition.Quantity = position.Quantity;
            productionPlanPosition.NumberOfHours = position.NumberOfHours;
            
            productionPlanPosition.DocumentPosition!.LampshadeNorm!.WeightNetto = position.WeightNetto;
            productionPlanPosition.DocumentPosition!.LampshadeNorm!.WeightBrutto = position.WeightBrutto;
            productionPlanPosition.DocumentPosition!.LampshadeNorm!.QuantityPerChange = position.QuantityPerChange;
        }
    
        applicationDbContext.SaveChanges();
    }

    public Document? GetKwit(int id)
    {
        var document = applicationDbContext.Documents
            .Include(d => d.Warehouse)
            .Include(d => d.Operator)
            .Include(d => d.Status)
            .Include(d => d.DocumentsDefinition)
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.ProductionPlan)
            .ThenInclude(pp => pp!.HeadsOfMetallurgicalTeams)
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.ProductionPlan)
            .ThenInclude(pp => pp!.Shift)
            .ThenInclude(s => s!.ShiftSupervisor)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.Lampshade)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.LampshadeNorm)
            .ThenInclude(ln => ln!.Variant)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.LampshadeNorm)
            .ThenInclude(ln => ln!.Lampshade)
            .Include(d => d.DocumentPositions)
            .ThenInclude(dp => dp.OrderPositionForProduction)
            .ThenInclude(op => op!.Order)
            .ThenInclude(order => order!.Customer)
            .Include(d => d.ProductionPlanPositions)
            .ThenInclude(ppp => ppp!.DocumentPosition)
            .ThenInclude(dp => dp!.LampshadeNorm)
            .Where(x => x.DocumentsDefinitionId == DocumentsDefinitions.KW_ID)
            .FirstOrDefault(x => x.Id == id);

        return document ?? null;
    }
    
    public List<ProductionPlan> GetProductionPlanPdf(DateOnly data)
    {
        return applicationDbContext.ProductionPlans
            .Include(x => x.Shift)
            .ThenInclude(x => x!.ShiftSupervisor)
            .Include(x => x.Status)
            .Include(x => x.Positions)
            .ThenInclude(x => x.Kwit)
            .ThenInclude(x => x.DocumentPositions)
            .ThenInclude(x => x.Lampshade)
            .Include(x => x.Positions)
            .ThenInclude(x => x.Kwit)
            .ThenInclude(x => x.DocumentPositions)
            .ThenInclude(x => x.LampshadeNorm)
            .ThenInclude(x => x!.Variant)
            .Include(x => x.Positions)
            .ThenInclude(x => x.Kwit)
            .ThenInclude(x => x.DocumentPositions)
            .ThenInclude(x => x.OrderPositionForProduction)
            .ThenInclude(x => x!.Order)
            .ThenInclude(x => x!.Customer)
            .Include(x => x.HeadsOfMetallurgicalTeams)
            .Include(x => x.Positions)
            .ThenInclude(x => x.DocumentPosition)
            .ThenInclude(x => x!.LampshadeNorm)
            .Where(x => x.Date == data)
            .ToList();
    } 
    
    public List<User?> GetShiftsSupervisors(DateOnly date)
    {
        var shifts = applicationDbContext.Shifts
            .Include(x => x.ShiftSupervisor)
            .Where(x => x.Date == date)
            .ToList();
        
        var shiftSupervisors = new List<User?>();

        for (var i = 0; i < 3; i++)
        {
            var shift = shifts.FirstOrDefault(x => x.ShiftNumber == i + 1);
            shiftSupervisors.Add(shift?.ShiftSupervisor);
        }
        
        return shiftSupervisors;
    }
    
    private static Luzyce.Api.Domain.Models.Warehouse WarehouseDomainFromDb(Warehouse warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        return new Luzyce.Api.Domain.Models.Warehouse
        {
            Id = warehouse.Id,
            Code = warehouse.Code,
            Name = warehouse.Name
        };
    }

    private static Luzyce.Api.Domain.Models.User UserDomainFromDb(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new Luzyce.Api.Domain.Models.User
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            Login = user.Login,
            Password = user.Password,
            Hash = user.Hash,
            CreatedAt = user.CreatedAt,
            RoleId = user.RoleId
        };
    }
    public static Luzyce.Api.Domain.Models.Role RoleDomainFromDb(Luzyce.Api.Db.AppDb.Models.Role role)
    {
        ArgumentNullException.ThrowIfNull(role);

        return new Luzyce.Api.Domain.Models.Role
        {
            Id = role.Id,
            Name = role.Name
        };
    }

    private static Luzyce.Api.Domain.Models.Status StatusDomainFromDb(Status status)
    {
        ArgumentNullException.ThrowIfNull(status);

        return new Luzyce.Api.Domain.Models.Status
        {
            Id = status.Id,
            Name = status.Name,
            Priority = status.Priority
        };
    }

    private static Luzyce.Api.Domain.Models.DocumentsDefinition DocumentsDefinitionDomainFromDb(DocumentsDefinition documentsDefinition)
    {
        ArgumentNullException.ThrowIfNull(documentsDefinition);

        return new Luzyce.Api.Domain.Models.DocumentsDefinition
        {
            Id = documentsDefinition.Id,
            Code = documentsDefinition.Code,
            Name = documentsDefinition.Name
        };
    }
}
