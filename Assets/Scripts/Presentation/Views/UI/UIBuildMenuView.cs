using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

public class UIBuildMenuView : MonoBehaviour
{
    private VisualElement _ui;
    private VisualElement _menu;

    private List<Button> _buildingButtons = new();
    
    void Awake()
    {
        _ui = GetComponent<UIDocument>().rootVisualElement;

        _menu = _ui.Q<VisualElement>("Menu");

        SetBuildingNames(new List<string> { "House", "Farm", "Mine"});
    }

    /// <summary>
    /// Генерирует кнопки зданий по списку названий
    /// </summary>
    public void SetBuildingNames(List<string> buildingNames)
    {
        // Очищаем старые кнопки
        foreach (var btn in _buildingButtons)
        {
            _menu.Remove(btn);
        }

        _buildingButtons.Clear();

        foreach (var name in buildingNames)
        {
            var btn = new Button { text = name };

            btn.clicked += () => OnBuildingButtonClicked(name);

            _menu.Add(btn);
        
            _buildingButtons.Add(btn);
        }
    }

    public event Action<string> OnBuildingSelected;

    private void OnBuildingButtonClicked(string buildingName)
    {
        Debug.Log($"Выбрано здание: {buildingName}");
        OnBuildingSelected?.Invoke(buildingName);
    }
}
