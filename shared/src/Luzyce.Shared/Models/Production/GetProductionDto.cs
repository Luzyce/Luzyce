namespace Luzyce.Shared.Models.Production;

public class GetProductionDto
{
    public DateOnly SelectedMonth { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}
