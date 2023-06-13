using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroneManager : Singleton<DroneManager>
{
    [SerializeField] private DroneData _squadLeaderDrone;
    private List<Drone> _drones;
    public event EventHandler<DroneData> OnDroneBuilt;

    private void Start()
    {
        _drones = new List<Drone>();
    }

    public void BuildDrone(DroneData data)
    {
        if (data != _squadLeaderDrone)
        {
            _drones.Add(new Drone(data));
        }
        OnDroneBuilt?.Invoke(this, data);
    }

    public List<Drone> GetDrones()
    {
        return _drones;
    }
}
