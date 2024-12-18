namespace Luzyce.Shared.Models.Production;

public class GetProduct
{
    public int Id { get; set; }
    public DateOnly? Date { get; set; }
    public int Shift { get; set; }
    public int Team { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string Lampshade { get; set; } = string.Empty;
    public string LampshadeVariant { get; set; } = string.Empty;
    public string ShiftSupervisor { get; set; } = string.Empty;
    public string HeadsOfMetallurgicalTeams { get; set; } = string.Empty;
    public int QuantityNetto { get; set; }
    public int QuantityLoss { get; set; }
    public int QuantityToImprove { get; set; }
    public int QuantityGross { get; set; }
    public decimal? WeightGross { get; set; }
    public decimal? WeightNetto { get; set; }
}
