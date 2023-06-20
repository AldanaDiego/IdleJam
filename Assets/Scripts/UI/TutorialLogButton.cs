using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialLogButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    private TutorialLogEntry _entry;

    public void Setup(TutorialLogEntry entry, TutorialMenuManager menu)
    {
        _entry = entry;
        _buttonText.text = _entry.Name;
        _button.onClick.AddListener(() => menu.ShowTutorialContent(_entry));
    }
}
