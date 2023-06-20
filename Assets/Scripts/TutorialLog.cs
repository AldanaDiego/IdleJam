using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLog : Singleton<TutorialLog>
{
    [SerializeField] private TutorialLogEntryDB _tutorialLogEntryDB;
    [SerializeField] private DroneData _cargoDroneData;
    [SerializeField] private DroneData _mutagenDroneData;

    private ExplorationManager _explorationManager;
    private DroneManager _droneManager;
    private List<TutorialLogEntry> _entriesDB;
    private List<TutorialLogEntry> _unlockedEntries;
    private int _explorationsFinished;

    protected override void Awake()
    {
        base.Awake();
        _entriesDB = new List<TutorialLogEntry>(_tutorialLogEntryDB.TutorialLogEntries);
        _unlockedEntries = new List<TutorialLogEntry>();
        UnlockEntriesByMethod(TutorialLogEntry.TutorialUnlockMethod.DEFAULT);
        _explorationsFinished = 0;
    }

    private void Start()
    {
        _explorationManager = ExplorationManager.GetInstance();
        _droneManager = DroneManager.GetInstance();
        _explorationManager.OnExplorationEventsTriggered += OnExplorationEventsTriggered;
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
            ExplorationsFinished = _explorationsFinished
        };
    }

    private void OnExplorationEventsTriggered(object sender, SquadExplorationEvent[] squadEVents)
    {
        _explorationsFinished++;
        if (_explorationsFinished == 1)
        {
            _droneManager.UnlockDrone(_cargoDroneData);
        }

        if (_explorationsFinished == 2)
        {
            _droneManager.UnlockDrone(_mutagenDroneData);
        }
    }
}
