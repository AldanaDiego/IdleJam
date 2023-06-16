using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentsManager : Singleton<EnvironmentsManager>
{
    [SerializeField] private List<BiomeEnvironment> _biomeEnvironments;

    private Dictionary<Biome, Transform> _environmentsMap;

    private void Start()
    {
        _environmentsMap = new Dictionary<Biome, Transform>();
        foreach (BiomeEnvironment biomeEnvironment in _biomeEnvironments)
        {
            _environmentsMap[biomeEnvironment.Biome] = biomeEnvironment.Environment;
        }
    }

    public Transform GetEnvironment(Biome biome)
    {
        return _environmentsMap[biome];
    }
}
