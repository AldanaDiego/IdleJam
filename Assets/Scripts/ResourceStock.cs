using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceStock : Singleton<ResourceStock>
{
    [SerializeField] private ResourceDB _resourceDB;

    private SquadManager _squadManager;
    private Dictionary<ResourceData, int> _stock;

    public event EventHandler OnResourcesStockChanged;

    override protected void Awake()
    {
        base.Awake();
        _stock = new Dictionary<ResourceData, int>();
        foreach (ResourceData resource in _resourceDB.Resources)
        {
            _stock[resource] = (resource.IsBasicResource ? 10 : 2); //TODO SHOULD BE 0
        }
    }

    private void Start()
    {
        _squadManager = SquadManager.GetInstance();
        DroneManager.GetInstance().OnDroneBuilt += OnDroneBuilt;
        ExploreLogManager.OnExploreLogFinished += OnExploreLogFinished;
    }

    public int GetResourceStock(ResourceData resource)
    {
        return _stock[resource];
    }

    public bool CanBuildDrone(DroneData drone)
    {
        foreach (var cost in drone.Cost)
        {
            if (cost.Value > _stock[cost.Key])
            {
                return false;
            }
        }
        return true;
    }

    public Dictionary<ResourceData, int> GetAllStocks()
    {
        return _stock;
    }

    private void CollectCargoFromSquads()
    {
        List<Squad> squads = _squadManager.GetAssignedSquads();
        foreach (Squad squad in squads)
        {
            foreach (Drone drone in squad.GetDrones())
            {
                if (drone.GetCurrentCargo() > 0)
                {
                    Dictionary<ResourceData, int> droneCargo = drone.GetResourceCargo();
                    foreach (var cargo in droneCargo)
                    {
                        _stock[cargo.Key] = _stock[cargo.Key] + cargo.Value;
                    }
                    drone.RemoveCargo();
                }
            }
        }
        OnResourcesStockChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnDroneBuilt(object sender, DroneData drone)
    {
        Dictionary<ResourceData, int> newStocks = new Dictionary<ResourceData, int>();
        foreach (var cost in drone.Cost)
        {
            _stock[cost.Key] -= cost.Value;
            newStocks[cost.Key] = _stock[cost.Key];
        }
        OnResourcesStockChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnExploreLogFinished(object sender, EventArgs empty)
    {
        CollectCargoFromSquads();
    }

    private void OnDestroy()
    {
        DroneManager.GetInstance().OnDroneBuilt -= OnDroneBuilt;
        ExploreLogManager.OnExploreLogFinished -= OnExploreLogFinished;
    }
}
