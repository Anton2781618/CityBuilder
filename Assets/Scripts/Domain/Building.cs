using System;

namespace Domain
{
    /// <summary>
    /// Тип здания.
    /// </summary>
    public enum BuildingType
    {
        None,
        House,
        Farm,
        Mine
    }

    /// <summary>
    /// Уровень здания.
    /// </summary>
    public enum BuildingLevel
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3
    }

    /// <summary>
    /// Стоимость постройки или апгрейда.
    /// </summary>
    public struct BuildingCost
    {
        public int Gold;
    }

    /// <summary>
    /// Доход здания.
    /// </summary>
    public struct BuildingIncome
    {
        public int GoldPerTick;
    }

    /// <summary>
    /// Позиция на сетке.
    /// </summary>
    public struct GridPosition : IEquatable<GridPosition>
    {
        public int X;
        public int Y;

        public GridPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(GridPosition other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is GridPosition other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(X, Y);
    }

    /// <summary>
    /// Модель здания (Domain).
    /// </summary>
    public class Building
    {
        public BuildingType Type { get; }
        public BuildingLevel Level { get; private set; }
        public GridPosition Position { get; private set; }
        public BuildingCost Cost { get; }
        public BuildingIncome Income { get; }

        public Building(BuildingType type, BuildingLevel level, GridPosition position, BuildingCost cost, BuildingIncome income)
        {
            Type = type;
            Level = level;
            Position = position;
            Cost = cost;
            Income = income;
        }

        public void Upgrade(BuildingLevel newLevel, BuildingCost upgradeCost, BuildingIncome newIncome)
        {
            Level = newLevel;
            // Можно добавить логику проверки стоимости и т.д.
        }

        public void Move(GridPosition newPosition)
        {
            Position = newPosition;
        }
    }
}
