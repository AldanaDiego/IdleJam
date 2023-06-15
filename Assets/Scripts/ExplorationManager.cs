using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplorationManager : Singleton<ExplorationManager>
{
    [SerializeField] private ExplorationEvent _defaultEvent;

    private UIStateManager _stateManager;
    private AreaManager _areaManager;
    private SquadManager _squadManager;

    private const int EVENTS_PER_DEPLOY = 3;

    public event EventHandler<SquadExplorationEvent[]> OnExplorationEventsTriggered;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _areaManager = AreaManager.GetInstance();
        _squadManager = SquadManager.GetInstance();
        _stateManager.OnStateChanged += OnStateChanged;
    }

    private void GenerateExploringEvents()
    {
        //List<SquadExplorationEvent> triggeredEvents = new List<SquadExplorationEvent>();
        int squadCount = _squadManager.GetAssignedSquads().Count;
        SquadExplorationEvent[] triggeredEvents = new SquadExplorationEvent[squadCount * EVENTS_PER_DEPLOY];
        foreach (Area area in _areaManager.GetAreas())
        {
            foreach (Squad squad in area.GetSquads())
            {
                for (int i = 0; i < EVENTS_PER_DEPLOY; i++)
                {
                    int index = squad.GetSquadNumber() + (i * squadCount);
                    SquadExplorationEvent explorationEvent = new SquadExplorationEvent(squad, area, PickEventByChance(area.GetBiomeEvents()));
                    triggeredEvents[index] = explorationEvent;
                }
            }
        }

        foreach (SquadExplorationEvent squadEvent in triggeredEvents)
        {
            HandleSquadExplorationEvent(squadEvent);
        }
        OnExplorationEventsTriggered?.Invoke(this, triggeredEvents);
    }

    private ExplorationEvent PickEventByChance(List<ExplorationEventChance> eventChances)
    {
        float totalChance = UnityEngine.Random.value;
        foreach (ExplorationEventChance eventChance in eventChances)
        {
            totalChance -= eventChance.Chance;
            if (totalChance <= 0f)
            {
                return eventChance.Event;
            }
        }
        return _defaultEvent;
    }

    private void MineResources(Squad squad, Area area)
    {
        List<ResourceChance> basicResourceChances = area.GetBiomeResourceChances().FindAll(chance => chance.Resource.IsBasicResource);
        foreach (Drone drone in squad.GetDrones())
        {
            if (drone.CanMine())
            {
                float totalChance = UnityEngine.Random.value;
                ResourceData mineResource = null;
                foreach (ResourceChance resourceChance in basicResourceChances)
                {
                    totalChance -= resourceChance.Chance;
                    if (totalChance <= 0f)
                    {
                        mineResource = resourceChance.Resource;
                        break;
                    }
                }
                drone.AddCargo(mineResource);
            }
        }
    }

    private void HandleSquadExplorationEvent(SquadExplorationEvent squadEvent)
    {
        //TODO this gonna be a big one D:
        bool canSquadMine = true;
        
        switch (squadEvent.ExplorationEvent.ExplorationEventType)
        {
            case ExplorationEvent.EventType.RARE_ITEM:
                canSquadMine = MineRareResource(squadEvent.Squad, squadEvent.Area);
                break;

            case ExplorationEvent.EventType.DISCOVER_AREA:
                if (_areaManager.CanDiscoverNewArea())
                {
                    _areaManager.DiscoverNewArea();
                }
                else
                {
                    squadEvent.EventDetails = _defaultEvent.Description;
                }
                break;

            case ExplorationEvent.EventType.NO_EVENT:
            default:
                break;
        }

        if (canSquadMine)
        {
            MineResources(squadEvent.Squad, squadEvent.Area);
        }
    }

    private bool MineRareResource(Squad squad, Area area)
    {
        List<ResourceChance> rareResourceChances = area.GetBiomeResourceChances().FindAll(chance => !chance.Resource.IsBasicResource);
        float totalChance = UnityEngine.Random.value;
        ResourceData mineResource = null;
        foreach (ResourceChance resourceChance in rareResourceChances)
        {
            totalChance -= resourceChance.Chance;
            if (totalChance <= 0f)
            {
                mineResource = resourceChance.Resource;
                break;
            }
        }

        if (mineResource.IsSingleFind)
        {
            foreach (Drone drone in squad.GetDrones())
            {
                if (drone.AddCargo(mineResource, 1))
                {
                    break;
                }
            }
        }
        else
        {
            foreach (Drone drone in squad.GetDrones())
            {
                if (drone.CanMine())
                {
                    drone.AddCargo(mineResource);
                }
            }
        }

        return mineResource.IsSingleFind;
    }

    private void OnStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == UIStateManager.GameState.EXPLORING)
        {
            GenerateExploringEvents();
        }
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= OnStateChanged;
    }
}
