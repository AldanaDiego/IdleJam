using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCJournalLog : Singleton<MCJournalLog>
{
    [SerializeField] private JournalLogDB _journalDB;

    private ExplorationManager _explorationManager;
    private List<JournalLogEntry> _unlockedEntries;
    private int _explorationsFinished;

    protected override void Awake()
    {
        base.Awake();
        _unlockedEntries = new List<JournalLogEntry>();
        _explorationsFinished = 0;
        UnlockEntriesByDays();
    }

    private void Start()
    {
        _explorationManager = ExplorationManager.GetInstance();
        _explorationManager.OnExplorationEventsTriggered += OnExplorationEventsTriggered;
    }

    public void SetupFromSave(List<JournalLogEntry> unlockedEntries, int explorationsFinished)
    {
        _unlockedEntries = unlockedEntries;
        _explorationsFinished = explorationsFinished;
    }

    public List<JournalLogEntry> GetUnlockedEntries()
    {
        return _unlockedEntries;
    }

    public int GetExplorationsFinished()
    {
        return _explorationsFinished;
    }

    private void UnlockEntriesByDays()
    {
        foreach (JournalLogEntry entry in _journalDB.JournalLogEntries)
        {
            if (entry.DaysToUnlock == _explorationsFinished)
            {
                _unlockedEntries.Add(entry);
            }
        }
    }

    private void OnExplorationEventsTriggered(object sender, SquadExplorationEvent[] squadEVents)
    {
        _explorationsFinished++;
        UnlockEntriesByDays();
    }

    private void OnDestroy()
    {
        _explorationManager.OnExplorationEventsTriggered -= OnExplorationEventsTriggered;
    }
}
