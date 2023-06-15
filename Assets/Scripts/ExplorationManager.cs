using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplorationManager : Singleton<ExplorationManager>
{
    private UIStateManager _stateManager;
    private AreaManager _areaManager;

    public event EventHandler<List<SquadExplorationEvent>> OnExplorationEventsTriggered;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _areaManager = AreaManager.GetInstance();
        _stateManager.OnStateChanged += OnStateChanged;
    }

    private void GenerateExploringEvents()
    {
        List<SquadExplorationEvent> triggeredEvents = new List<SquadExplorationEvent>();
        foreach (Area area in _areaManager.GetAreas())
        {
            foreach (Squad squad in area.GetSquads())
            {
                for (int i = 0; i < 3; i++)
                {
                    ExplorationEvent explorationEvent = PickEventByChance(area.GetBiomeEvents());
                    HandleExplorationEvent(explorationEvent, squad, area);
                    triggeredEvents.Add(new SquadExplorationEvent(squad, explorationEvent));
                }
            }
        }
        //TODO Shuffle Events
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
        return null;
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

    private void HandleExplorationEvent(ExplorationEvent explorationEvent, Squad squad, Area area)
    {
        //TODO this gonna be a big one D:

        bool canSquadMine = true;
        if (explorationEvent != null)
        {
            switch (explorationEvent.ExplorationEventType)
            {
                case ExplorationEvent.EventType.RARE_ITEM:
                    canSquadMine = MineRareResource(squad, area);
                    break;
                default:
                    break;
            }
        }

        if (canSquadMine)
        {
            MineResources(squad, area);
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
