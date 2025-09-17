using UnityEngine;
using System.Collections.Generic;
using Domain;

[CreateAssetMenu(menuName = "CityBuilder/BuildingConfig")]
public class BuildingConfig : ScriptableObject
{
    public BuildingType Type;
    [SerializeField] private List<LevelPrefab> _levelPrefabs;

    [System.Serializable]
    public class LevelPrefab
    {
        public BuildingLevel Level;
        public GameObject Prefab;
        public int Price;
    }

    public GameObject GetPrefab(BuildingLevel level)
    {
        var entry = _levelPrefabs.Find(x => x.Level == level);

        return entry != null ? entry.Prefab : null;
    }
}
