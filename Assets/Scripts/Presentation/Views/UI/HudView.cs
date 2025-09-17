using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Domain;

namespace Presentation.Views.UI
{
    /// <summary>
    /// View для HUD: отображение ресурсов и уведомлений.
    /// </summary>
    public class HudView : MonoBehaviour
    {
        [Inject] private PlayerEconomy _economy;
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
