using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MCJournalMenuManager : MonoBehaviour
{
    [SerializeField] private Transform _journalList;
    [SerializeField] private TextMeshProUGUI _journalContent;
    [SerializeField] private MCJournalButton _journalButtonPrefab;

    private MCJournalLog _journalLog;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate = OnCreate;
        menuBehaviour.OnShow = OnShow;
        menuBehaviour.OnHide = OnHide;
    }

    private void OnCreate()
    {
        _journalLog = MCJournalLog.GetInstance();
    }

    private void OnShow()
    {
        foreach (JournalLogEntry entry in _journalLog.GetUnlockedEntries())
        {
            MCJournalButton button = Instantiate(_journalButtonPrefab, _journalList);
            button.Setup(entry, this);
        }
    }

    private void OnHide()
    {
        foreach (Transform child in _journalList)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowTutorialContent(JournalLogEntry entry)
    {
        _journalContent.text = entry.Content;
    }
}
