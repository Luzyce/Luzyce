using System.ComponentModel.DataAnnotations;
using Luzyce.Api.Core.Dictionaries;

namespace Luzyce.Api.Db.AppDb.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now.ConvertToEuropeWarsaw();
    public int RoleId { get; set; }

    [Required]
    public Role? Role { get; set; }
}
