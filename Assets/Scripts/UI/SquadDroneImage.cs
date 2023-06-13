using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SquadDroneImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;
    private Drone _drone;

    public static event EventHandler<Drone> OnSquadDroneClicked;

    public void Setup(Drone drone, Transform parent)
    {
        _drone = drone;
        _image.sprite = drone.GetImage();
        transform.SetParent(parent);
        if (drone.IsSquadLeader())
        {
            _button.interactable = false;
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void OnClicked()
    {
        if (!_drone.IsSquadLeader())
        {
            OnSquadDroneClicked?.Invoke(this, _drone);
        }
    }
}
