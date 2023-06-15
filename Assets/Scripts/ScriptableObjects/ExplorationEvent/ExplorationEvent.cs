using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplorationEventScriptableObject", menuName = "ScriptableObjects/ExplorationEvent")]
public class ExplorationEvent : ScriptableObject
{
    public enum EventType { RARE_ITEM }

    public string Name;
    public string Description;
    public EventType ExplorationEventType;
}
