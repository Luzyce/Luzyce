namespace Luzyce.Api.Db.AppDb.Models;

public class Client
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}