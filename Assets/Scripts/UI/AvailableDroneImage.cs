using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AvailableDroneImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    [SerializeField] private Image _mutagenImage;
    private Drone _drone;
    private Transform _transform;

    public static event EventHandler<Drone> OnAvailableDroneClicked;

    public void Setup(Drone drone)
    {
        if (drone.IsSquadLeader())
        {
            _button.interactable = false;
        }
        else
        {
            AudioManager audioManager = AudioManager.GetInstance();
            _button.onClick.AddListener(() => audioManager.PlayButtonClickedAudio());
        }
        _drone = drone;
        _transform = transform;
        _image.sprite = drone.GetImage();
    }

    public void Setup(Drone drone, Mutagen mutagen)
    {
        Setup(drone);
        _mutagenImage.color = mutagen.Color;
    }

    public void MoveTo(Transform parent)
    {
        _transform.SetParent(parent);
    }

    public void OnClicked()
    {
        if (!_drone.IsSquadLeader())
        {
            OnAvailableDroneClicked?.Invoke(this, _drone);
        }
    }
}
