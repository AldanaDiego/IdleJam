using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialLogEntryScriptableObject", menuName = "ScriptableObjects/TutorialLogEntry")]
public class TutorialLogEntry : ScriptableObject
{
    public enum TutorialUnlockMethod { DEFAULT };

    public string Name;
    [TextArea(5,5)] public string Content;
    public TutorialUnlockMethod UnlockMethod;

    public bool Equals(TutorialLogEntry other)
    {
        return Name.Equals(other.Name);
    }
}
