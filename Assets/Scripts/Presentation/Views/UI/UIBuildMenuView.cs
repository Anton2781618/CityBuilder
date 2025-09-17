using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using VContainer;

public class UIBuildMenuView : MonoBehaviour
{
    [Inject] private List<BuildingConfig> _buildingConfigs;
    private VisualElement _root;
    private VisualElement _menu;
    public event Action<BuildingConfig> OnBuildingSelected;

    void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _menu = _root.Q<VisualElement>("Menu");

        CreateBuildingButtons(_buildingConfigs);
    }

    /// <summary>
    /// Генерирует кнопки зданий по списку BuildingConfig
    /// </summary>
    public void CreateBuildingButtons(List<BuildingConfig> configs)
    {
        foreach (var config in configs)
        {
            var btn = new Button { text = config.Type.ToString() };

            btn.clicked += () => OnBuildingButtonClicked(config);

            _menu.Add(btn);
        }
    }

    private void OnBuildingButtonClicked(BuildingConfig config)
    {
        OnBuildingSelected?.Invoke(config);
    }
}
