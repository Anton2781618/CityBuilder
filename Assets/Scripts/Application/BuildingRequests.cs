using Domain;
using System;
using UnityEngine;

namespace Application
{
    /// <summary>
    /// DTO для запроса установки здания.
    /// </summary>
    public class PlaceBuildingRequest
    {
        public BuildingType Type { get; set; }
        public BuildingLevel Level { get; set; }
        public GridPosition Position { get; set; }
    }

    /// <summary>
    /// DTO для запроса перемещения здания.
    /// </summary>
    public class MoveBuildingRequest
    {
        public GridPosition From { get; set; }
        public GridPosition To { get; set; }
    }

    /// <summary>
    /// DTO для запроса удаления здания.
    /// </summary>
    public class RemoveBuildingRequest
    {
        public GridPosition Position { get; set; }
    }

    /// <summary>
    /// DTO для запроса апгрейда здания.
    /// </summary>
    public class UpgradeBuildingRequest
    {
        public GridPosition Position { get; set; }
        public BuildingLevel NewLevel { get; set; }
    }

    /// <summary>
    /// DTO для события наведения курсора на клетку.
    /// </summary>
    public class CellHoveredEvent
    {
        public Collider HoveredCell { get; set; }
        public CellHoveredEvent(Collider hoveredCell) => HoveredCell = hoveredCell;
    
    }
    /// <summary>
    /// DTO для события клика на клетку.
    /// </summary>
    public class CellClickedEvent
    {
        public Collider ClickedCell { get; set; }
        public CellClickedEvent(Collider clickedCell) => ClickedCell = clickedCell;
    }
}
