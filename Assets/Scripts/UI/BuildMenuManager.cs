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
    [SerializeField] private Button _buildButton;
    [SerializeField] private TextMeshProUGUI _additionalCostText;

    private ResourceStock _stock;
    private DroneManager _droneManager;
    private int _currentDroneIndex = 0;
    private UIStateManager _stateManager;

    private void Start()
    {
        _stock = ResourceStock.GetInstance();
        _droneManager = DroneManager.GetInstance();
        _stateManager = UIStateManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
        _stock.OnResourcesStockChanged += OnResourcesStockChanged;
        gameObject.SetActive(false);
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

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == UIStateManager.GameState.BUILD)
        {
            gameObject.SetActive(true);
            _currentDroneIndex = 0;
            UpdateDroneShownInfo();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnResourcesStockChanged(object sender, Dictionary<ResourceData, int> newstocks)
    {
        UpdateBuildButton();
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChanged -= OnUIStateChanged;
        _stock.OnResourcesStockChanged -= OnResourcesStockChanged;
    }
}
