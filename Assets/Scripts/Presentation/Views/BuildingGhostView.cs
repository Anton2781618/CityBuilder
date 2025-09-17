using UnityEngine;
using Domain;

namespace Presentation.Views
{
    /// <summary>
    /// View-призрак здания для предпросмотра установки.
    /// </summary>
    public class BuildingGhostView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;

        public void ShowAt(GridPosition pos, bool canPlace)
        {
            transform.localPosition = new Vector3(pos.X, 0, pos.Y);

            meshRenderer.material.color = canPlace ? Color.green : Color.red;

            meshRenderer.enabled = true;
        }

        public void Hide()
        {
            meshRenderer.enabled = false;
        }

        public void ResetColor()
        {
            meshRenderer.material.color = Color.white;
        }
    }
}
