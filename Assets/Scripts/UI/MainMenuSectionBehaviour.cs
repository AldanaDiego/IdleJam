using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSectionBehaviour : MonoBehaviour
{
    [SerializeField] private UIStateManager.GameState _stateToShow;
    private UIStateManager _stateManager;

    public delegate void OnShowDelegate();
    public OnShowDelegate OnShow;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
    }

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == _stateToShow)
        {
            gameObject.SetActive(true);
            OnShow?.Invoke();
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
