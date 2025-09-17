using UnityEngine;
using VContainer;
using Application;
using Domain;
using Presentation.Views;
using MessagePipe;
using UnityEditor.PackageManager.Requests;

namespace Presentation.Presenters
{
    /// <summary>
    /// Presenter для установки зданий: связывает UI, Input, GridView и UseCase.
    /// </summary>
    public class PlaceBuildingPresenter : MonoBehaviour
    {
        //-------------------------------------------------------------------
        [Inject] private GridView _gridView;
        [Inject] private GridModel _gridModel;
        //-------------------------------------------------------------------
        [Inject] private UIBuildingInfoPanel _buildingInfoPanel;

        //-------------------------------------------------------------------
        // 
        [Inject] private ISubscriber<CellHoveredEvent> _cellHoveredSubscriber;
        [Inject] private ISubscriber<CellClickedEvent> _cellClickedSubscriber;

        //-------------------------------------------------------------------
        // Для отслеживания последней подсвеченной клетки
        private Collider _lastHoveredCell = null;
        //-------------------------------------------------------------------
        // UseCase
        [Inject] private PlaceBuildingUseCase _placeBuildingUseCase;
        [Inject] private RemoveBuildingUseCase _removeBuildingUseCase;
        [Inject] private MoveBuildingUseCase _moveBuildingUseCase;
        //-------------------------------------------------------------------
        [Inject] private UIBuildMenuView _buildMenuView;
        [Inject] private UIBuildingInfoPanel _infoPanel;
        private BuildingGhostView _ghostView;

        private bool _isPlacementMode = false;
        //-------------------------------------------------------------------
        private BuildingLevel _selectedLevel = BuildingLevel.Level1;
        private BuildingCost _selectedCost = new BuildingCost { Gold = 100 };
        private BuildingIncome _selectedIncome = new BuildingIncome { GoldPerTick = 1 };

        //-------------------------------------------------------------------
        private BuildingConfig _selectedConfig;

        //!-------------------------------------------------------------------

        private void Awake()
        {
            _gridView.Initialize(_gridModel);
            _cellHoveredSubscriber.Subscribe(OnCellHoveredEvent);
            _cellClickedSubscriber.Subscribe(evt => OnCellClick());
            _buildMenuView.OnBuildingSelected += OnCreateBuilding;
            _infoPanel.OnDeleteBuilding += OnDeleteBuilding;
            _infoPanel.OnMoveBuilding += OnMoveBuilding;
        }
        private void OnDeleteBuilding(Domain.Building building)
        {
            var pos = building.Position;

            var request = new RemoveBuildingRequest { Position = pos };

            bool removed = _removeBuildingUseCase.Execute(request);

            if (removed)
            {
                var cellView = _gridView.GetCellViewByGridPosition(pos);
                if (cellView != null && cellView.BuildingGo != null)
                {
                    Destroy(cellView.BuildingGo);
                    cellView.BuildingGo = null;
                }
            }
        }

        private void OnCreateBuilding(BuildingConfig config)
        {
            _selectedConfig = config;
            _lastHoveredCell = null;
            _isPlacementMode = true;

            var ghostPrefab = config.GetPrefab(BuildingLevel.Level1); 
            
            if (ghostPrefab != null)
            {
                if (_ghostView == null)
                {
                    _ghostView = Instantiate(ghostPrefab, _gridView.transform).GetComponent<BuildingGhostView>();
                    _ghostView.Hide();
                }
            }
        }

        private void OnCellHoveredEvent(CellHoveredEvent evt)
        {
            OnCellHover(evt.HoveredCell);
        }


        public void OnCellHover(Collider cell)
        {
            // Сбросить подсветку предыдущей клетки
            if (_lastHoveredCell && _lastHoveredCell != cell)
            {
                _gridView.ResetHighlight(_lastHoveredCell);
            }

            if (cell == null)
            {
                _ghostView?.Hide();
                
                return;
            }

            var cellView = _gridView.GetCellViewByCollider(cell);

            if (cellView != null)
            {
                var pos = cellView.GridPosition;

                bool canPlace = _gridModel.IsCellAvailable(pos);
            
                if(_isPlacementMode) _ghostView?.ShowAt(pos, canPlace);
            
                _gridView.HighlightCell(cell, canPlace ? Color.green : Color.red);
            }
            
            _lastHoveredCell = cell;
        }

        public void OnCellClick()
        {
            
            if (_isPlacementMode)
            {
                PlaceBuilding();
            }
            else
            {
                OpenInfoPanel();
            }
        }

        private void PlaceBuilding()
        {
            // Проверить, выбран ли тип здания и есть ли призрак
            if (_ghostView == null || _lastHoveredCell == null || !_isPlacementMode) return;

            var cellView = _gridView.GetCellViewByCollider(_lastHoveredCell);

            bool placed = _placeBuildingUseCase.Execute(new PlaceBuildingRequest
            {
                Type = _selectedConfig.Type,
                Level = _selectedLevel,
                Position = cellView.GridPosition
            }, _selectedCost, _selectedIncome);

            if (placed)
            {
                cellView.SetBuildingConfig(_selectedConfig);

                _isPlacementMode = false;

                _gridView.HighlightCell(_lastHoveredCell, Color.gray);

                Vector3 position = cellView.transform.position;

                _ghostView.ResetColor();

                _ghostView.transform.SetParent(_lastHoveredCell.transform);

                _ghostView.transform.position = position;

                cellView.BuildingGo = _ghostView.gameObject;

                _ghostView = null;
            }
        }

        private void OpenInfoPanel()
        {
            if (_lastHoveredCell == null) return;

            var cellView = _gridView.GetCellViewByCollider(_lastHoveredCell);

            var cellModel = _gridModel.GetCell(cellView.GridPosition);

            if (cellModel.State == CellState.Occupied)
            {
                Building building = cellModel.OccupiedBuilding;

                _buildingInfoPanel.Show(building);
            }

        }

        private void OnMoveBuilding(Domain.Building building)
        {
            OnDeleteBuilding(building);

            OnCreateBuilding(_gridView.GetCellViewByGridPosition(building.Position).BuildingConfig);
        }
    }
}