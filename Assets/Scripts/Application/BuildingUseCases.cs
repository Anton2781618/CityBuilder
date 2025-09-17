using Domain;
using MessagePipe;

namespace Application
{
    /// <summary>
    /// UseCase: установка здания на сетку.
    /// </summary>
    public class PlaceBuildingUseCase
    {
        private readonly GridModel _grid;
        private readonly Economy _economy;
        private readonly IPublisher<BuildingPlacedEvent> _publisher;

        public PlaceBuildingUseCase(GridModel grid, Economy economy, IPublisher<BuildingPlacedEvent> publisher)
        {
            _grid = grid;
            _economy = economy;
            _publisher = publisher;
        }

        public bool Execute(PlaceBuildingRequest request, BuildingCost cost, BuildingIncome income)
        {
            //место занято или вне границ
            if (!_grid.IsCellAvailable(request.Position)) return false;  
            
            // недостаточно золота для строительства
            if (!_economy.TrySpendGold(cost.Gold)) return false;

            var building = new Building(request.Type, request.Level, request.Position, cost, income);

            _grid.GetCell(request.Position).PlaceBuilding(building);

            _publisher.Publish(new BuildingPlacedEvent(building));
            
            return true;
        }
    }

    /// <summary>
    /// UseCase: удаление здания.
    /// </summary>
    public class RemoveBuildingUseCase
    {
        private readonly GridModel _grid;
        private readonly IPublisher<BuildingRemovedEvent> _publisher;

        public RemoveBuildingUseCase(GridModel grid, IPublisher<BuildingRemovedEvent> publisher)
        {
            _grid = grid;
            _publisher = publisher;
        }

        public bool Execute(RemoveBuildingRequest request)
        {
            var cell = _grid.GetCell(request.Position);

            if (cell.State != CellState.Occupied) return false;

            cell.RemoveBuilding();
            
            _publisher.Publish(new BuildingRemovedEvent(request.Position));
            
            return true;
        }
    }

    /// <summary>
    /// UseCase: апгрейд здания.
    /// </summary>
    public class UpgradeBuildingUseCase
    {
        private readonly GridModel _grid;
        private readonly Economy _economy;
        private readonly IPublisher<BuildingUpgradedEvent> _publisher;

        public UpgradeBuildingUseCase(GridModel grid, Economy economy, IPublisher<BuildingUpgradedEvent> publisher)
        {
            _grid = grid;
            _economy = economy;
            _publisher = publisher;
        }

        public bool Execute(UpgradeBuildingRequest request, BuildingCost upgradeCost, BuildingIncome newIncome)
        {
            var cell = _grid.GetCell(request.Position);

            if (cell.State != CellState.Occupied) return false;

            if (!_economy.TrySpendGold(upgradeCost.Gold)) return false;

            cell.OccupiedBuilding.Upgrade(request.NewLevel, upgradeCost, newIncome);

            _publisher.Publish(new BuildingUpgradedEvent(cell.OccupiedBuilding));
            
            return true;
        }
    }
}
