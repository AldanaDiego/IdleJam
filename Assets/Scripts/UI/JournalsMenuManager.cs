using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalsMenuManager : MonoBehaviour
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

    public void OnResourcesButtonClicked()
    {
        _stateManager.ChangeGameState(UIStateManager.GameState.RESOURCES);
    }
}
