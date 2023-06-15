using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private UIStateManager.GameState _state;
    private UIStateManager _stateManager;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
    }

    public void ChangeGameState()
    {
        _stateManager.ChangeGameState(_state);
    }
}
