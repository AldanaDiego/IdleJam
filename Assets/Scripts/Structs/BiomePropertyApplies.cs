using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BiomePropertyApplies
{
    public Biome.BiomeProperty Property;
    public bool Applies;

    public BiomePropertyApplies(Biome.BiomeProperty property, bool applies)
    {
        Property = property;
        Applies = applies;
    }
}
