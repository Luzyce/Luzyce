namespace Luzyce.Shared.Models.ProductionOrder;

public class UpdateProductionOrderPosition
{
    public int Id { get; set; }
    public int QuantityNetto { get; set; }
    public decimal? NumberOfChanges { get; set; }
    public int? QuantityPerChange { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public int? QuantityMade { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public string? CustomerLampshadeNumber { get; set; }
    public string? MethodOfPackaging { get; set; }
    public int? QuantityPerPack { get; set; }
}