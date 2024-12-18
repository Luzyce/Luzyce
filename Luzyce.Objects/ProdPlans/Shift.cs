using Luzyce.Objects.Users;

namespace Luzyce.Objects.ProdPlans;

public class Shift
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int ShiftNumber { get; set; }
    public User? ShiftSupervisor { get; set; }
}
