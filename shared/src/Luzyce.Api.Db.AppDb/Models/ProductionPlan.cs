using System.ComponentModel.DataAnnotations;

namespace Luzyce.Api.Db.AppDb.Models;

public class ProductionPlan
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int? ShiftId { get; set; }
    public Shift? Shift { get; set; }
    [Range(1, 3)]
    public int Team { get; set; }
    public int StatusId { get; set; }
    public Status? Status { get; set; }
    public int? HeadsOfMetallurgicalTeamsId { get; set; }
    public User? HeadsOfMetallurgicalTeams { get; set; }
    public string? Remarks { get; set; }
    
    public List<ProductionPlanPositions> Positions { get; set; } = [];
}