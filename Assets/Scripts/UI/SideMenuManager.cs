using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuManager : MonoBehaviour
{
    [SerializeField] private Button _squadsButton;
    [SerializeField] private Button _assignButton;

    private UIStateManager _stateManager;
    private SquadManager _squadManager;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _squadManager = SquadManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
        UpdateButtonsInteractable();
    }

    private void UpdateButtonsInteractable()
    {
        _squadsButton.interactable = _squadManager.GetSquadCount() > 0;
        _assignButton.interactable = _squadManager.HasReadySquads();
    }

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == UIStateManager.GameState.IDLE)
        {
            gameObject.SetActive(true);
            UpdateButtonsInteractable();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= OnUIStateChanged;
    }
}
