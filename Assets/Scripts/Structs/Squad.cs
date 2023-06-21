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
    [SerializeField] private List<Mutagen> _mutagens;

    public Squad(int squadNumber)
    {
        _drones = new List<Drone>();
        _area = -1;
        _squadNumber = squadNumber;
        _mutagens = new List<Mutagen>();
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
        if (drone.GetDroneData().IsMutagen)
        {
            RemoveMutagen();
        }
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

    public bool AddMutagen(Mutagen mutagen)
    {
        int count = _drones.FindAll(drone => drone.GetDroneData().IsMutagen).Count;
        if (count > _mutagens.Count)
        {
            _mutagens.Add(mutagen);
            return true;
        }
        return false;
    }

    public bool RemoveMutagen()
    {
        if (_mutagens.Count > 0)
        {
            _mutagens.RemoveAt(_mutagens.Count - 1);
            return true;
        }
        return false;
    }

    public List<Mutagen> GetMutagens()
    {
        return _mutagens;
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
