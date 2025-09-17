using Domain;
using UnityEngine;

namespace Presentation.Views
{
    /// <summary>
    /// View для отдельной клетки сетки. Управляет визуализацией (цвет, состояние).
    /// </summary>
    public class GridCellView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _rendererHighlighter;
        public GridPosition GridPosition { get; private set; }
        public GameObject BuildingGo = null;
        public BuildingConfig BuildingConfig { get; private set; }


        public void Setup(GridPosition position)
        {
            GridPosition = position;
        }
        public void SetBuildingConfig(BuildingConfig config)
        {
            BuildingConfig = config;
        }

        public void HighlighteCell(bool enabled, Color color = default)
        {
            _rendererHighlighter.gameObject.SetActive(enabled);
            
            _rendererHighlighter.material.color = color;
        }
    }
}
