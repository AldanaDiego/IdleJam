using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSectionBehaviour : MonoBehaviour
{
    [SerializeField] private UIStateManager.GameState _stateToShow;
    private UIStateManager _stateManager;

    public delegate void OnActionDelegate();
    public OnActionDelegate OnShow;
    public OnActionDelegate OnHide;
    public OnActionDelegate OnCreate;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
        OnCreate?.Invoke(); //Use OnCreate instead of Start on UI with this script to ensure it runs properly before hiding
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == _stateToShow)
        {
            Show();
            OnShow?.Invoke();
        }
        else
        {
            Hide();
            OnHide?.Invoke();
        }
    }
}
