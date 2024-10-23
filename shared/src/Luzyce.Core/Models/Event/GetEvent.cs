using Luzyce.Shared.Models.User;

namespace Luzyce.Shared.Models.Event;

public class GetEvent
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public GetClient? Client { get; set; } = new();
    public string Operation { get; set; } = string.Empty;
    public GetUserResponseDto? User { get; set; } = new();
    public string? Hash { get; set; } = string.Empty;
}