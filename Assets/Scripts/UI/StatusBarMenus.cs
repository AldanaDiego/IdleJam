using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarMenus : MonoBehaviour
{
    [SerializeField] private Button _configButton;
    [SerializeField] private Button _journalButton;
    
    private UIStateManager _stateManager;
    
    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(object sender, UIStateManager.GameState newState)
    {
        _configButton.interactable = (newState == UIStateManager.GameState.IDLE);
        _journalButton.interactable = (newState == UIStateManager.GameState.IDLE);
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged += OnStateChanged;
    }
}
