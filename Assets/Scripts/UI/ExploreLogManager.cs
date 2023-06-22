using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ExploreLogManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logTextPrefab;
    [SerializeField] private Transform _exploreLogList;
    [SerializeField] private Button _backButton;
    [SerializeField] private ScrollRect _scroll;

    private ExplorationManager _explorationManager;

    private const float LOG_COOLDOWN = 2.5f;

    public event EventHandler<SquadExplorationEvent> OnExploreLogShown;
    public static event EventHandler OnExploreLogFinished;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate += OnCreate;
        menuBehaviour.OnShow += OnShow;
        menuBehaviour.OnHide += OnHide;
    }

    private void OnCreate()
    {
        _explorationManager = ExplorationManager.GetInstance();
        _explorationManager.OnExplorationEventsTriggered += OnExplorationEventsTriggered;
    }

    private void OnShow()
    {
        _backButton.interactable = false;
    }

    private void OnHide()
    {
        OnExploreLogFinished?.Invoke(this, EventArgs.Empty);
        foreach (Transform child in _exploreLogList)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnExplorationEventsTriggered(object sender, SquadExplorationEvent[] squadEvents)
    {
        StartCoroutine(ShowLogMessages(squadEvents));
    }

    private IEnumerator ShowLogMessages(SquadExplorationEvent[] squadEvents)
    {
        TextMeshProUGUI log;
        foreach(SquadExplorationEvent squadEvent in squadEvents)
        {
            if (squadEvent.HasCameraEvent)
            {
                OnExploreLogShown?.Invoke(this, squadEvent);
            }
            yield return new WaitForSeconds(LOG_COOLDOWN);
            log = Instantiate(_logTextPrefab, _exploreLogList);
            log.text = squadEvent.ToString();
            _scroll.verticalNormalizedPosition = 0f;
        }
        
        log = Instantiate(_logTextPrefab, _exploreLogList);
        log.text = ">All squads exploration ended";
        _scroll.verticalNormalizedPosition = 0f;
        yield return new WaitForSeconds(LOG_COOLDOWN);
        _backButton.interactable = true;
    }

    private void OnDestroy()
    {
        _explorationManager.OnExplorationEventsTriggered -= OnExplorationEventsTriggered;
    }
}
