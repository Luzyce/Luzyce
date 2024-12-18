namespace Luzyce.Objects.Lampshades;

public class LampshadeNorm
{
    public int Id { get; set; }
    public int? QuantityPerChange { get; set; }
    public decimal? WeightBrutto { get; set; }
    public decimal? WeightNetto { get; set; }
    public string? MethodOfPackaging { get; set; }
    public int QuantityPerPack { get; set; }
}
