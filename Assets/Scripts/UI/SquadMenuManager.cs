using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquadMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _squadNameText;
    [SerializeField] private TextMeshProUGUI _squadAssignedText;
    [SerializeField] private Transform _availableDronesList;
    [SerializeField] private Transform _squadDronesList;
    [SerializeField] private AvailableDroneImage _availableDroneImagePrefab;
    [SerializeField] private Button _leftArrowButton;
    [SerializeField] private Button _rightArrowButton;

    private SquadManager _squadManager;
    private DroneManager _droneManager;
    private int _squadCount;
    private int _currentSquadIndex;
    private int _currentSquadSize;
    private Color _transparent;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnShow = OnShow;
        menuBehaviour.OnCreate = OnCreate;
        menuBehaviour.OnHide = OnHide;
    }

    private void OnCreate()
    {
        _squadManager = SquadManager.GetInstance();
        _droneManager = DroneManager.GetInstance();
        AvailableDroneImage.OnAvailableDroneClicked += OnAvailableDroneClicked;
        _transparent = Color.white;
        _transparent.a = 0f;
    }

    public void ShowNextSquad()
    {
        _currentSquadIndex++;
        if (_currentSquadIndex >= _squadCount)
        {
            _currentSquadIndex = 0;
        }
        UpdateSquadMenu();
    }

    public void ShowPreviousSquad()
    {
        _currentSquadIndex--;
        if (_currentSquadIndex < 0)
        {
            _currentSquadIndex = _squadCount - 1;
        }
        UpdateSquadMenu();
    }

    private void UpdateAvailableList()
    {
        foreach (Drone drone in _droneManager.GetAvailableDrones())
        {
            AvailableDroneImage image = Instantiate(_availableDroneImagePrefab, _availableDronesList);
            image.Setup(drone);
        }
    }

    private void UpdateSquadMenu()
    {
        Squad squad = _squadManager.GetSquad(_currentSquadIndex);
        _squadNameText.text = squad.ToString();
        _squadAssignedText.text = (squad.GetArea() != -1) ? "Assigned to Area " + squad.GetArea().ToString("D2") : "";
        foreach (Transform child in _squadDronesList)
        {
            Destroy(child.gameObject);
        }

        foreach (Drone drone in squad.GetDrones())
        {
            AvailableDroneImage image = Instantiate(_availableDroneImagePrefab, _squadDronesList);
            image.Setup(drone);
        }
    }

    private void OnShow()
    {
        _squadCount = _squadManager.GetSquadCount();
        _currentSquadIndex = 0;
        UpdateAvailableList();
        UpdateSquadMenu();
        _leftArrowButton.interactable = (_squadCount > 1);
        _rightArrowButton.interactable = (_squadCount > 1);
    }

    public void OnHide()
    {
        foreach (Transform child in _availableDronesList)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnAvailableDroneClicked(object sender, Drone drone)
    {
        Squad squad = _squadManager.GetSquad(_currentSquadIndex);
        int squadCurrentSize = squad.GetDroneCount();
        bool isAssigned = squad.IsAssigned();
        if (drone.GetSquad() != -1)
        {
            if (!isAssigned || (isAssigned && squadCurrentSize > 2)) //Assigned squad cannot be empty
            {
                _squadManager.RemoveFromSquad(drone, _currentSquadIndex);
                ((AvailableDroneImage) sender).MoveTo(_availableDronesList);
            }
        }
        else if (squadCurrentSize < _squadManager.GetSquadSizeLimit())
        {
            _squadManager.AddToSquad(drone, _currentSquadIndex);
            ((AvailableDroneImage) sender).MoveTo(_squadDronesList);
        }
    }


    private void OnDestroy()
    {
        AvailableDroneImage.OnAvailableDroneClicked -= OnAvailableDroneClicked;
    }
}
