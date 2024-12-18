using Luzyce.Objects.Kwits.List;
using Luzyce.Objects.Lampshades.Detail;
using Luzyce.Objects.Orders;
using Luzyce.Objects.Orders.List;
using Luzyce.Objects.ProdOrders.Detail;
using Luzyce.Objects.ProdPlans.Detail;
using Luzyce.Objects.ProdPlans.List;
using Luzyce.Objects.Users;

namespace Luzyce.Objects.Productions.List;

public class PositionsListItem
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public Client? Client => ProdOrderPosition.Parent.Parent?.Customer;
    public OrdersListItem? Order => ProdOrderPosition.Parent.Parent;
    public LampshadeDetail Lampshade => ProdOrderPosition.Position.Lampshade;
    public User? ShiftSupervisor => ProdPLan.Shift.ShiftSupervisor;
    public User HeadsOfMetallurgicalTeams => ProdPLan.HeadsOfMetallurgicalTeams;
    public int QuantityNetto => Kwit.QuantityNetto;
    public int QuantityLoss => Kwit.QuantityLoss;
    public int QuantityToImprove => Kwit.QuantityToImprove;
    public int QuantityGross => Kwit.QuantityGross;
    

    public ProdOrderPositionDetail ProdOrderPosition => ProdPlanPositionDetail.Position.ProdOrderPosition;
    public ProdPlansListItem ProdPLan => ProdPlanPositionDetail.Parent;
    public ProdPlanPositionDetail ProdPlanPositionDetail => Kwit.ProdPlanPosition;
    public KwitsListItem Kwit { get; set; } = new();
}
