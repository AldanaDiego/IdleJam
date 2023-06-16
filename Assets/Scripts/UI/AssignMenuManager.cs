using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssignMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _areaName;
    [SerializeField] private TextMeshProUGUI _areaTitle;
    [SerializeField] private TextMeshProUGUI _areaBiome;
    [SerializeField] private TextMeshProUGUI _areaBiomeDetails;
    [SerializeField] private SquadPreview _squadPreviewPrefab;
    [SerializeField] private Transform _availableSquadsList;
    [SerializeField] private Transform _areaAssignedSquadList;
    [SerializeField] private Button _leftArrowButton;
    [SerializeField] private Button _rightArrowButton;

    private AreaManager _areaManager;
    private SquadManager _squadManager;
    private int _currentAreaIndex;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnShow = OnShow;
        menuBehaviour.OnCreate = OnCreate;
    }

    private void OnCreate()
    {
        _areaManager = AreaManager.GetInstance();
        _squadManager = SquadManager.GetInstance();
        SquadPreview.OnSquadPreviewClicked += OnSquadPreviewClicked;
    }

    public void ShowNextArea()
    {
        _currentAreaIndex++;
        if (_currentAreaIndex >= _areaManager.GetAreaCount())
        {
            _currentAreaIndex = 0;
        }
        UpdateAreaInfo();
    }

    public void ShowPreviousArea()
    {
        _currentAreaIndex--;
        if (_currentAreaIndex < 0)
        {
            _currentAreaIndex = _areaManager.GetAreaCount() - 1;
        }
        UpdateAreaInfo();
    }

    private void UpdateAreaInfo()
    {
        Area area = _areaManager.GetArea(_currentAreaIndex);
        _areaName.text = "Area " + _currentAreaIndex.ToString("D2");
        _areaTitle.text = "Area " + _currentAreaIndex.ToString("D2") + "\nSquads";
        _areaBiome.text = $"Biome: {area.GetBiomeName()}";
        _areaBiomeDetails.text = area.GetBiomeDetails();

        foreach (Transform child in _areaAssignedSquadList)
        {
            Destroy(child.gameObject);
        }
        List<Squad> areaSquads = area.GetSquads();
        foreach (Squad squad in areaSquads)
        {
            SquadPreview preview = Instantiate(_squadPreviewPrefab, _areaAssignedSquadList);
            preview.Setup(squad);
        }
    }

    private void UpdateAvailableSquads()
    {
        foreach (Transform child in _availableSquadsList)
        {
            Destroy(child.gameObject);
        }
        List<Squad> squads = _squadManager.GetUnassignedSquads();
        foreach (Squad squad in squads)
        {
            SquadPreview preview = Instantiate(_squadPreviewPrefab, _availableSquadsList);
            preview.Setup(squad);
        }
    }

    private void OnShow()
    {
        _currentAreaIndex = 0;
        int count = _areaManager.GetAreaCount();
        UpdateAreaInfo();
        UpdateAvailableSquads();
        _leftArrowButton.interactable = (count > 1);
        _rightArrowButton.interactable = (count > 1);
    }

    private void OnSquadPreviewClicked(object sender, Squad squad)
    {
        if (squad.GetArea() == -1)
        {
            _areaManager.GetArea(_currentAreaIndex).AddSquad(squad);
            ((SquadPreview)sender).MoveTo(_areaAssignedSquadList);
        }
        else
        {
            _areaManager.GetArea(_currentAreaIndex).RemoveSquad(squad);
            ((SquadPreview)sender).MoveTo(_availableSquadsList);
        }
    }

    private void OnDestroy()
    {
        SquadPreview.OnSquadPreviewClicked -= OnSquadPreviewClicked;
    }
}
