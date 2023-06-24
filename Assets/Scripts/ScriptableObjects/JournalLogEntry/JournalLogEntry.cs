using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JournalLogEntryScriptableObject", menuName = "ScriptableObjects/JournalLogEntry")]
public class JournalLogEntry : ScriptableObject
{
    public string Name;
    [TextArea(5,5)] public string Content;
    public int DaysToUnlock;

    public bool Equals(JournalLogEntry other)
    {
        return Name.Equals(other.Name);
    }
}
