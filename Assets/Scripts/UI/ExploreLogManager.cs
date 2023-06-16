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

    public static event EventHandler OnExploreLogFinished;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate = OnCreate;
        menuBehaviour.OnShow = OnShow;
        menuBehaviour.OnHide = OnHide;
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
        foreach(SquadExplorationEvent squadEvent in squadEvents)
        {
            yield return new WaitForSeconds(1f);
            TextMeshProUGUI log = Instantiate(_logTextPrefab, _exploreLogList);
            log.text = squadEvent.ToString();
            _scroll.verticalNormalizedPosition = 0f;
        }
        _scroll.verticalNormalizedPosition = 0f;
        yield return new WaitForSeconds(1f);
        _backButton.interactable = true;
    }

    private void OnDestroy()
    {
        _explorationManager.OnExplorationEventsTriggered -= OnExplorationEventsTriggered;
    }
}
