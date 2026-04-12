namespace Luzyce.Shared.Models.ProductionOrder;

public class GetProductDefaultsResponse
{
    public Dictionary<int, ProductDefaultValues> Defaults { get; set; } = new();
}

public class ProductDefaultValues
{
    public string LampshadeCode { get; set; } = string.Empty;
    public int VariantId { get; set; }
    public string Dekor { get; set; } = string.Empty;
}
