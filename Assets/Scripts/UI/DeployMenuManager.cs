using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployMenuManager : MonoBehaviour
{
    private UIStateManager _stateManager;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate = OnCreate; 
    }

    private void OnCreate()
    {
        _stateManager = UIStateManager.GetInstance();
    }

    public void OnOkClicked()
    {
        _stateManager.ChangeGameState(UIStateManager.GameState.EXPLORING);
    }

    public void OnCancelClicked()
    {
        _stateManager.ChangeGameState(UIStateManager.GameState.IDLE);
    }
}
