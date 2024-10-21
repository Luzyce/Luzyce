namespace Luzyce.Shared.Models.Document;

public class GetDocumentByQrCodeResponseDto
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public List<GetDocumentPositionResponseDto>? DocumentPositions { get; set; } = [];
}
