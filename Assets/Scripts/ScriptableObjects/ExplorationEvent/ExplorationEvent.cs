using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplorationEventScriptableObject", menuName = "ScriptableObjects/ExplorationEvent")]
public class ExplorationEvent : ScriptableObject
{
    public string Name;
    public string Description;
}
