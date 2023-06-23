using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplorationManager : Singleton<ExplorationManager>
{
    [SerializeField] private ExplorationEvent _defaultEvent;
    [SerializeField] private ExplorationEvent _mutagenMutationEvent;

    private UIStateManager _stateManager;
    private AreaManager _areaManager;
    private SquadManager _squadManager;
    private BiomeMutationManager _biomeMutationManager;
    private Dictionary<Area,Biome> _biomeChanges;

    private const int EVENTS_PER_DEPLOY = 5;

    public event EventHandler<SquadExplorationEvent[]> OnExplorationEventsTriggered;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _areaManager = AreaManager.GetInstance();
        _squadManager = SquadManager.GetInstance();
        _biomeMutationManager = BiomeMutationManager.GetInstance();
        _stateManager.OnStateChanged += OnStateChanged;
        ExploreLogManager.OnExploreLogFinished += OnExploreLogFinished;
        _biomeChanges = new Dictionary<Area, Biome>();
    }

    private void GenerateExploringEvents()
    {
        int squadCountTotal = _squadManager.GetAssignedSquads().Count;
        SquadExplorationEvent[] triggeredEvents = new SquadExplorationEvent[squadCountTotal * EVENTS_PER_DEPLOY];
        int currentSquad = 0;
        foreach (Area area in _areaManager.GetAreas())
        {
            List<ExplorationEventChance> areaEvents = new List<ExplorationEventChance>(area.GetBiome().ExplorationEventChances);
            foreach (Squad squad in area.GetSquads())
            {
                for (int i = 0; i < EVENTS_PER_DEPLOY; i++)
                {
                    int index = currentSquad + (i * squadCountTotal);
                    SquadExplorationEvent explorationEvent;
                    if (i == EVENTS_PER_DEPLOY - 1 && squad.GetMutagens().Count > 0)
                    {
                        explorationEvent = new SquadExplorationEvent(squad, area, _mutagenMutationEvent);
                    }
                    else
                    {
                        explorationEvent = new SquadExplorationEvent(squad, area, PickEventByChance(areaEvents));
                    }
                    triggeredEvents[index] = explorationEvent;

                    if (explorationEvent.ExplorationEvent.IsUniquePerExploration)
                    {
                        areaEvents.RemoveAll(areaEvent => areaEvent.Event.Equals(explorationEvent.ExplorationEvent));
                    }
                }
                currentSquad++;
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
        List<ResourceChance> basicResourceChances = area.GetBiome().ResourceChances.FindAll(chance => chance.Resource.IsBasicResource);
        List<Drone> cargoDrones = squad.GetCargoDrones();
        foreach (Drone drone in squad.GetDrones())
        {
            if (drone.CanMine())
            {
                if (drone.CanAddCargo())
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
                else
                {
                    foreach (Drone cargoDrone in cargoDrones)
                    {
                        if (cargoDrone.CanAddCargo())
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
                            cargoDrone.AddCargo(mineResource, drone.GetMiningSpeed());
                        }
                    }
                }
            }
        }
    }

    private void HandleSquadExplorationEvent(SquadExplorationEvent squadEvent)
    {
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

            case ExplorationEvent.EventType.BIOME_NATURAL_MUTATION:
                int propertyChanged = UnityEngine.Random.Range(0, 3);
                Biome biome = squadEvent.Area.GetBiome();
                List<BiomePropertyApplies> biomeChanges = new List<BiomePropertyApplies>();
                BiomePropertyApplies isApplied = biome.Properties.Find(prop => (int) prop.Property == propertyChanged);
                biomeChanges.Add(new BiomePropertyApplies(isApplied.Property, !isApplied.Applies));

                Biome newBiome = _biomeMutationManager.GetBiomeMutation(biome, biomeChanges);
                _biomeChanges[squadEvent.Area] = newBiome;
                squadEvent.EventDetails = $"{squadEvent.Area} " + GetBiomePropertyChangeMessage(isApplied.Property, !isApplied.Applies);
                break;

            case ExplorationEvent.EventType.BIOME_INDUCED_MUTATION:
                List<Mutagen> mutagens = squadEvent.Squad.GetMutagens();
                biome = _biomeChanges.ContainsKey(squadEvent.Area) ? _biomeChanges[squadEvent.Area] : squadEvent.Area.GetBiome();
                newBiome = _biomeMutationManager.GetBiomeMutation(biome, mutagens);
                _biomeChanges[squadEvent.Area] = newBiome;
                if (mutagens.Count == 1)
                {
                    squadEvent.EventDetails = $"Through mutagens, {squadEvent.Area} " + GetBiomePropertyChangeMessage(mutagens[0].BiomeProperty, mutagens[0].AppliesProperty);
                }
                else
                {
                    squadEvent.EventDetails = $"Through mutagens, {squadEvent.Area} is changing";
                }
                
                break;

            case ExplorationEvent.EventType.NO_EVENT:
                /*
                //TODO
                string mineResult = MineResources(squadEvent.Squad, squadEvent.Area);
                if (mineResult != null)
                {
                    squadEvent.EventDetails = mineResult;
                }
                canSquadMine = false;
                */
                break;
            default:
                break;
        }

        if (canSquadMine)
        {
            MineResources(squadEvent.Squad, squadEvent.Area);
        }
    }

    private string GetBiomePropertyChangeMessage(Biome.BiomeProperty property, bool isApplied)
    {
        switch (property)
        {
            case Biome.BiomeProperty.HUMIDITY:
                return isApplied ? "humidity is incresing" : "humidity is decreasing";
            case Biome.BiomeProperty.TEMPERATURE:
                return isApplied ? "is becoming warmer" : "is becoming colder";
            case Biome.BiomeProperty.VEGETATION:
                return isApplied ? "is growing more vegetation" : "is losing vegetation";
            case Biome.BiomeProperty.STATIC:
                return isApplied ? "atmosphere has more electric charge" : "atmosphere has less electric charge";
            case Biome.BiomeProperty.POLLUTION:
                return isApplied ? "is becoming polluted" : "air is cleaner";
            default:
                return "";
        }
    }

    private bool MineRareResource(Squad squad, Area area)
    {
        List<ResourceChance> rareResourceChances = area.GetBiome().ResourceChances.FindAll(chance => !chance.Resource.IsBasicResource);
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

    private void ApplyBiomeChanges()
    {
        foreach ((Area area, Biome biome) in _biomeChanges)
        {
            area.SetBiome(biome);
        }
        _biomeChanges.Clear();
    }

    private void OnStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == UIStateManager.GameState.EXPLORING)
        {
            GenerateExploringEvents();
        }
    }

    private void OnExploreLogFinished(object sender, EventArgs empty)
    {
        ApplyBiomeChanges();
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= OnStateChanged;
        ExploreLogManager.OnExploreLogFinished -= OnExploreLogFinished;
    }
}
