using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    private UIStateManager _stateManager;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
        _button.interactable = false;
    }

    public void ChangeGameState()
    {
        _stateManager.ChangeGameState(UIStateManager.GameState.IDLE);
    }

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        _button.interactable = (newState != UIStateManager.GameState.IDLE) && (newState != UIStateManager.GameState.EXPLORING);
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= OnUIStateChanged;
    }
}
