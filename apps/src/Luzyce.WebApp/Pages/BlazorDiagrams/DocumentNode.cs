using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Luzyce.WebApp.Pages.BlazorDiagrams;

public class DocumentNode(Point? position = null) : NodeModel(position)
{
    public bool IsSelected { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string AddrToRedirect { get; set; } = string.Empty;
}