using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Area : IComparable<Area>, ISerializationCallbackReceiver
{
    [SerializeField] private Biome _biome;
    [SerializeField] private int _areaNumber;

    private List<Squad> _squads;

    public Area(int areaNumber, Biome biome)
    {
        _areaNumber = areaNumber;
        _biome = biome;
        _squads = new List<Squad>();
    }

    public Biome GetBiome()
    {
        return _biome;
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

    public int GetAreaNumber()
    {
        return _areaNumber;
    }

    public override string ToString()
    {
        return "Area " + _areaNumber.ToString("D2");
    }

    public int CompareTo(Area other)
    {
        return _areaNumber.CompareTo(other.GetAreaNumber());
    }

    public void OnBeforeSerialize(){}

    public void OnAfterDeserialize()
    {
        _squads = new List<Squad>();
    }
}
