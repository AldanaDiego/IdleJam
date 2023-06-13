using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIStateManager : Singleton<UIStateManager>
{
    public event EventHandler<GameState> OnStateChanged;
    public enum GameState { IDLE, BUILD, SQUAD, ASSIGN };

    public void ChangeGameState(GameState newState)
    {
        OnStateChanged?.Invoke(this, newState);
    }
}
