using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIStateManager : Singleton<UIStateManager>
{
    public event EventHandler<GameState> OnStateChanged;
    public enum GameState { IDLE, BUILD, SQUAD, ASSIGN, DEPLOY, EXPLORING, CONFIG, JOURNALS, RESOURCES, TUTORIAL, MCJOURNAL, MUTAGEN };

    private GameState _currentGameState = GameState.IDLE;

    public void ChangeGameState(GameState newState)
    {
        _currentGameState = newState;
        OnStateChanged?.Invoke(this, newState);
    }

    public GameState GetGameState()
    {
        return _currentGameState;
    }
}
