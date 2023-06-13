using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Squad
{
    private List<Drone> _drones;
    public int _area;
    private int _squadNumber;

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
}
