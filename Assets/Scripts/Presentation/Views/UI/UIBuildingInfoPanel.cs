using System;
using Domain;
using UnityEngine;
using UnityEngine.UIElements;

public class UIBuildingInfoPanel : MonoBehaviour
{
    private Building _currentBuilding;
    public event Action<Building> OnDeleteBuilding;
    private VisualElement _root;
    private VisualElement _menu;
    private Label _labelBuildingName;

    void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _menu = _root.Q<VisualElement>("Menu");
        _labelBuildingName = _root.Q<Label>("BuildingName");

        HidePanel();

        CreateButtonsBuildingOptions();
    }

    public void Show(Building building)
    {
        _root.style.display = DisplayStyle.Flex;
        _currentBuilding = building;
        _labelBuildingName.text = building.Type.ToString();
    }

    public void HidePanel() => _root.style.display = DisplayStyle.None;

    /// <summary>
    /// Создаем кнопки улучшений, удаления, перемещения здания
    /// </summary>
    public event Action<Building> OnMoveBuilding;

    public void CreateButtonsBuildingOptions()
    {
        var upgradeButton = new Button { text = "Улучшить" };
        var moveButton = new Button { text = "Передвинуть" };
        var sellButton = new Button { text = "Удалить" };

        _menu.Add(upgradeButton);
        _menu.Add(moveButton);
        _menu.Add(sellButton);

        upgradeButton.clicked += () => OnUpgradeClicked();
        moveButton.clicked += () => OnMoveClicked();
        sellButton.clicked += () => OnDeleteClicked();
    }

    private void OnDeleteClicked()
    {
        Debug.Log("Удаление здания: " + _currentBuilding.Type);
        if (_currentBuilding != null)
        {
            OnDeleteBuilding?.Invoke(_currentBuilding);
        }

        HidePanel();
    }

    private void OnMoveClicked()
    {
        if (_currentBuilding != null)
        {
            OnMoveBuilding?.Invoke(_currentBuilding);
        }
        HidePanel();
    }

    private void OnUpgradeClicked()
    {
     
    }
}
