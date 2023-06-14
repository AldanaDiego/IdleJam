using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadExplorationEvent
{
    public Squad Squad;
    public ExplorationEvent ExplorationEvent;

    public SquadExplorationEvent(Squad squad, ExplorationEvent explorationEvent)
    {
        Squad = squad;
        ExplorationEvent = explorationEvent;
    }

    public override string ToString()
    {
        if (ExplorationEvent == null)
        {
            return $">{Squad} is exploring normally";
        }
        return ExplorationEvent.Description.Replace("SQUAD", Squad.ToString());
    }
}
