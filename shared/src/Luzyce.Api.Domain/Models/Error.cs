namespace Luzyce.Api.Domain.Models;

public class Error
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string ShortName { get; set; }
    public required string Name { get; set; }
}
