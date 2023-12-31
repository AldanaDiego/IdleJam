using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDataScriptableObject", menuName = "ScriptableObjects/ResourceData")]
public class ResourceData : ScriptableObject
{
    public string Name;
    public bool IsBasicResource;
    public int InitialAmount;
    public bool IsSingleFind;

    public override string ToString()
    {
        return Name;
    }
}
