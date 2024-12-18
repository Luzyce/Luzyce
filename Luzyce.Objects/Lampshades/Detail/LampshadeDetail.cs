namespace Luzyce.Objects.Lampshades.Detail;

public class LampshadeDetail
{    
    public string Code => Lampshade.Code;
    public string Variant => LampshadeVariant.Name;
    public string VariantShortName => LampshadeVariant.ShortName;
    public string Dekor { get; set; } = string.Empty;
    public int? QuantityPerChange => LampshadeNorm.QuantityPerChange;
    public decimal? WeightBrutto => LampshadeNorm.WeightBrutto;
    public decimal? WeightNetto => LampshadeNorm.WeightNetto;
    public string? MethodOfPackaging => LampshadeNorm.MethodOfPackaging;
    public int QuantityPerPack => LampshadeNorm.QuantityPerPack;
    
    public Lampshade Lampshade { get; set; } = new();
    public LampshadeVariant LampshadeVariant { get; set; } = new();
    public LampshadeNorm LampshadeNorm { get; set; } = new();
}
