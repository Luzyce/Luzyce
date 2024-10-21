namespace Luzyce.Shared.Models.Kwit;

public class UpdateKwit
{
    public int Id { get; set; }
    public int QuantityNetto { get; set; } // from KwitPosition
    public int QuantityLoss { get; set; } // from KwitPosition
    public int QuantityToImprove { get; set; } // from KwitPosition
}