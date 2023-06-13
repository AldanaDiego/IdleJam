using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Area
{
    private List<Squad> _squads;
    private Biome _biome;
    private int _areaNumber;

    public Area(int areaNumber, Biome biome)
    {
        _areaNumber = areaNumber;
        _biome = biome;
        _squads = new List<Squad>();
    }

    public string GetBiomeName()
    {
        return _biome.Name;
    }

    public string GetBiomeDetails()
    {
        return _biome.Details;
    }

    public List<Squad> GetSquads()
    {
        return _squads;
    }

    public void AddSquad(Squad squad)
    {
        squad.SetArea(_areaNumber);
        _squads.Add(squad);
    }

    public void RemoveSquad(Squad squad)
    {
        squad.SetArea(-1);
        _squads.Remove(squad);
    }
}
