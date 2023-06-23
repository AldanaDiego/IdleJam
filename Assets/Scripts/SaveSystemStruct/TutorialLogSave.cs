using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TutorialLogSave
{
    public List<TutorialLogEntry> EntriesDB;
    public List<TutorialLogEntry> UnlockedEntries;
    public int ExplorationsFinished;
    public bool HasBuiltScannerDrone;
    public bool HasBuiltMinerDrone;
    public bool HasBuiltCargoDrone;
    public bool HasBuiltMutagenDrone;
    public int ExplorationsSinceMutagenUnlocked;
}
