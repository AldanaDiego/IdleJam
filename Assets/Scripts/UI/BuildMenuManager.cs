using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuildMenuManager : MonoBehaviour
{
    [SerializeField] private DroneDB _droneDB;
    [SerializeField] private TextMeshProUGUI _droneNameText;
    [SerializeField] private Image _droneImage;
    [SerializeField] private List<ResourceTextUI> _droneCostUI;
    [SerializeField] private Button _buildButton;
    [SerializeField] private TextMeshProUGUI _additionalCostText;

    private ResourceStock _stock;
    private DroneManager _droneManager;
    private List<DroneData> _availableDrones;
    private int _currentDroneIndex = 0;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnShow = OnShow;
        menuBehaviour.OnCreate = OnCreate;
    }

    private void OnCreate()
    {
        _stock = ResourceStock.GetInstance();
        _droneManager = DroneManager.GetInstance();
        _stock.OnResourcesStockChanged += OnResourcesStockChanged;
        _availableDrones = _droneDB.Drones;
    }

    public void ShowNextDrone()
    {
        _currentDroneIndex++;
        if (_currentDroneIndex >= _availableDrones.Count)
        {
            _currentDroneIndex = 0;
        }
        UpdateDroneShownInfo();
    }

    public void ShowPreviousDrone()
    {
        _currentDroneIndex--;
        if (_currentDroneIndex < 0)
        {
            _currentDroneIndex = _availableDrones.Count - 1;
        }
        UpdateDroneShownInfo();
    }

    public void BuildDrone()
    {
        _buildButton.interactable = false;
        _droneManager.BuildDrone(_availableDrones[_currentDroneIndex]);
    }

    private void UpdateDroneShownInfo()
    {
        DroneData currentDrone = _availableDrones[_currentDroneIndex];
        _droneNameText.text = currentDrone.Name;
        _droneImage.sprite = currentDrone.Image;
        UpdateBuildButton();
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

        _additionalCostText.text = "None";
        foreach (var cost in currentDrone.Cost)
        {
            if (!cost.Key.IsBasicResource)
            {
                _additionalCostText.text = $"{cost.Value} x {cost.Key.Name}";
                break;
            }
        }
    }

    private void UpdateBuildButton()
    {
        _buildButton.interactable = _stock.CanBuildDrone(_availableDrones[_currentDroneIndex]);
    }

    private void OnShow()
    {
        _currentDroneIndex = 0;
        UpdateDroneShownInfo();
    }

    private void OnResourcesStockChanged(object sender, EventArgs empty)
    {
        UpdateBuildButton();
    }

    private void OnDestroy()
    {
        _stock.OnResourcesStockChanged -= OnResourcesStockChanged;
    }
}
