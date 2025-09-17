using UnityEngine;
using System.Collections.Generic;
using Domain;
using VContainer;

namespace Presentation.Views
{
    /// <summary>
    /// View для визуализации клеточной сетки и подсветки.
    /// </summary>
    public class GridView : MonoBehaviour
    {
        [Inject] [SerializeField] private GridCellView cellPrefab;
        [SerializeField] private int gridWidth = 32;
        [SerializeField] private int gridHeight = 32;

        private readonly List<GridCellView> _cells = new();
        private readonly Dictionary<Collider, GridCellView> _colliderToCellView = new();

        public void Initialize(GridModel grid)
        {
            gridWidth = grid.Width;            
            gridHeight = grid.Height;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridHeight; z++)
                {
                    var myPos = transform.position;

                    var newCell = Instantiate(cellPrefab, new Vector3(myPos.x + x, myPos.y, myPos.z + z), Quaternion.identity, transform);

                    newCell.name = $"Cell_{x}_{z}";

                    newCell.Setup(new GridPosition(x, z));

                    _cells.Add(newCell);

                    var collider = newCell.GetComponent<Collider>();
                    if (collider != null)
                    {
                        _colliderToCellView[collider] = newCell;
                    }
                }
            }
        }
        public GridCellView GetCellViewByCollider(Collider collider)
        {
            return _colliderToCellView.TryGetValue(collider, out var view) ? view : null;
        }

        public GridCellView GetCellViewByGridPosition(GridPosition position)
        {
            return _cells.Find(c => c.GridPosition.X == position.X && c.GridPosition.Y == position.Y);
        }

        public void RenderGrid(GridModel grid)
        {
            foreach (var cell in grid.GetAllCells())
            {
                var color = cell.State == CellState.Occupied ? Color.red : Color.gray;
                // HighlightCell(cell.Position, color);
                // Можно добавить отображение BuildingView для занятых клеток
            }
        }

        public void HighlightCell(Collider col, Color color)
        {
            _colliderToCellView[col].HighlighteCell(true, color);
        }

        public void ResetHighlight(Collider col)
        {
            _colliderToCellView[col].HighlighteCell(false);
        }
    }
}
