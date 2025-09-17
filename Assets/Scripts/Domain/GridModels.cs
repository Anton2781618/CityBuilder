using System;
using System.Collections.Generic;
using UnityEngine;

namespace Domain
{
    /// <summary>
    /// Состояние клетки сетки.
    /// </summary>
    public enum CellState
    {
        Empty,
        Occupied
    }

    /// <summary>
    /// Клетка сетки.
    /// </summary>
    public class GridCell
    {
        public GridPosition Position { get; }
        public CellState State { get; private set; }
        public Building OccupiedBuilding { get; private set; }

        public GridCell(GridPosition position)
        {
            Position = position;
            State = CellState.Empty;
        }

        public void PlaceBuilding(Building building)
        {
            State = CellState.Occupied;
            OccupiedBuilding = building;
        }

        public void RemoveBuilding()
        {
            State = CellState.Empty;
            OccupiedBuilding = null;
        }
    }

    /// <summary>
    /// Сетка зданий.
    /// </summary>
    public class GridModel
    {
        public int Width { get; }
        public int Height { get; }
        private readonly GridCell[,] _cells;

        public GridModel(Vector2 size)
        {
            Width = (int)size.x;

            Height = (int)size.y;

            _cells = new GridCell[Width, Height];
            
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _cells[x, y] = new GridCell(new GridPosition(x, y));
                }
            }
        }

        public GridCell GetCell(int x, int y) => _cells[x, y];
        public GridCell GetCell(GridPosition pos) => _cells[pos.X, pos.Y];

        public bool IsCellAvailable(GridPosition pos) => _cells[pos.X, pos.Y].State == CellState.Empty;

        public IEnumerable<GridCell> GetAllCells()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    yield return _cells[x, y];
        }
    }

    /// <summary>
    /// Модель игрока (ресурсы).
    /// </summary>
    public class PlayerEconomy
    {
        public int Gold { get; private set; }

        public PlayerEconomy(int initialGold)
        {
            Gold = initialGold;
        }

        public bool TrySpendGold(int amount)
        {
            if (Gold >= amount)
            {
                Gold -= amount;
                return true;
            }
            return false;
        }

        public void AddGold(int amount)
        {
            Gold += amount;
        }
    }
}
