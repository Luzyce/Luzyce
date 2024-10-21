using Luzyce.Shared.Models.User;

namespace Luzyce.Shared.Models.Document;

public class GetDocumentResponseDto
{
    public int Id { get; set; }
    public int DocNumber { get; set; }
    public GetWarehouseResponseDto? Warehouse { get; set; } = new GetWarehouseResponseDto();
    public int Year { get; set; }
    public string? Number { get; set; }
    public GetDocumentsDefinitionResponseDto? DocumentsDefinition { get; set; } = new GetDocumentsDefinitionResponseDto();
    public GetUserResponseDto? User { get; set; } = new GetUserResponseDto();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public GetStatusResponseDto? Status { get; set; } = new GetStatusResponseDto();
}
