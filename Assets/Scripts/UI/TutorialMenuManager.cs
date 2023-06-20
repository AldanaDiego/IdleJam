using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialMenuManager : MonoBehaviour
{
    [SerializeField] private Transform _tutorialList;
    [SerializeField] private TextMeshProUGUI _tutorialContent;
    [SerializeField] private TutorialLogButton _tutorialButtonPrefab;

    private TutorialLog _tutorialLog;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate = OnCreate;
        menuBehaviour.OnShow = OnShow;
        menuBehaviour.OnHide = OnHide;
    }

    private void OnCreate()
    {
        _tutorialLog = TutorialLog.GetInstance();
    }

    private void OnShow()
    {
        foreach (TutorialLogEntry entry in _tutorialLog.GetUnlockedTutorials())
        {
            TutorialLogButton button = Instantiate(_tutorialButtonPrefab, _tutorialList);
            button.Setup(entry, this);
        }
    }

    private void OnHide()
    {
        foreach (Transform child in _tutorialList)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowTutorialContent(TutorialLogEntry entry)
    {
        _tutorialContent.text = entry.Content;
    }
}
