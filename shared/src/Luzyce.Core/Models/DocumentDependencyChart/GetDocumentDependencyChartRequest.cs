namespace Luzyce.Shared.Models.DocumentDependencyChart;

public class GetDocumentDependencyChartRequest
{
    public int DocumentId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
}