namespace Luzyce.Api.Db.AppDb.Models;

public class DocumentItemRelationships
{
    public int Id { get; set; }
    public int ParentDocumentId { get; set; }
    public required Document ParentDocument { get; set; }
    public int SubordinateDocumentId { get; set; }
    public required Document SubordinateDocument { get; set; }
    public int ParentPositionId { get; set; }
    public required DocumentPositions ParentPosition { get; set; }
    public int SubordinatePositionId { get; set; }
    public required DocumentPositions SubordinatePosition { get; set; }
    public required int NetQuantityParent { get; set; }
    public required int QuantityLossParent { get; set; }
}
