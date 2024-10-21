namespace Luzyce.Shared.Models.Lampshade;

public class GetLampshadeNorm
{
    public int Id { get; set; }
    public GetLampshade Lampshade { get; set; } = new GetLampshade();
    public GetVariantResponseDto Variant { get; set; } = new GetVariantResponseDto();
    public int QuantityPerChange { get; set; }
    public decimal? WeightBrutto { get; set; }
    public decimal? WeightNetto { get; set; }
    public string? MethodOfPackaging { get; set; } = string.Empty;
    public int? QuantityPerPack { get; set; }
}