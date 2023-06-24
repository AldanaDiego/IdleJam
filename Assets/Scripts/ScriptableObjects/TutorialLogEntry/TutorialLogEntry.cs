using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialLogEntryScriptableObject", menuName = "ScriptableObjects/TutorialLogEntry")]
public class TutorialLogEntry : ScriptableObject
{
    public enum TutorialUnlockMethod { DEFAULT, ON_SCANNER_BUILT, ON_MINER_BUILT, ON_CARGO_BUILT, ON_MUTAGEN_BUILT, ON_CHEMICAL_MUTAGENS_UNLOCKED };

    public string Name;
    [TextArea(5,5)] public string Content;
    public TutorialUnlockMethod UnlockMethod;

    public bool Equals(TutorialLogEntry other)
    {
        return Name.Equals(other.Name);
    }
}
