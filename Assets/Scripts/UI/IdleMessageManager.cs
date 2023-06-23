using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IdleMessageManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    private TutorialLog _tutorialLog;
    private UIStateManager _stateManager;
    private string _nextMessage;

    private void Start()
    {
        _nextMessage = null;
        _tutorialLog = TutorialLog.GetInstance();
        _stateManager = UIStateManager.GetInstance();
        _tutorialLog.OnFeatureUnlocked += OnFeatureUnlocked;
        _stateManager.OnStateChanged += OnStateChanged;
        gameObject.SetActive(false);
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void OnFeatureUnlocked(object sender, string message)
    {
        _nextMessage = message;
    }

    private void OnStateChanged(object sender, UIStateManager.GameState newState)
    {
        if (newState == UIStateManager.GameState.IDLE && _nextMessage != null)
        {
            gameObject.SetActive(true);
            _messageText.text = _nextMessage;
            _nextMessage = null;
            StartCoroutine(Hide());
        }
    }
}
