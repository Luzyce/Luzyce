namespace Luzyce.Shared.Models.User;

public class GetUserForUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public bool IsLocked { get; set; }
}
