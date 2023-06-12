using System;

[Serializable]
public class Drone
{
    private DroneData _data;
    
    public Drone(DroneData data)
    {
        _data = data;
    }
}
