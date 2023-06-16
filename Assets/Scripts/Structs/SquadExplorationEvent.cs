using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadExplorationEvent
{
    public Squad Squad;
    public Area Area;
    public ExplorationEvent ExplorationEvent;
    public string EventDetails;
    public bool HasCameraEvent;

    public SquadExplorationEvent(Squad squad, Area area, ExplorationEvent explorationEvent)
    {
        Squad = squad;
        Area = area;
        ExplorationEvent = explorationEvent;
        EventDetails = null;
        HasCameraEvent = true;
    }

    public override string ToString()
    {
        if (EventDetails != null)
        {
            return $">{Squad}: {EventDetails}";
        }

        return $">{Squad}: {ExplorationEvent.Description}";
    }
}
