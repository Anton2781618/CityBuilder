using UnityEngine;
using VContainer;
using Application;
using Domain;
using Presentation.Views;
using MessagePipe;

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
        [Inject] private ISubscriber<CellHoveredEvent> _cellHoveredSubscriber;

        //-------------------------------------------------------------------
        // Для отслеживания последней подсвеченной клетки
        private Collider _lastHoveredCell = null;
        //-------------------------------------------------------------------
        [Inject] private PlaceBuildingUseCase _placeBuildingUseCase;
        [Inject] private UIBuildMenuView _buildMenuView;
        [SerializeField] private BuildingGhostView _ghostPrefab;
        private BuildingGhostView _ghostView;

        //-------------------------------------------------------------------
        private BuildingType _selectedType = BuildingType.House;
        private BuildingLevel _selectedLevel = BuildingLevel.Level1;
        private BuildingCost _selectedCost = new BuildingCost { Gold = 100 };
        private BuildingIncome _selectedIncome = new BuildingIncome { GoldPerTick = 1 };

        //!-------------------------------------------------------------------

        private void Awake()
        {
            _gridView.Initialize(_gridModel);
            _cellHoveredSubscriber.Subscribe(OnCellHoveredEvent);
            _buildMenuView.OnBuildingSelected += OnBuildingSelected;
        }
        private void OnBuildingSelected(string buildingName)
        {
            // Преобразовать строку в BuildingType (можно через Enum.Parse или свой маппинг)
            if (System.Enum.TryParse(buildingName, out BuildingType type))
            {
                _selectedType = type;

                // Создать призрак здания из префаба
                if (_ghostView == null)
                {
                    _ghostView = Instantiate(_ghostPrefab, _gridView.transform);

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
            
                _ghostView?.ShowAt(pos, canPlace);
            
                _gridView.HighlightCell(cell, canPlace ? Color.green : Color.red);
            }
            
            _lastHoveredCell = cell;
        }

        public void OnCellClick(GridPosition pos)
        {
            // Проверить, выбран ли тип здания и есть ли призрак
            if (_ghostView == null) return;

            bool placed = _placeBuildingUseCase.Execute(new PlaceBuildingRequest {
                Type = _selectedType,
                Level = _selectedLevel,
                Position = pos
            }, _selectedCost, _selectedIncome);

            if (placed)
            {
                _ghostView.Hide();
                _gridView.HighlightCell(pos, Color.gray);
            }
        }

        public void OnBuildingTypeSelected(BuildingType type)
        {
            _selectedType = type;
        }
    }
}