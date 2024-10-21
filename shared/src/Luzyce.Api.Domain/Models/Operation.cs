namespace Luzyce.Api.Domain.Models;

public class Operation
{
    public int Id { get; set; }
    public DateTime Time { get; set; }
    public int DocumentId { get; set; }
    public Document? Document { get; set; }
    public int OperatorId { get; set; }
    public User? Operator { get; set; }
    public int QuantityNetDelta { get; set; }
    public int QuantityLossDelta { get; set; }
    public int QuantityToImproveDelta { get; set; }
    public int? ErrorCodeId { get; set; }
    public Error? ErrorCode { get; set; }
    public bool IsCancelled { get; set; }
    public int ClientId { get; set; }
    public Client? Client { get; set; }
}
