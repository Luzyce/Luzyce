using Blazor.Diagrams.Core.Controls;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Luzyce.WebApp.Pages.BlazorDiagrams;

public class NodeInformationControl(List<string>? positions) : Control
{
    public List<string>? Positions { get; set; } = positions;

    public override Point? GetPosition(Model model)
    {
        var node = (model as NodeModel)!;
        return node.Size == null ? null : node.Position.Add(0, node.Size!.Height + 10);
    }
}
