using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeMutationManager : Singleton<BiomeMutationManager>
{
    [SerializeField] private BiomeDB _biomeDB;
    private Dictionary<int, Biome> _biomes;

    private void Start()
    {
        _biomes = new Dictionary<int, Biome>();
        foreach (Biome biome in _biomeDB.Biomes)
        {
            int id = CalculateID(biome);
            _biomes[id] = biome;
            Debug.Log($"Biome {id}: {biome}");
        }
    }

    public Biome GetBiomeMutation(Biome biome, List<BiomePropertyApplies> propApplies)
    {
        return biome;
    }

    private int CalculateID(Biome biome)
    {
        int i = 0;
        foreach (BiomePropertyApplies propApplies in biome.Properties)
        {
            if (propApplies.Applies)
            {
                i += (int) Mathf.Pow(2, (int) propApplies.Property); //If you read this, dont be like me xd
            }
        }
        return i;
    }

}
