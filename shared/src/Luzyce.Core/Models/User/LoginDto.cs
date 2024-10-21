namespace Luzyce.Shared.Models.User;

public class LoginDto
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
}
