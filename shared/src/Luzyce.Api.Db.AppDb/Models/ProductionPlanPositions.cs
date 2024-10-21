namespace Luzyce.Api.Db.AppDb.Models;

public class ProductionPlanPositions
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int ProductionPlanId { get; set; }
    public ProductionPlan? ProductionPlan { get; set; }
    public int DocumentPositionId { get; set; }
    public DocumentPositions? DocumentPosition { get; set; }
    public int? NumberOfHours { get; set; }
    
    public List<Document> Kwit { get; set; } = [];
}