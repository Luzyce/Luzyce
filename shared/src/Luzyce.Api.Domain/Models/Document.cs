namespace Luzyce.Api.Domain.Models;

public class Document
{
    public int Id { get; set; }
    public int DocNumber { get; set; }
    public int WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    public int Year { get; set; }
    public string? Number { get; set; }
    public int DocumentsDefinitionId { get; set; }
    public DocumentsDefinition? DocumentsDefinition { get; set; }
    public int OperatorId { get; set; }
    public User? Operator { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public int StatusId { get; set; }
    public Status? Status { get; set; }
    public int? LockedById { get; set; }
    public Client? LockedBy { get; set; }
}
