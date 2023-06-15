using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroneManager : Singleton<DroneManager>
{
    private List<Drone> _drones;
    public event EventHandler<DroneData> OnDroneBuilt;

    protected override void Awake()
    {
        base.Awake();
        _drones = new List<Drone>();
    }

    public void SetupFromSave(List<Squad> squads)
    {
        foreach (Squad squad in squads)
        {
            foreach (Drone drone in squad.GetDrones())
            {
                if (!drone.IsSquadLeader())
                {
                    _drones.Add(drone);
                }
            }
        }
    }

    public void BuildDrone(DroneData data)
    {
        if (!data.IsLeader)
        {
            _drones.Add(new Drone(data));
        }
        OnDroneBuilt?.Invoke(this, data);
    }

    public List<Drone> GetDrones()
    {
        return _drones;
    }

    public List<Drone> GetAvailableDrones()
    {
        return _drones.FindAll(drone => drone.GetSquad() == -1);
    }
}
