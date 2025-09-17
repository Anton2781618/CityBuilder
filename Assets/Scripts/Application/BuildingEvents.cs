using Domain;
using MessagePipe;
using System;

namespace Application
{
    /// <summary>
    /// События для MessagePipe.
    /// </summary>
    public class BuildingPlacedEvent
    {
        public Building Building { get; }
        public BuildingPlacedEvent(Building building) => Building = building;
    }

    public class BuildingMovedEvent
    {
        public Building Building { get; }
        public GridPosition From { get; }
        public GridPosition To { get; }
        public BuildingMovedEvent(Building building, GridPosition from, GridPosition to)
        {
            Building = building;
            From = from;
            To = to;
        }
    }

    public class BuildingRemovedEvent
    {
        public GridPosition Position { get; }
        public BuildingRemovedEvent(GridPosition position) => Position = position;
    }

    public class BuildingUpgradedEvent
    {
        public Building Building { get; }
        public BuildingUpgradedEvent(Building building) => Building = building;
    }

    public class NotEnoughGoldEvent
    {
        public int RequiredGold { get; }
        public NotEnoughGoldEvent(int requiredGold) => RequiredGold = requiredGold;
    }
}
