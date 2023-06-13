using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : Singleton<SquadManager>
{
    [SerializeField] private DroneData _squadLeaderDrone;
    private List<List<Drone>> _squads;
    private int _squadSizeLimit = 6;

    protected override void Awake()
    {
        base.Awake();
        _squads = new List<List<Drone>>();
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
        return _squads[squadNumber];
    }

    public DroneData GetSquadLeaderData()
    {
        return _squadLeaderDrone;
    }

    public void AddToSquad(Drone drone, int squad)
    {
        drone.SetSquad(squad);
        _squads[squad].Add(drone);
    }

    public void RemoveFromSquad(Drone drone, int squad)
    {
        drone.SetSquad(-1);
        _squads[squad].Remove(drone);
    }

    public bool HasReadySquads()
    {
        foreach (List<Drone> squad in _squads)
        {
            if (squad.Count > 1)
            {
                return true;
            }
        }
        return false;
    }

    public List<int> GetReadySquads()
    {
        List<int> readySquads = new List<int>();
        for (int i = 0; i < _squads.Count; i++)
        {
            if (_squads[i].Count > 1)
            {
                readySquads.Add(i);
            }
        }
        return readySquads;
    }

    private void OnDroneBuilt(object sender, DroneData data)
    {
        if (data == _squadLeaderDrone)
        {
            _squads.Add(new List<Drone>());
            AddToSquad(new Drone(data), _squads.Count - 1);
        }
    }

    private void OnDestroy()
    {
        DroneManager.GetInstance().OnDroneBuilt -= OnDroneBuilt;
    }
}
