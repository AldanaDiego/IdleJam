using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneDBScriptableObject", menuName = "ScriptableObjects/DroneDB")]
public class DroneDB : ScriptableObject
{
    public List<DroneData> Drones;
}
