using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenuManager : MonoBehaviour
{
    private UIStateManager _stateManager;
    
    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
    }

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        gameObject.SetActive((newState == UIStateManager.GameState.IDLE));
    }
}
