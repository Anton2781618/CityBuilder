using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingModel : MonoBehaviour
{
    [SerializeField] private Transform wrappwer;

    public float Rottion => wrappwer.rotation.eulerAngles.y;

    private BuildingShapeUnit[] _shapeUnits;


    private void Awake()
    {
        _shapeUnits = GetComponentsInChildren<BuildingShapeUnit>();
    }

    public void Rotate(float angle)
    {
        wrappwer.rotation = Quaternion.Euler(0, angle, 0);
    }

    public List<Vector3> GetAllBuildingPositions()
    {
        return _shapeUnits.Select(u => u.transform.position).ToList();
        
    }
}
