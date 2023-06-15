using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : Singleton<SquadManager>
{
    private List<Squad> _squads;
    private int _squadSizeLimit = 6;

    protected override void Awake()
    {
        base.Awake();
        _squads = new List<Squad>();
    }

    private void Start()
    {
        DroneManager.GetInstance().OnDroneBuilt += OnDroneBuilt;
    }

    public void SetupFromSave(List<Squad> squads)
    {
        _squads = squads;
        _squads.Sort();
    }

    public int GetSquadCount()
    {
        return _squads.Count;
    }

    public int GetSquadSizeLimit()
    {
        return _squadSizeLimit;
    }

    public List<Drone> GetSquadMembers(int squadNumber)
    {
        return _squads[squadNumber].GetDrones();
    }

    public Squad GetSquad(int squadNumber)
    {
        return _squads[squadNumber];
    }

    public void AddToSquad(Drone drone, int squad)
    {
        _squads[squad].AddDrone(drone);
    }

    public void RemoveFromSquad(Drone drone, int squad)
    {
        _squads[squad].RemoveDrone(drone);
    }

    public bool HasReadySquads()
    {
        foreach (Squad squad in _squads)
        {
            if (squad.GetDroneCount() > 1)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasAssignedSquads()
    {
        foreach (Squad squad in _squads)
        {
            if (squad.GetArea() != -1)
            {
                return true;
            }
        }
        return false;
    }

    public List<Squad> GetUnassignedSquads()
    {
        return _squads.FindAll(squad => (squad.GetDroneCount() > 1 && squad.GetArea() == -1));   
    }

    public List<Squad> GetAssignedSquads()
    {
        return _squads.FindAll(squad => (squad.GetArea() != -1));   
    }

    public List<Squad> GetAllSquads()
    {
        return _squads;
    }

    private void OnDroneBuilt(object sender, DroneData data)
    {
        if (data.IsLeader)
        {
            _squads.Add(new Squad(_squads.Count));
            AddToSquad(new Drone(data), _squads.Count - 1);
        }
    }

    private void OnDestroy()
    {
        DroneManager.GetInstance().OnDroneBuilt -= OnDroneBuilt;
    }
}
