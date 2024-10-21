using System.ComponentModel.DataAnnotations;

namespace Luzyce.Api.Db.AppDb.Models;

public class Shift
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    [Range(1, 3)]
    public int ShiftNumber { get; set; }
    public int? ShiftSupervisorId { get; set; }
    public User? ShiftSupervisor { get; set; }
}