using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeMutationManager : Singleton<BiomeMutationManager>
{
    [SerializeField] private BiomeDB _biomeDB;
    private Dictionary<int, Biome> _biomes;
    private List<Mutagen> _unlockedMutagens;

    protected override void Awake()
    {
        base.Awake();
        _unlockedMutagens = new List<Mutagen>();
    }

    private void Start()
    {
        _biomes = new Dictionary<int, Biome>();
        #if UNITY_EDITOR
            ValidateBiomes();
        #else
            foreach (Biome biome in _biomeDB.Biomes)
            {
                _biomes[CalculateID(biome)] = biome;
            }
        #endif
    }

    public void SetupFromSave(List<Mutagen> unlockedMutagens)
    {
        _unlockedMutagens = unlockedMutagens;
    }

    public Biome GetBiomeMutation(Biome biome, List<BiomePropertyApplies> propertiesChanged)
    {
        int id = CalculateID(biome);
        foreach (BiomePropertyApplies change in propertiesChanged)
        {
            int amount = (int)Mathf.Pow(2, (int)change.Property);
            if (change.Applies)
            {
                id += amount;
            }
            else
            {
                id -= amount;
            }
        }
        return _biomes[id];
    }

    public Biome GetBiomeMutation(Biome biome, List<Mutagen> mutagens)
    {
        int id = CalculateID(biome);
        foreach (Mutagen mutagen in mutagens)
        {
            bool hasMutagenEffect = true;
            foreach (BiomePropertyApplies property in biome.Properties)
            {
                if (property.Property == mutagen.BiomeProperty)
                {
                    hasMutagenEffect = property.Applies != mutagen.AppliesProperty;
                    break;
                }
            }

            if (!hasMutagenEffect)
            {
                continue;
            }

            int amount = (int)Mathf.Pow(2, (int)mutagen.BiomeProperty);   
            if (mutagen.AppliesProperty)
            {
                id += amount;
            }
            else
            {
                id -= amount;
            }
        }
        return _biomes[id];
    }

    public List<Mutagen> GetUnlockedMutagens()
    {
        return _unlockedMutagens;
    }

    public void UnlockMutagen(Mutagen mutagen)
    {
        _unlockedMutagens.Add(mutagen);
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

    private void ValidateBiomes()
    {
        Debug.Log($"Validating biomes");
        foreach (Biome biome in _biomeDB.Biomes)
        {
            int id = CalculateID(biome);
            if (_biomes.ContainsKey(id))
            {
                Debug.Log($"Duplicate biome ID: {_biomes[id]} {biome}");
            }
            
            List<ResourceChance> resourceChances = biome.ResourceChances.FindAll(chance => chance.Resource.IsBasicResource);
            if (resourceChances.Count != 3)
            {
                Debug.Log($"{biome} doesn't have all basic resources");
            }

            float chanceSum = 0f;
            foreach (ResourceChance chance in resourceChances)
            {
                chanceSum += chance.Chance;
            }

            if (chanceSum != 1f)
            {
                Debug.Log($"{biome} basic resources don't sum 1");
            }

            resourceChances = biome.ResourceChances.FindAll(chance => !chance.Resource.IsBasicResource);
            if (resourceChances.Count > 0)
            {
                chanceSum = 0f;
                foreach (ResourceChance chance in resourceChances)
                {
                    chanceSum += chance.Chance;
                }

                if (chanceSum != 1f)
                {
                    Debug.Log($"{biome} rare resources don't sum 1");
                }
            }

            Vector2 coordinates = new Vector2(
                ((id % 8) + 1) * 50,
                (Mathf.RoundToInt(id/8)) * 50
            );
            if (biome.Coordinates != coordinates)
            {
                Debug.Log($"{biome} has wrong coordinates");
            }

            _biomes[id] = biome;
        }
    }

}
