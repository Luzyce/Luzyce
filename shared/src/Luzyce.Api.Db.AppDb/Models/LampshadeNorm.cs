using Microsoft.EntityFrameworkCore;

namespace Luzyce.Api.Db.AppDb.Models;

public class LampshadeNorm
{
    public int Id { get; set; }
    public int LampshadeId { get; set; }
    public Lampshade? Lampshade { get; set; }
    public int VariantId { get; set; }
    public LampshadeVariant? Variant { get; set; }
    public int? QuantityPerChange { get; set; }
    [Precision(8, 2)]
    public decimal? WeightBrutto { get; set; }
    [Precision(8, 2)]
    public decimal? WeightNetto { get; set; }
    public string? MethodOfPackaging { get; set; }
    public int? QuantityPerPack { get; set; }
}