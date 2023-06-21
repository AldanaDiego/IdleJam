using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MutagenMenuManager : MonoBehaviour
{
    [SerializeField] private Button _squadPreviousButton;
    [SerializeField] private Button _squadNextButton;
    [SerializeField] private Button _mutagenPreviousButton;
    [SerializeField] private Button _mutagenNextButton;
    [SerializeField] private TextMeshProUGUI _squadNameText;
    [SerializeField] private TextMeshProUGUI _squadAssignedText;
    [SerializeField] private TextMeshProUGUI _mutagenNameText;
    [SerializeField] private Image _mutagenImage;
    [SerializeField] private Transform _squadDronesList;
    [SerializeField] private AvailableDroneImage _availableDroneImagePrefab;

    private SquadManager _squadManager;
    private BiomeMutationManager _biomeMutationManager;
    private List<Squad> _squads;
    private List<Mutagen> _mutagens;
    private int _currentSquadIndex;
    private int _currentMutagenIndex;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate = OnCreate;
        menuBehaviour.OnShow = OnShow;
    }

    private void OnCreate()
    {
        _squadManager = SquadManager.GetInstance();
        _biomeMutationManager = BiomeMutationManager.GetInstance();
        _currentSquadIndex = 0;
        _currentMutagenIndex = 0;
    }

    private void OnShow()
    {
        _squads = _squadManager.GetMutagenSquads();
        _mutagens = _biomeMutationManager.GetUnlockedMutagens();
        _squadPreviousButton.interactable = _squads.Count > 1;
        _squadNextButton.interactable = _squads.Count > 1;
        _currentSquadIndex = 0;
        _currentMutagenIndex = 0;
        UpdateSquadMenu();
        UpdateMutagenMenu();
    }

    public void OnPreviousSquadButtonClick()
    {
        _currentSquadIndex--;
        if (_currentSquadIndex < 0)
        {
            _currentSquadIndex = _squads.Count - 1;
        }
        UpdateSquadMenu();
    }

    public void OnNextSquadButtonClick()
    {
        _currentSquadIndex++;
        if (_currentSquadIndex >= _squads.Count)
        {
            _currentSquadIndex = 0;
        }
        UpdateSquadMenu();
    }

    public void OnPreviousMutagenButtonClick()
    {
        _currentMutagenIndex--;
        if (_currentMutagenIndex < 0)
        {
            _currentMutagenIndex = _mutagens.Count - 1;
        }
        UpdateMutagenMenu();
    }

    public void OnNextMutagenButtonClick()
    {
        _currentMutagenIndex++;
        if (_currentMutagenIndex >= _mutagens.Count)
        {
            _currentMutagenIndex = 0;
        }
        UpdateMutagenMenu();
    }

    private void UpdateSquadMenu()
    {
        Squad squad = _squads[_currentSquadIndex];
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

    private void UpdateMutagenMenu()
    {
        Mutagen mutagen = _mutagens[_currentMutagenIndex];
        _mutagenNameText.text = mutagen.Name;
        _mutagenImage.sprite = mutagen.Image;
    }
}
