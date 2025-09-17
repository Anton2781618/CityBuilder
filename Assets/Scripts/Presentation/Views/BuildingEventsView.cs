using UnityEngine;
using VContainer;
using MessagePipe;
using Application;
using Domain;

namespace Presentation.Views
{
    /// <summary>
    /// View для отображения событий зданий.
    /// Подписывается на события MessagePipe и обновляет UI.
    /// </summary>
    public class BuildingEventsView : MonoBehaviour
    {
        [Inject] private ISubscriber<BuildingPlacedEvent> _placedSubscriber;
        [Inject] private ISubscriber<BuildingMovedEvent> _movedSubscriber;
        [Inject] private ISubscriber<BuildingRemovedEvent> _removedSubscriber;
        [Inject] private ISubscriber<BuildingUpgradedEvent> _upgradedSubscriber;

        private void Start()
        {
            _placedSubscriber.Subscribe(OnBuildingPlaced);
            _movedSubscriber.Subscribe(OnBuildingMoved);
            _removedSubscriber.Subscribe(OnBuildingRemoved);
            _upgradedSubscriber.Subscribe(OnBuildingUpgraded);
        }

        private void OnBuildingPlaced(BuildingPlacedEvent evt)
        {
            // TODO: обновить UI, отобразить новое здание
            Debug.Log($"[BuildingEventsView] Здание построено: {evt.Building.Type} на {evt.Building.Position.X},{evt.Building.Position.Y}");
        }

        private void OnBuildingMoved(BuildingMovedEvent evt)
        {
            // TODO: обновить UI, переместить здание
            Debug.Log($"[BuildingEventsView] Здание перемещено: {evt.Building.Type} из {evt.From.X},{evt.From.Y} в {evt.To.X},{evt.To.Y}");
        }

        private void OnBuildingRemoved(BuildingRemovedEvent evt)
        {
            // TODO: обновить UI, удалить здание
            Debug.Log($"[BuildingEventsView] Здание удалено с {evt.Position.X},{evt.Position.Y}");
        }

        private void OnBuildingUpgraded(BuildingUpgradedEvent evt)
        {
            // TODO: обновить UI, показать апгрейд
            Debug.Log($"[BuildingEventsView] Здание улучшено: {evt.Building.Type} уровень {evt.Building.Level}");
        }
    }
}
