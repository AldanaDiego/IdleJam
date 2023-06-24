using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialLog : Singleton<TutorialLog>
{
    [SerializeField] private TutorialLogEntryDB _tutorialLogEntryDB;
    [SerializeField] private DroneData _cargoDroneData;
    [SerializeField] private DroneData _mutagenDroneData;
    [SerializeField] private MutagenDB _naturalMutagens;
    [SerializeField] private MutagenDB _chemicalMutagens;

    public event EventHandler<string> OnFeatureUnlocked;

    private ExplorationManager _explorationManager;
    private DroneManager _droneManager;
    private BiomeMutationManager _biomeMutationManager;
    private List<TutorialLogEntry> _entriesDB;
    private List<TutorialLogEntry> _unlockedEntries;
    
    private int _explorationsFinished;
    private int _explorationsSinceMutagenUnlocked;
    
    private bool _hasBuiltScannerDrone;
    private bool _hasBuiltMinerDrone;
    private bool _hasBuiltCargoDrone;
    private bool _hasBuiltMutagenDrone;

    protected override void Awake()
    {
        base.Awake();
        _entriesDB = new List<TutorialLogEntry>(_tutorialLogEntryDB.TutorialLogEntries);
        _unlockedEntries = new List<TutorialLogEntry>();
        UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod.DEFAULT);
        _explorationsFinished = 0;
        _explorationsSinceMutagenUnlocked = 0;
        _hasBuiltScannerDrone = false;
        _hasBuiltMinerDrone = false;
        _hasBuiltCargoDrone = false;
        _hasBuiltMutagenDrone = false;
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
        _hasBuiltMutagenDrone = save.HasBuiltMutagenDrone;
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
            HasBuiltScannerDrone = _hasBuiltScannerDrone,
            HasBuiltMinerDrone = _hasBuiltMinerDrone,
            HasBuiltCargoDrone = _hasBuiltCargoDrone,
            HasBuiltMutagenDrone = _hasBuiltMutagenDrone,
            ExplorationsSinceMutagenUnlocked = _explorationsSinceMutagenUnlocked
        };
    }
    
    public bool HasBuiltMutagenDrone()
    {
        return _hasBuiltMutagenDrone;
    }

    public int GetExplorationsFinished()
    {
        return _explorationsFinished;
    }

    private void OnExplorationEventsTriggered(object sender, SquadExplorationEvent[] squadEVents)
    {
        _explorationsFinished++;
        if (_hasBuiltMutagenDrone)
        {
            _explorationsSinceMutagenUnlocked++;
        }

        if (_explorationsFinished == 2)
        {
            _droneManager.UnlockDrone(_cargoDroneData);
            OnFeatureUnlocked?.Invoke(this, "Cargo Drone Unlocked");
        }

        if (_explorationsFinished == 7)
        {
            _droneManager.UnlockDrone(_mutagenDroneData);
            OnFeatureUnlocked?.Invoke(this, "Mutagen Drone Unlocked");
        }

        if (_explorationsSinceMutagenUnlocked == 5)
        {
            foreach (Mutagen mutagen in _chemicalMutagens.Mutagens)
            {
                _biomeMutationManager.UnlockMutagen(mutagen);
                
            }
            UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod.ON_CHEMICAL_MUTAGENS_UNLOCKED);
            OnFeatureUnlocked?.Invoke(this, "New Mutagens Unlocked");
        }
    }

    private void OnDroneBuilt(object sender, DroneData data)
    {
        if (data.IsMutagen && !_hasBuiltMutagenDrone)
        {
            UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod.ON_MUTAGEN_BUILT);
            foreach (Mutagen mutagen in _naturalMutagens.Mutagens)
            {
                _biomeMutationManager.UnlockMutagen(mutagen);
            }
            OnFeatureUnlocked?.Invoke(this, "Mutagens Unlocked");
            _hasBuiltMutagenDrone = true;
        }
        else if (data.IsCargo && !_hasBuiltCargoDrone)
        {
            UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod.ON_CARGO_BUILT);
            _hasBuiltCargoDrone = true;
        }
        else if (data.IsLeader && !_hasBuiltScannerDrone)
        {
            UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod.ON_SCANNER_BUILT);
            _hasBuiltScannerDrone = true;
        }
        else if (!_hasBuiltMinerDrone)
        {
            UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod.ON_MINER_BUILT);
            _hasBuiltMinerDrone = true;
        }
    }
}
