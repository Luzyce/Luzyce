namespace Luzyce.Api.Domain.Models;

public class OrderForProduction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Number { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerSymbol { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
}