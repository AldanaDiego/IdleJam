using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreCameraManager : MonoBehaviour
{
    [SerializeField] private Camera _environmentCamera;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate += OnCreate;
        menuBehaviour.OnShow += OnShow;
        menuBehaviour.OnHide += OnHide;
    }

    private void OnCreate()
    {
        //
    }

    private void OnShow()
    {
        _environmentCamera.gameObject.SetActive(true);
    }

    private void OnHide()
    {
        _environmentCamera.gameObject.SetActive(false);
    }
}
