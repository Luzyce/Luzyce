namespace Luzyce.Shared.Models.DocumentDependencyChart;

public class GetDocumentDependencyChart
{
    public int Id { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string AddrToRedirect { get; set; } = string.Empty;
    public List<string>? Positions { get; set; } = null;
    public List<GetDocumentDependencyChart>? Derivatives { get; set; } = null;
}