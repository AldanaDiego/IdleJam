using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplorationEventScriptableObject", menuName = "ScriptableObjects/ExplorationEvent")]
public class ExplorationEvent : ScriptableObject
{
    public enum EventType { NO_EVENT, RARE_ITEM, DISCOVER_AREA, BIOME_NATURAL_MUTATION, BIOME_INDUCED_MUTATION }

    public string Name;
    public string Description;
    public EventType ExplorationEventType;
    public bool IsUniquePerExploration;

    public bool Equals(ExplorationEvent other)
    {
        return ExplorationEventType == other.ExplorationEventType;
    }
}
