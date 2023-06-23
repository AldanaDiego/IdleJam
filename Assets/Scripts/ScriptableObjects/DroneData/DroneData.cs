using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneDataScriptableObject", menuName = "ScriptableObjects/DroneData")]
public class DroneData : ScriptableObject, ISerializationCallbackReceiver
{
    public string Name;
    public Sprite Image;
    public Dictionary<ResourceData, int> Cost;
    public bool IsLeader;
    public bool IsMutagen;
    public bool IsCargo;
    public int MiningSpeed;
    public int CargoCapacity;
    public int MaxHp;

    [SerializeField] private ResourceAmount[] _costArray;

    public void OnBeforeSerialize()
    {
        return;
    }

    //Scriptable Objects don't support Dictionary, so use an array and cast it on read
    public void OnAfterDeserialize()
    {
        Cost = new Dictionary<ResourceData, int>();
        foreach (ResourceAmount amount in _costArray)
        {
            Cost.Add(amount.Resource, amount.Amount);
        }
    }
}
