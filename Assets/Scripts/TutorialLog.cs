using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLog : Singleton<TutorialLog>
{
    [SerializeField] private TutorialLogEntryDB _tutorialLogEntryDB;

    private List<TutorialLogEntry> _entriesDB;
    private List<TutorialLogEntry> _unlockedEntries;

    protected override void Awake()
    {
        base.Awake();
        _entriesDB = new List<TutorialLogEntry>(_tutorialLogEntryDB.TutorialLogEntries);
        _unlockedEntries = new List<TutorialLogEntry>();
        foreach (TutorialLogEntry entry in _entriesDB.FindAll(entry => entry.UnlockMethod == TutorialLogEntry.TutorialUnlockMethod.DEFAULT))
        {
            _entriesDB.Remove(entry);
            _unlockedEntries.Add(entry);
        }
    }

    public List<TutorialLogEntry> GetUnlockedTutorials()
    {
        return _unlockedEntries;
    }
}
