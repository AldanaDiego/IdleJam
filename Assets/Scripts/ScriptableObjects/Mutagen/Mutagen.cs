using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MutagenScriptableObject", menuName = "ScriptableObjects/Mutagen")]
public class Mutagen : ScriptableObject
{
    public string Name;
    public Biome.BiomeProperty BiomeProperty;
    public bool AppliesProperty;
    public Color Color;
    public Sprite Image;
}
