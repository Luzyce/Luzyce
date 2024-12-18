namespace Luzyce.Objects.Users;

public class User
{
    public int Id { get; set; }
    public string FullName => $"{Name} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Role Role { get; set; } = new();
    
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
