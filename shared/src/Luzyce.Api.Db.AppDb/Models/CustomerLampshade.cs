namespace Luzyce.Api.Db.AppDb.Models;

public class CustomerLampshade
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public int LampshadeId { get; set; }
    public Lampshade? Lampshade { get; set; }
    public int LampshadeNormId { get; set; }
    public LampshadeNorm? LampshadeNorm { get; set; }
    public string LampshadeDekor { get; set; } = string.Empty;
    public string CustomerSymbol { get; set; } = string.Empty;
}