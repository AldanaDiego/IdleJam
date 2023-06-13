using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : Singleton<AreaManager>
{
    [SerializeField] private List<Biome> _biomes;

    private List<Area> _areas;

    private void Start()
    {
        _areas = new List<Area> { new Area(0, _biomes[0]) }; //TODO :eyes:
    }

    public int GetAreaCount()
    {
        return _areas.Count;
    }

    public Area GetArea(int index)
    {
        return _areas[index];
    }
}
