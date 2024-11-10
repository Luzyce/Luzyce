namespace Luzyce.Api.Db.AppDb.Models;

public class KwitLack
{
    public int Id { get; set; }
    public int LackId { get; set; }
    public Error? Lack { get; set; }
    public int KwitId { get; set; }
    public Document? Kwit { get; set; }
    public int Quantity { get; set; }
}
