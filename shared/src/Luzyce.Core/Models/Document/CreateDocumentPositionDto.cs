namespace Luzyce.Shared.Models.Document;

public class CreateDocumentPositionDto
{
    public int QuantityNetto { get; set; }
    public int QuantityLoss { get; set; } = 0;
    public int QuantityToImprove { get; set; } = 0;
    public int QuantityGross { get; set; } = 0;
    public int LampshadeId { get; set; }
}
