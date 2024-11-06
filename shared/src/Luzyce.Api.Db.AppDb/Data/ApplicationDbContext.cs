using Luzyce.Api.Core.Dictionaries;
using Luzyce.Api.Db.AppDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Luzyce.Api.Db.AppDb.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions, IConfiguration config)
    : DbContext(dbContextOptions)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentItemRelationships> DocumentItemRelationships { get; set; }
    public DbSet<DocumentPositions> DocumentPositions { get; set; }
    public DbSet<DocumentRelations> DocumentRelations { get; set; }
    public DbSet<DocumentsDefinition> DocumentsDefinitions { get; set; }
    public DbSet<Error> Errors { get; set; }
    public DbSet<Lampshade> Lampshades { get; set; }
    public DbSet<LampshadeNorm> LampshadeNorms { get; set; }
    public DbSet<LampshadeVariant> LampshadeVariants { get; set; }
    public DbSet<Operation> Operations { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<OrderForProduction> OrdersForProduction { get; set; }
    public DbSet<OrderPositionForProduction> OrderPositionsForProduction { get; set; }
    public DbSet<ProductionPlan> ProductionPlans { get; set; }
    public DbSet<ProductionPlanPositions> ProductionPlanPositions { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerLampshade> CustomerLampshades { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Log> Logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseMySql(config.GetConnectionString("AppDbConnection"),
                ServerVersion.AutoDetect(config.GetConnectionString("AppDbConnection")))
            .UseValidationCheckConstraints(options => options.UseRegex(false));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var roles = new List<Role>
        {
            new()
            {
                Id = 1,
                Name = "Admin"
            },
            new()
            {
                Id = 2,
                Name = "User"
            },
            new()
            {
                Id = 3,
                Name = "Hutmustrz"
            },
            new()
            {
                Id = 4,
                Name = "Hutnik"
            },
        };

        modelBuilder.Entity<Role>().HasData(roles);

        var users = new List<User>
        {
            new()
            {
                Id = 1,
                Name = "Admin",
                LastName = "Admin",
                Email = "admin@gmail.com",
                Login = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Hash = "admin",
                RoleId = 1
            },
            new()
            {
                Id = 2,
                Name = "Przykładowy",
                LastName = "Hutmustrz",
                RoleId = 3
            },
            new()
            {
                Id = 3,
                Name = "Przykładowy",
                LastName = "Hutnik",
                RoleId = 4
            }
        };

        modelBuilder.Entity<User>().HasData(users);

        var documentsDefinitionsList = new List<DocumentsDefinition>
        {
            new()
            {
                Id = 1,
                Code = "KW",
                Name = "Kwit"
            },
            new()
            {
                Id = 2,
                Code = "ZP",
                Name = "Zlecenie Produkcji"
            }
        };

        modelBuilder.Entity<DocumentsDefinition>().HasData(documentsDefinitionsList);

        var statusList = new List<Status>
        {
            new()
            {
                Id = 1,
                Name = "Otwarty",
                Priority = 10
            },
            new()
            {
                Id = 2,
                Name = "Anulowany",
                Priority = 20
            },
            new()
            {
                Id = 3,
                Name = "Zamknięty",
                Priority = 30
            },
            new()
            {
                Id = 4,
                Name = "Anulowany",
                Priority = 40
            },
            new()
            {
                Id = 5,
                Name = "Zakończony",
                Priority = 50
            },
            new()
            {
                Id = 6,
                Name = "Zarchiwizowany",
                Priority = 60
            }
        };

        modelBuilder.Entity<Status>().HasData(statusList);

        var warehouseList = new List<Warehouse>
        {
            new()
            {
                Id = 1,
                Code = "M",
                Name = "Magazyn"
            },
            new()
            {
                Id = 2,
                Code = "P",
                Name = "Produkcja"
            }
        };

        modelBuilder.Entity<Warehouse>().HasData(warehouseList);

        var exampleDocumentList = new List<Document>
        {
            new()
            {
                Id = 1,
                DocNumber = 1,
                WarehouseId = warehouseList[0].Id,
                Year = 2023,
                Number = "M/0001/KW/2024",
                DocumentsDefinitionId = 1,
                OperatorId = users[0].Id,
                CreatedAt = DateTime.Now.ConvertToEuropeWarsaw(),
                UpdatedAt = DateTime.Now.ConvertToEuropeWarsaw(),
                StatusId = 1,
                ProductionPlanPositionsId = 1
            },
            new()
            {
                Id = 2,
                DocNumber = 1,
                WarehouseId = warehouseList[1].Id,
                Year = 2024,
                Number = "P/0001/ZP/2024",
                DocumentsDefinitionId = 2,
                OperatorId = users[0].Id,
                CreatedAt = DateTime.Now.ConvertToEuropeWarsaw(),
                UpdatedAt = DateTime.Now.ConvertToEuropeWarsaw(),
                StatusId = 1
            }
        };

        modelBuilder.Entity<Document>().HasData(exampleDocumentList);

        var exampleLampshade = new Lampshade
        {
            Id = 1,
            Code = "KL4124",
        };

        modelBuilder.Entity<Lampshade>().HasData(exampleLampshade);

        var exampleCustomer = new Customer
        {
            Id = 1,
            Name = "Test",
            Symbol = "TST"
        };

        modelBuilder.Entity<Customer>().HasData(exampleCustomer);

        var exampleOrderForProduction = new OrderForProduction
        {
            Id = 1,
            Date = DateTime.Now.ConvertToEuropeWarsaw(),
            Number = "1",
            OriginalNumber = "1",
            CustomerId = 1,
        };

        modelBuilder.Entity<OrderForProduction>().HasData(exampleOrderForProduction);

        var exampleOrderPositionForProduction = new OrderPositionForProduction
        {
            Id = 1,
            OrderId = 1,
            Order = null!,
            OrderNumber = "1",
            Symbol = "TEST",
            ProductId = exampleLampshade.Id,
            Product = null!,
            Description = "Test",
            OrderPositionLp = 1,
            Quantity = 1,
            QuantityInStock = 1,
            Unit = "szt",
            SerialNumber = "1",
            ProductSymbol = "KL4124",
            ProductName = "KL4124",
            ProductDescription = "Test"
        };

        modelBuilder.Entity<OrderPositionForProduction>().HasData(exampleOrderPositionForProduction);

        var lampshadeVariants = new List<LampshadeVariant>
        {
            new()
            {
                Id = 1,
                Name = "Opal",
                ShortName = ""
            },
            new()
            {
                Id = 2,
                Name = "Opal Mat",
                ShortName = "M"
            },
            new()
            {
                Id = 3,
                Name = "Opal Alabaster",
                ShortName = "AL"
            },
            new()
            {
                Id = 4,
                Name = "Opal Falbanka",
                ShortName = "FA"
            },
            new()
            {
                Id = 5,
                Name = "Jasny",
                ShortName = "J"
            },
            new()
            {
                Id = 6,
                Name = "Jasny Kier",
                ShortName = "J-KR"
            },
            new()
            {
                Id = 7,
                Name = "Jasny Pladry",
                ShortName = "J-PL"
            },
            new()
            {
                Id = 8,
                Name = "Jasny Antiko",
                ShortName = "J-AC"
            },
            new()
            {
                Id = 9,
                Name = "Jasny Alabaster",
                ShortName = "J-AL"
            },
            new()
            {
                Id = 10,
                Name = "Jasny Mat",
                ShortName = "J-M"
            },
            new()
            {
                Id = 11,
                Name = "Jasny Mrożony",
                ShortName = "J-MR"
            }
        };

        modelBuilder.Entity<LampshadeVariant>().HasData(lampshadeVariants);

        var exampleLampshadeNorm = new LampshadeNorm
        {
            Id = 1,
            LampshadeId = exampleLampshade.Id,
            Lampshade = null!,
            VariantId = lampshadeVariants[0].Id,
            Variant = null!,
            QuantityPerChange = 50,
            WeightBrutto = 3,
            WeightNetto = (decimal)0.45,
            MethodOfPackaging = "300x300x110",
            QuantityPerPack = 16
        };

        modelBuilder.Entity<LampshadeNorm>().HasData(exampleLampshadeNorm);

        var exampleDocumentPositionList = new List<DocumentPositions>
        {
            new()
            {
                Id = 1,
                DocumentId = exampleDocumentList[0].Id,
                Document = null!,
                QuantityNetto = 0,
                QuantityLoss = 0,
                QuantityToImprove = 0,
                QuantityGross = 0,
                OperatorId = users[0].Id,
                Operator = null!,
                StartTime = DateTime.Now.ConvertToEuropeWarsaw(),
                EndTime = null,
                LampshadeId = 1,
                Lampshade = null!
            },
            new()
            {
                Id = 2,
                DocumentId = exampleDocumentList[1].Id,
                Document = null!,
                QuantityNetto = 0,
                QuantityLoss = 0,
                QuantityToImprove = 0,
                QuantityGross = 0,
                OperatorId = users[0].Id,
                Operator = null!,
                StartTime = DateTime.Now.ConvertToEuropeWarsaw(),
                EndTime = null,
                LampshadeId = exampleLampshade.Id,
                Lampshade = null!,
                LampshadeNormId = exampleLampshadeNorm.Id,
                LampshadeNorm = null!,
                LampshadeDekor = "F",
                Remarks = "Test",
                OrderPositionForProductionId = exampleOrderPositionForProduction.Id,
                OrderPositionForProduction = null!,
                po_NumberOfChanges = 1,
                po_QuantityMade = 0,
                SubiektProductId = 2628
            }
        };

        modelBuilder.Entity<DocumentPositions>().HasData(exampleDocumentPositionList);

        var exampleShift = new Shift
        {
            Id = 1,
            Date = DateOnly.FromDateTime(DateTime.Now),
            ShiftNumber = 1,
            ShiftSupervisorId = users[0].Id,
            ShiftSupervisor = null!
        };

        modelBuilder.Entity<Shift>().HasData(exampleShift);

        var exampleProductionPlan = new ProductionPlan
        {
            Id = 1,
            Date = DateOnly.FromDateTime(DateTime.Now),
            ShiftId = 1,
            Team = 1,
            StatusId = 1,
            HeadsOfMetallurgicalTeamsId = 1,
            HeadsOfMetallurgicalTeams = null!
        };

        modelBuilder.Entity<ProductionPlan>().HasData(exampleProductionPlan);

        var exampleProductionPlanPosition = new ProductionPlanPositions
        {
            Id = 1,
            Quantity = 50,
            ProductionPlanId = exampleProductionPlan.Id,
            ProductionPlan = null!,
            DocumentPositionId = exampleDocumentPositionList[1].Id,
            DocumentPosition = null!,
            NumberOfHours = 8
        };

        modelBuilder.Entity<ProductionPlanPositions>().HasData(exampleProductionPlanPosition);

        var errorList = new List<Error>
        {
            new()
            {
                Id = 1,
                Code = "00",
                Name = "INNE",
                ShortName = "INN"
            },
            new()
            {
                Id = 2,
                Code = "01",
                Name = "KAMIENIE",
                ShortName = "KAM"
            },
            new()
            {
                Id = 3,
                Code = "02",
                Name = "KRĘTE",
                ShortName = "KR"
            },
            new()
            {
                Id = 4,
                Code = "03",
                Name = "GISZPA",
                ShortName = "GIS"
            },
            new()
            {
                Id = 5,
                Code = "04",
                Name = "PLADRY W SZKLE",
                ShortName = "PLS"
            },
            new()
            {
                Id = 6,
                Code = "05",
                Name = "PLADRY Z NABIERANIA",
                ShortName = "PLN"
            },
            new()
            {
                Id = 7,
                Code = "06",
                Name = "PLADRY PĘKAJĄCE",
                ShortName = "PLP"
            },
            new()
            {
                Id = 8,
                Code = "07",
                Name = "PLADRY OPALOWE",
                ShortName = "PLO"
            },
            new()
            {
                Id = 9,
                Code = "08",
                Name = "SMUGI",
                ShortName = "SMU"
            },
            new()
            {
                Id = 10,
                Code = "09",
                Name = "PASY",
                ShortName = "PAS"
            },
            new()
            {
                Id = 11,
                Code = "10",
                Name = "JASNE",
                ShortName = "JAS"
            },
            new()
            {
                Id = 12,
                Code = "11",
                Name = "RAUCH",
                ShortName = "RAU"
            },
            new()
            {
                Id = 13,
                Code = "12",
                Name = "NAGAR",
                ShortName = "NAG"
            },
            new()
            {
                Id = 14,
                Code = "13",
                Name = "POPĘKANE",
                ShortName = "POP"
            },
            new()
            {
                Id = 15,
                Code = "14",
                Name = "ZIMNA FORMA",
                ShortName = "ZFO"
            },
            new()
            {
                Id = 16,
                Code = "15",
                Name = "BRUDNA FORMA",
                ShortName = "BFO"
            },
            new()
            {
                Id = 17,
                Code = "16",
                Name = "BRUDNY BURGULEC",
                ShortName = "BBU"
            },
            new()
            {
                Id = 18,
                Code = "17",
                Name = "ZENDRA",
                ShortName = "ZEN"
            },
            new()
            {
                Id = 19,
                Code = "18",
                Name = "PRZERWANE SZKŁO",
                ShortName = "PRS"
            },
            new()
            {
                Id = 20,
                Code = "19",
                Name = "POMARSZCZONE",
                ShortName = "POM"
            },
            new()
            {
                Id = 21,
                Code = "20",
                Name = "SKALECZONE",
                ShortName = "SKA"
            },
            new()
            {
                Id = 22,
                Code = "21",
                Name = "ROZBERNA BAŃKA",
                ShortName = "RBA"
            },
            new()
            {
                Id = 23,
                Code = "22",
                Name = "CIENKIE",
                ShortName = "CIE"
            },
            new()
            {
                Id = 24,
                Code = "23",
                Name = "GRUBE",
                ShortName = "GRU"
            },
            new()
            {
                Id = 25,
                Code = "24",
                Name = "PŁASKIE",
                ShortName = "PŁA"
            },
            new()
            {
                Id = 26,
                Code = "25",
                Name = "PRZEDMUCHANE",
                ShortName = "PRZ"
            },
            new()
            {
                Id = 27,
                Code = "26",
                Name = "NIEDODMUCHANE",
                ShortName = "NDO"
            },
            new()
            {
                Id = 28,
                Code = "27",
                Name = "WYCIĄGNIĘTE",
                ShortName = "WYC"
            },
            new()
            {
                Id = 29,
                Code = "28",
                Name = "ZAPCHANE",
                ShortName = "ZAP"
            },
            new()
            {
                Id = 30,
                Code = "29",
                Name = "FRETY",
                ShortName = "FRE"
            },
            new()
            {
                Id = 31,
                Code = "30",
                Name = "RYGLE",
                ShortName = "RYG"
            },
            new()
            {
                Id = 32,
                Code = "31",
                Name = "ZATARTE",
                ShortName = "ZAT"
            },
            new()
            {
                Id = 33,
                Code = "32",
                Name = "PRZYPALONE",
                ShortName = "PRZ"
            },
            new()
            {
                Id = 34,
                Code = "33",
                Name = "POPĘKANE NA PALNIKU",
                ShortName = "PNP"
            }
        };

        modelBuilder.Entity<Error>().HasData(errorList);
    }
}
