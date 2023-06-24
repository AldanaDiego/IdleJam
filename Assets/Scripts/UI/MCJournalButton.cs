using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MCJournalButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    private JournalLogEntry _entry;

    public void Setup(JournalLogEntry entry, MCJournalMenuManager menu)
    {
        _entry = entry;
        _buttonText.text = _entry.Name;
        _button.onClick.AddListener(() => menu.ShowTutorialContent(_entry));
    }
}
