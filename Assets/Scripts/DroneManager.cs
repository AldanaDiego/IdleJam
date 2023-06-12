using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroneManager : Singleton<DroneManager>
{
    private List<Drone> _drones;
    public event EventHandler<DroneData> OnDroneBuilt;

    private void Start()
    {
        _drones = new List<Drone>();
    }

    public void BuildDrone(DroneData data)
    {
        _drones.Add(new Drone(data));
        OnDroneBuilt?.Invoke(this, data);
    }
}
