using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : Singleton<SquadManager>
{
    [SerializeField] private DroneData _squadLeaderDrone;
    private List<List<Drone>> _squads;
    private int _squadSizeLimit = 5;

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

    private void OnDroneBuilt(object sender, DroneData data)
    {
        if (data == _squadLeaderDrone)
        {
            _squads.Add(new List<Drone>());
        }
    }

    private void OnDestroy()
    {
        DroneManager.GetInstance().OnDroneBuilt -= OnDroneBuilt;
    }
}
