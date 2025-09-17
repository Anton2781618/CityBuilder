using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Domain;

namespace Presentation.Views.UI
{
    /// <summary>
    /// View для HUD: отображение ресурсов 
    /// </summary>
    public class EconomyView : MonoBehaviour
    {
        [Inject] private Economy _economy;
        private Label _goldLabel;
        private VisualElement _root;

        private void Awake()
        {
            var uiDocument = GetComponent<UIDocument>();
            _root = uiDocument.rootVisualElement;
            _goldLabel = _root.Q<Label>("GoldLabel");
        }

        private void Update()
        {
            if (_goldLabel != null)
                _goldLabel.text = $"Gold: {_economy.Gold}";
        }
    }
}
