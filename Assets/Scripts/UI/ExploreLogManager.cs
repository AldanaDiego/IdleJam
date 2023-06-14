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

    private ExplorationManager _explorationManager;

    public static event EventHandler OnExploreLogFinished;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate = OnCreate;
        menuBehaviour.OnShow = OnShow;
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

    private void OnExplorationEventsTriggered(object sender, List<SquadExplorationEvent> squadEvents)
    {
        foreach (Transform child in _exploreLogList)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(ShowLogMessages(squadEvents));
    }

    private IEnumerator ShowLogMessages(List<SquadExplorationEvent> squadEvents)
    {
        foreach(SquadExplorationEvent squadEvent in squadEvents)
        {
            yield return new WaitForSeconds(1f);
            TextMeshProUGUI log = Instantiate(_logTextPrefab);
            log.text = squadEvent.ToString();
            log.transform.SetParent(_exploreLogList);
        }
        yield return new WaitForSeconds(1f);
        _backButton.interactable = true;
    }

    public void OnReturnButtonClicked()
    {
        OnExploreLogFinished?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        _explorationManager.OnExplorationEventsTriggered -= OnExplorationEventsTriggered;
    }
}
