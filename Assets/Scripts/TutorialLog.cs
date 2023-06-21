using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLog : Singleton<TutorialLog>
{
    [SerializeField] private TutorialLogEntryDB _tutorialLogEntryDB;
    [SerializeField] private DroneData _cargoDroneData;
    [SerializeField] private DroneData _mutagenDroneData;
    [SerializeField] private MutagenDB _naturalMutagens;
    [SerializeField] private MutagenDB _chemicalMutagens;

    private ExplorationManager _explorationManager;
    private DroneManager _droneManager;
    private BiomeMutationManager _biomeMutationManager;
    private List<TutorialLogEntry> _entriesDB;
    private List<TutorialLogEntry> _unlockedEntries;
    
    private int _explorationsFinished;
    private int _explorationsSinceMutagenUnlocked;
    private bool _hasUnlockedMutagenDrone;

    protected override void Awake()
    {
        base.Awake();
        _entriesDB = new List<TutorialLogEntry>(_tutorialLogEntryDB.TutorialLogEntries);
        _unlockedEntries = new List<TutorialLogEntry>();
        UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod.DEFAULT);
        _explorationsFinished = 0;
        _explorationsSinceMutagenUnlocked = 0;
        _hasUnlockedMutagenDrone = false;
    }

    private void Start()
    {
        _explorationManager = ExplorationManager.GetInstance();
        _droneManager = DroneManager.GetInstance();
        _biomeMutationManager = BiomeMutationManager.GetInstance();
        _explorationManager.OnExplorationEventsTriggered += OnExplorationEventsTriggered;
        _droneManager.OnDroneBuilt += OnDroneBuilt;
    }

    private void UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod method)
    {
        foreach (TutorialLogEntry entry in _entriesDB.FindAll(entry => entry.UnlockMethod == method))
        {
            _entriesDB.Remove(entry);
            _unlockedEntries.Add(entry);
        }
    }

    public void SetupFromSave(TutorialLogSave save)
    {
        _entriesDB = save.EntriesDB;
        _unlockedEntries = save.UnlockedEntries;
        _explorationsFinished = save.ExplorationsFinished;
        _hasUnlockedMutagenDrone = save.HasUnlockedMutagenDrone;
        _explorationsSinceMutagenUnlocked = save.ExplorationsSinceMutagenUnlocked;
    }

    public List<TutorialLogEntry> GetUnlockedTutorials()
    {
        return _unlockedEntries;
    }

    public TutorialLogSave GetTutorialLogSave()
    {
        return new TutorialLogSave()
        {
            EntriesDB = _entriesDB,
            UnlockedEntries = _unlockedEntries,
            ExplorationsFinished = _explorationsFinished,
            HasUnlockedMutagenDrone = _hasUnlockedMutagenDrone,
            ExplorationsSinceMutagenUnlocked = _explorationsSinceMutagenUnlocked
        };
    }
    
    public bool HasUnlockedMutagenDrone()
    {
        return _hasUnlockedMutagenDrone;
    }

    private void OnExplorationEventsTriggered(object sender, SquadExplorationEvent[] squadEVents)
    {
        _explorationsFinished++;
        if (_hasUnlockedMutagenDrone)
        {
            _explorationsSinceMutagenUnlocked++;
        }
        if (_explorationsFinished == 1)
        {
            _droneManager.UnlockDrone(_cargoDroneData);
        }

        if (_explorationsFinished == 2)
        {
            _droneManager.UnlockDrone(_mutagenDroneData);
            _hasUnlockedMutagenDrone = true;

        }

        if (_explorationsSinceMutagenUnlocked == 1)
        {
            foreach (Mutagen mutagen in _chemicalMutagens.Mutagens)
            {
                _biomeMutationManager.UnlockMutagen(mutagen);
            }
        }
    }

    private void OnDroneBuilt(object sender, DroneData data)
    {
        if (data == _mutagenDroneData)
        {
            foreach (Mutagen mutagen in _naturalMutagens.Mutagens)
            {
                _biomeMutationManager.UnlockMutagen(mutagen);
            }
        }
    }
}
