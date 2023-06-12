using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T Instance;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = GetComponent<T>();
    }

    public static T GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<T>();
        }
        return Instance;
    }
}