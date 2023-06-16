using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroneManager : Singleton<DroneManager>
{
    [SerializeField] private List<DroneModel> _dronePrefabs;

    private List<Drone> _drones;
    private Dictionary<DroneData, Transform> _droneModels;
    public event EventHandler<DroneData> OnDroneBuilt;

    protected override void Awake()
    {
        base.Awake();
        _drones = new List<Drone>();
    }

    private void Start()
    {
        _droneModels = new Dictionary<DroneData, Transform>();
        foreach (DroneModel dronePrefab in _dronePrefabs)
        {
            _droneModels[dronePrefab.Drone] = dronePrefab.Prefab;
        }
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

    public Transform GetDronePrefab(DroneData drone)
    {
        return _droneModels[drone];
    }

    public List<Drone> GetAvailableDrones()
    {
        return _drones.FindAll(drone => drone.GetSquad() == -1);
    }
}
