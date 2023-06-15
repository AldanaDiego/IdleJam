using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : Singleton<AreaManager>
{
    [SerializeField] private BiomeDB _biomeDB;
    [SerializeField] private Biome _startBiome;

    private List<Area> _areas;

    protected override void Awake()
    {
        base.Awake();
        _areas = new List<Area> { new Area(0, _startBiome) };
    }

    public void SetupFromSave(List<Area> areas, List<Squad> squads)
    {
        _areas = areas;
        _areas.Sort();
        foreach (Squad squad in squads)
        {
            if (squad.GetArea() != -1)
            {
                _areas[squad.GetArea()].AddSquad(squad);
            }
        }
    }

    public int GetAreaCount()
    {
        return _areas.Count;
    }

    public Area GetArea(int index)
    {
        return _areas[index];
    }

    public List<Area> GetAreas()
    {
        return _areas;
    }
}
