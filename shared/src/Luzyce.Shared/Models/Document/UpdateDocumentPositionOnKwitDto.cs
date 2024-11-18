namespace Luzyce.Shared.Models.Document;

public class UpdateDocumentPositionOnKwitDto
{
    public char Type { get; set; }
    public required string Field { get; set; }
    public string? ErrorCode { get; set; }
    public int Quantity { get; set; } = 1;
}
