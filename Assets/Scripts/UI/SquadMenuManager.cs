using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquadMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _squadNameText;
    [SerializeField] private Transform _availableDronesList;
    [SerializeField] private Transform _squadDronesList;
    [SerializeField] private AvailableDroneImage _availableDroneImagePrefab;

    private SquadManager _squadManager;
    private DroneManager _droneManager;
    private int _squadCount;
    private int _currentSquadIndex;
    private int _currentSquadSize;
    private Color _transparent;

    private void Start()
    {
        _squadManager = SquadManager.GetInstance();
        _droneManager = DroneManager.GetInstance();
        AvailableDroneImage.OnAvailableDroneClicked += OnAvailableDroneClicked;
        _transparent = Color.white;
        _transparent.a = 0f;
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnShow = OnShow;
        gameObject.SetActive(false);
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
        foreach (Transform child in _availableDronesList)
        {
            Destroy(child.gameObject);
        }
        foreach (Drone drone in _droneManager.GetAvailableDrones())
        {
            AvailableDroneImage image = Instantiate(_availableDroneImagePrefab);
            image.Setup(drone, _availableDronesList);
        }
    }

    private void UpdateSquadMenu()
    {
        _squadNameText.text = "Squad " + _currentSquadIndex.ToString("D2");
        foreach (Transform child in _squadDronesList)
        {
            Destroy(child.gameObject);
        }

        foreach (Drone drone in _squadManager.GetSquad(_currentSquadIndex))
        {
            AvailableDroneImage image = Instantiate(_availableDroneImagePrefab);
            image.Setup(drone, _squadDronesList);
        }
    }

    private void OnShow()
    {
        _squadCount = _squadManager.GetSquadCount();
        _currentSquadIndex = 0;
        UpdateAvailableList();
        UpdateSquadMenu();
    }

    private void OnAvailableDroneClicked(object sender, Drone drone)
    {
        int squadCurrentSize = _squadManager.GetSquad(_currentSquadIndex).Count;
        if (drone.GetSquad() != -1)
        {
            _squadManager.RemoveFromSquad(drone, _currentSquadIndex);
            ((AvailableDroneImage) sender).MoveTo(_availableDronesList);
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
