using System.ComponentModel.DataAnnotations;

namespace Luzyce.Api.Domain.Models;

public class Warehouse
{
    public int Id { get; set; }
    [Required]
    [MinLength(1)]
    [MaxLength(2)]
    public required string Code { get; set; }
    public required string Name { get; set; }


}
