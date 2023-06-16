using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadExplorationEvent
{
    public Squad Squad;
    public Area Area;
    public ExplorationEvent ExplorationEvent;
    public string EventDetails;

    public SquadExplorationEvent(Squad squad, Area area, ExplorationEvent explorationEvent)
    {
        Squad = squad;
        Area = area;
        ExplorationEvent = explorationEvent;
        EventDetails = null;
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
