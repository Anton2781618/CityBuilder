using System.Collections.Generic;
using Domain;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    private BuildingGidCell[,] _grid;

    private void Start()
    {
        _grid = new BuildingGidCell[_width, _height];

        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                _grid[x, y] = new BuildingGidCell();
            }
        }

    }

    public void SetBuilding(Building building, List<Vector3> allBuildingPositions)
    {
        foreach (var position in allBuildingPositions)
        {
            var (x, y) = WorldToGridPosition(position);

            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                _grid[x, y].SetBuilding(building);
            }
        }
    }

    //проверяет можно ли построить здание в указанных позициях
    public bool CanBuild(List<Vector3> allBuildingPositions)
    {
        foreach (var position in allBuildingPositions)
        {
            var (x, y) = WorldToGridPosition(position);

            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                return false; // позация вне границ сетки
            }

            if (!_grid[x, y].IsEmpty())
            {
                return false; // ячейка занята
            }
        }

        return true; // все позиции свободны
    }

    // преобразует мировые координаты в координаты сетки
    private (int X, int Y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / BuildingSystem.CellSize);
        int y = Mathf.FloorToInt(worldPosition.z / BuildingSystem.CellSize);
        return (x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        Vector3 origin = transform.position;

        for (int y = 0; y <= _height; y++)
        {
            Vector3 start = origin + new Vector3(0, 0.01f, y * BuildingSystem.CellSize);
            Vector3 end = origin + new Vector3(_width * BuildingSystem.CellSize, 0.01f, y * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }

        for (int x = 0; x <= _width; x++)
        {
            Vector3 start = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, 0);
            Vector3 end = origin + new Vector3(x * BuildingSystem.CellSize, 0.01f, _height * BuildingSystem.CellSize);
            Gizmos.DrawLine(start, end);
        }

    }

}
    public class BuildingGidCell
    {
        private Building _building;

        public void SetBuilding(Building building)
        {
            _building = building;
        }


        public bool IsEmpty() => _building == null;
    }