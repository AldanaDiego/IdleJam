using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuManager : MonoBehaviour
{
    private UIStateManager _stateManager;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
        gameObject.SetActive(false);
    }

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        gameObject.SetActive((newState == UIStateManager.GameState.BUILD));
    }
}
