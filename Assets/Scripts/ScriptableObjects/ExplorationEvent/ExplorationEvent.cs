using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplorationEventScriptableObject", menuName = "ScriptableObjects/ExplorationEvent")]
public class ExplorationEvent : ScriptableObject, IEqualityComparer<ExplorationEvent>
{
    public enum EventType { NO_EVENT, RARE_ITEM, DISCOVER_AREA, BIOME_NATURAL_MUTATION, BIOME_INDUCED_MUTATION }

    public string Name;
    public string Description;
    public EventType ExplorationEventType;
    public bool IsUniquePerExploration;

    public bool Equals(ExplorationEvent x, ExplorationEvent y)
    {
        return x.ExplorationEventType == y.ExplorationEventType;
    }

    public int GetHashCode(ExplorationEvent obj)
    {
        return (int) ExplorationEventType;
    }
}
