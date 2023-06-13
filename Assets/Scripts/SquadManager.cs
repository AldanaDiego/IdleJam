using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : Singleton<SquadManager>
{
    [SerializeField] private DroneData _squadLeaderDrone;
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

    public int GetSquadCount()
    {
        return _squads.Count;
    }

    public int GetSquadSizeLimit()
    {
        return _squadSizeLimit;
    }

    public List<Drone> GetSquad(int squadNumber)
    {
        return _squads[squadNumber].GetDrones();
    }

    public DroneData GetSquadLeaderData()
    {
        return _squadLeaderDrone;
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

    private void OnDroneBuilt(object sender, DroneData data)
    {
        if (data == _squadLeaderDrone)
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
