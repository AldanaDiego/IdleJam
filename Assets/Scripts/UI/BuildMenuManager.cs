using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildMenuManager : MonoBehaviour
{
    [SerializeField] private List<DroneData> _availableDrones;
    [SerializeField] private TextMeshProUGUI _droneNameText;
    [SerializeField] private Image _droneImage;
    [SerializeField] private List<ResourceTextUI> _droneCostUI;
    //[SerializeField] private TextMeshProUGUI _additionalCostText;

    private int _currentDroneIndex = 0;

    private UIStateManager _stateManager;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
        gameObject.SetActive(false);
    }

    public void ShowNextDrone()
    {
        _currentDroneIndex++;
        if (_currentDroneIndex >= _availableDrones.Count)
        {
            _currentDroneIndex = 0;
        }
        UpdateDroneShownInfo(_currentDroneIndex);
    }

    public void ShowPreviousDrone()
    {
        _currentDroneIndex--;
        if (_currentDroneIndex < 0)
        {
            _currentDroneIndex = _availableDrones.Count - 1;
        }
        UpdateDroneShownInfo(_currentDroneIndex);
    }

    private void UpdateDroneShownInfo(int position)
    {
        DroneData currentDrone = _availableDrones[position];
        _droneNameText.text = currentDrone.Name;
        _droneImage.sprite = currentDrone.Image;
        foreach (ResourceTextUI resourceText in _droneCostUI)
        {
            if (currentDrone.Cost.TryGetValue(resourceText.Resource, out int cost))
            {
                resourceText.TextUI.text = $"x {cost}";
            }
            else
            {
                resourceText.TextUI.text = "x 0";
            }
        }
    }

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == UIStateManager.GameState.BUILD)
        {
            gameObject.SetActive(true);
            UpdateDroneShownInfo(0);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
