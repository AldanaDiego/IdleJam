using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeScriptableObject", menuName = "ScriptableObjects/Biome")]
public class Biome : ScriptableObject
{
    public enum BiomeProperty { HUMIDITY = 0, TEMPERATURE = 1, VEGETATION = 2, STATIC = 3, POLLUTION = 4 };

    public string Name;
    public string Details;
    public Vector2 Coordinates;
    public List<BiomePropertyApplies> Properties;
    public List<ExplorationEventChance> ExplorationEventChances;
    public List<ResourceChance> ResourceChances;
}
