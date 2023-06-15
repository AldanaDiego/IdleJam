using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Drone : ISerializationCallbackReceiver
{
    [SerializeField] private DroneData _data;
    [SerializeField] private int _squad = -1;

    private Dictionary<ResourceData, int> _resourceCargo;
    private int _currentCargo;

    public Drone(DroneData data)
    {
        _data = data;
        _squad = -1;
        _currentCargo = 0;
        _resourceCargo = new Dictionary<ResourceData, int>();
    }

    public Sprite GetImage()
    {
        return _data.Image;
    }

    public int GetSquad()
    {
        return _squad;
    }

    public void SetSquad(int newSquad)
    {
        _squad = newSquad;
    }

    public bool IsSquadLeader()
    {
        return _data.IsLeader;
    }

    public int GetMiningSpeed()
    {
        return _data.MiningSpeed;
    }

    public int GetCargoCapacity()
    {
        return _data.CargoCapacity;
    }

    public int GetMaxHp()
    {
        return _data.MaxHp;
    }

    public int GetCurrentCargo()
    {
        return _currentCargo;
    }

    public bool AddCargo(ResourceData resource, int amount)
    {
        int amountToAdd = Math.Min(amount, GetCargoCapacity() - _currentCargo);
        if (amountToAdd > 0)
        {
            if (_resourceCargo.ContainsKey(resource))
            {
                _resourceCargo[resource] = _resourceCargo[resource] + amountToAdd;
            }
            else
            {
                _resourceCargo[resource] = amountToAdd;
            }
            _currentCargo += amountToAdd;
            return true;
        }
        return false;
    }

    public bool AddCargo(ResourceData resource)
    {
        return AddCargo(resource, GetMiningSpeed());
    }

    public Dictionary<ResourceData, int> GetResourceCargo()
    {
        return _resourceCargo;
    }

    public void RemoveCargo()
    {
        _resourceCargo.Clear();
        _currentCargo = 0;
    }

    public bool CanMine()
    {
        return (_data.MiningSpeed > 0) && (_data.CargoCapacity - _currentCargo > 0);
    }

    public override string ToString()
    {
        return _data.Name;
    }

    public void OnBeforeSerialize(){}

    public void OnAfterDeserialize()
    {
        _currentCargo = 0;
        _resourceCargo = new Dictionary<ResourceData, int>();
    }
}
