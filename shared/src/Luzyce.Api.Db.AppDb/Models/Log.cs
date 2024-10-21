namespace Luzyce.Api.Db.AppDb.Models;

public class Log
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int? ClientId { get; set; }
    public Client? Client { get; set; }
    public string Operation { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string? Hash { get; set; }
    public string? Data { get; set; }
}