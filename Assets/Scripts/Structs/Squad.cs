using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Squad : IComparable<Squad>
{
    [SerializeField] private List<Drone> _drones;
    [SerializeField] private int _area;
    [SerializeField] private int _squadNumber;

    public Squad(int squadNumber)
    {
        _drones = new List<Drone>();
        _area = -1;
        _squadNumber = squadNumber;
    }

    public int GetSquadNumber()
    {
        return _squadNumber;
    }

    public void AddDrone(Drone drone)
    {
        drone.SetSquad(_squadNumber);
        _drones.Add(drone);
    }

    public void RemoveDrone(Drone drone)
    {
        drone.SetSquad(-1);
        _drones.Remove(drone);
    }

    public int GetDroneCount()
    {
        return _drones.Count;
    }

    public List<Drone> GetDrones()
    {
        return _drones;
    }

    public int GetArea()
    {
        return _area;
    }

    public void SetArea(int newArea)
    {
        _area = newArea;
    }

    public bool IsAssigned()
    {
        return _area != -1;
    }

    public bool HasMutagenDrone()
    {
        int index = _drones.FindIndex(drone => drone.GetDroneData().IsMutagen);
        return index >= 0;
    }

    public override string ToString()
    {
        return "Squad " + _squadNumber.ToString("D2");
    }

    public int CompareTo(Squad other)
    {
        return _squadNumber.CompareTo(other.GetSquadNumber());
    }
}
