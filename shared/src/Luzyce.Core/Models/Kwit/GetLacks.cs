namespace Luzyce.Shared.Models.Kwit;

public class GetLacks
{
    public int Quantity { get; set; }
    public string ErrorName { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
}
