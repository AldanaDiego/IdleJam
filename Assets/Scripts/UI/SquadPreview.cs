using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SquadPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _squadNameText;
    [SerializeField] private List<Image> _droneImages;
    [SerializeField] private Button _button;

    private Transform _transform;
    private Squad _squad;
    private Color _transparent;

    public static event EventHandler<Squad> OnSquadPreviewClicked;

    public void Setup(Squad squad)
    {
        _transform = transform;
        _squad = squad;
        _squadNameText.text = squad.ToString();
        _transparent = Color.white;
        _transparent.a = 0f;

        int i = 0;
        foreach (Drone drone in _squad.GetDrones())
        {
            _droneImages[i].sprite = drone.GetImage();
            _droneImages[i].color = Color.white;
            i++;
        }

        while (i < _droneImages.Count)
        {
            _droneImages[i].sprite = null;
            _droneImages[i].color = _transparent;
            i++;
        }

        AudioManager audioManager = AudioManager.GetInstance();
        _button.onClick.AddListener(() => audioManager.PlayButtonClickedAudio());
    }

    public void MoveTo(Transform parent)
    {
        _transform.SetParent(parent);
    }

    public void OnClicked()
    {
        OnSquadPreviewClicked?.Invoke(this, _squad);
    }
}
