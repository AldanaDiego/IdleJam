using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceStock : Singleton<ResourceStock>
{
    [SerializeField] private List<ResourceData> _resourceList;
    private Dictionary<ResourceData, int> _stock;

    public event EventHandler<Dictionary<ResourceData, int>> OnResourcesStockChanged;

    override protected void Awake()
    {
        base.Awake();
        _stock = new Dictionary<ResourceData, int>();
        foreach (ResourceData resource in _resourceList)
        {
            _stock[resource] = 3; //TODO should be zero but testing xd
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
        foreach (var stock in _stock)
        {
            if (drone.Cost[stock.Key] > stock.Value)
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
