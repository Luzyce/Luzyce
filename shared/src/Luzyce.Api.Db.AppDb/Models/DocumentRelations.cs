namespace Luzyce.Api.Db.AppDb.Models;

public class DocumentRelations
{
    public int Id { get; set; }
    public int ParentDocumentId { get; set; }
    public required Document ParentDocument { get; set; }
    public int SubordinateDocumentId { get; set; }
    public required Document SubordinateDocument { get; set; }

}
