using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeScriptableObject", menuName = "ScriptableObjects/Biome")]
public class Biome : ScriptableObject
{
    public string Name;
    public string Details;
    public List<ExplorationEventChance> ExplorationEventChances;
    public List<ResourceChance> ResourceChances;
}
