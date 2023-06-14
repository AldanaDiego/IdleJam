using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceStock : Singleton<ResourceStock>
{
    [SerializeField] private ResourceDB _resourceDB;
    private Dictionary<ResourceData, int> _stock;

    public event EventHandler<Dictionary<ResourceData, int>> OnResourcesStockChanged;

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
        DroneManager.GetInstance().OnDroneBuilt += OnDroneBuilt;
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

    private void OnDroneBuilt(object sender, DroneData drone)
    {
        Dictionary<ResourceData, int> newStocks = new Dictionary<ResourceData, int>();
        foreach (var cost in drone.Cost)
        {
            _stock[cost.Key] -= cost.Value;
            newStocks[cost.Key] = _stock[cost.Key];
        }
        OnResourcesStockChanged?.Invoke(this, newStocks);
    }

    private void OnDestroy()
    {
        DroneManager.GetInstance().OnDroneBuilt -= OnDroneBuilt;
    }
}
