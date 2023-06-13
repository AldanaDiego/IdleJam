using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SquadMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _squadNameText;
    [SerializeField] private GridLayoutGroup _availableDronesList;
    [SerializeField] private List<Image> _squadMembers;
    [SerializeField] private Image _availableDroneImagePrefab;

    private UIStateManager _stateManager;
    private SquadManager _squadManager;
    private DroneManager _droneManager;
    private int _squadCount;
    private int _currentSquadIndex;
    private int _currentSquadSize;
    private Color _transparent;

    private void Start()
    {
        _stateManager = UIStateManager.GetInstance();
        _squadManager = SquadManager.GetInstance();
        _droneManager = DroneManager.GetInstance();
        _stateManager.OnStateChanged += OnUIStateChanged;
        _transparent = Color.white;
        _transparent.a = 0f;
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
        foreach (Drone drone in _droneManager.GetDrones())
        {
            Image image = Instantiate(_availableDroneImagePrefab);
            image.sprite = drone.GetImage();
            image.transform.SetParent(_availableDronesList.transform);
        }
    }

    private void UpdateSquadMenu()
    {
        _squadNameText.text = "Squad " + _squadCount.ToString("D2");
        _squadMembers[0].sprite = _squadManager.GetSquadLeaderData().Image;

        int i = 1;
        foreach (Drone drone in _squadManager.GetSquad(_currentSquadIndex))
        {
            _squadMembers[i].sprite = drone.GetImage();
            _squadMembers[i].color = Color.white;
            i++;
        }

        while (i <= _squadManager.GetSquadSizeLimit())
        {
            _squadMembers[i].sprite = null;
            _squadMembers[i].color = _transparent;
            i++;
        }
    }

    private void OnUIStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == UIStateManager.GameState.SQUAD)
        {
            gameObject.SetActive(true);
            _squadCount = _squadManager.GetSquadCount();
            _currentSquadIndex = 0;
            UpdateAvailableList();
            UpdateSquadMenu();
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
