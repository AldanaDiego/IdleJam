using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AvailableDroneImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    private Drone _drone;

    public static event EventHandler<Drone> OnAvailableDroneClicked;

    public void Setup(Drone drone, Transform parent)
    {
        _drone = drone;
        _image.sprite = drone.GetImage();
        transform.SetParent(parent);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void OnClicked()
    {
        OnAvailableDroneClicked?.Invoke(this, _drone);
    }
}
