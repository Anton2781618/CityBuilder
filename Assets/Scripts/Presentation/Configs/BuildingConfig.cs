using UnityEngine;
using System.Collections.Generic;
using Domain;

[CreateAssetMenu(menuName = "CityBuilder/BuildingConfig")]
public class BuildingConfig : ScriptableObject
{
    public BuildingType Type;
    [SerializeField] private List<BuildingLevelData> _levelPrefabs;

    [System.Serializable]
    public class BuildingLevelData
    {
        public BuildingLevel Level;
        public GameObject Prefab;
        public int Price;
        public int GoldPerTick; // Прирост денег за тик
    }

    public GameObject GetPrefab(BuildingLevel level)
    {
        var entry = _levelPrefabs.Find(x => x.Level == level);

        return entry != null ? entry.Prefab : null;
    }
}
