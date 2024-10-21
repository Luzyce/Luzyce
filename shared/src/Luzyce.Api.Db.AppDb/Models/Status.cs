namespace Luzyce.Api.Db.AppDb.Models;

public class Status
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Priority { get; set; }
}
