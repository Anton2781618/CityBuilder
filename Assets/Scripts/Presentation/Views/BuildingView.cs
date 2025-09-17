using UnityEngine;
using VContainer;
using Domain;

namespace Presentation.Views
{
    /// <summary>
    /// View для 3D здания на сцене.
    /// Управляет визуализацией и действиями над объектом.
    /// </summary>
    public class BuildingView : MonoBehaviour
    {
        public Building BuildingModel { get; private set; }

        public void Initialize(Building model)
        {
            BuildingModel = model;
            // TODO: визуализировать здание (установить позицию, материал, иконку)
            transform.position = new Vector3(model.Position.X, 0, model.Position.Y);
        }

        public void MoveTo(GridPosition newPos)
        {
            // Анимация или мгновенное перемещение
            transform.position = new Vector3(newPos.X, 0, newPos.Y);
        }

        public void Upgrade(BuildingLevel newLevel)
        {
            // TODO: визуально показать апгрейд (смена материала, эффекта)
            Debug.Log($"Здание {BuildingModel.Type} улучшено до {newLevel}");
        }

        public void Remove()
        {
            // TODO: анимация удаления
            Destroy(gameObject);
        }
    }
}
