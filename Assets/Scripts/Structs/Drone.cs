using UnityEngine;
using System;

[Serializable]
public class Drone
{
    private DroneData _data;
    private int _squad = -1;

    public Drone(DroneData data)
    {
        _data = data;
        _squad = -1;
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
}
