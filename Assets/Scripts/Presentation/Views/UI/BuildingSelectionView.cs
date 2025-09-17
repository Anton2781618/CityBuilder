using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Domain;

namespace Presentation.Views.UI
{
    /// <summary>
    /// View для панели выбора зданий.
    /// </summary>
    public class BuildingSelectionView : MonoBehaviour
    {
        private VisualElement _root;
        private Button _houseBtn;
        private Button _farmBtn;
        private Button _mineBtn;

        private void Awake()
        {
            var uiDocument = GetComponent<UIDocument>();
            _root = uiDocument.rootVisualElement;
            _houseBtn = _root.Q<Button>("HouseBtn");
            _farmBtn = _root.Q<Button>("FarmBtn");
            _mineBtn = _root.Q<Button>("MineBtn");

            _houseBtn.clicked += () => SelectBuilding(BuildingType.House);
            _farmBtn.clicked += () => SelectBuilding(BuildingType.Farm);
            _mineBtn.clicked += () => SelectBuilding(BuildingType.Mine);
        }

        private void SelectBuilding(BuildingType type)
        {
            Debug.Log($"Выбран тип здания: {type}");
            // TODO: отправить событие выбора типа здания
        }
    }
}
