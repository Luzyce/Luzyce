namespace Luzyce.Api.Domain.Models;

public class ProductionOrderPosition
{
    public int DocumentPositionId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public int VariantId { get; set; }
    public string Dekor { get; set; } = string.Empty;
    public int Gross { get; set; }
    public int Net { get; set; }
    public int SubiektProductId { get; set; }
}