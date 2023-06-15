using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcesMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resourceTextPrefab;
    [SerializeField] private Transform _resourceList;
    private ResourceStock _resourceStock;

    private void Awake()
    {
        MainMenuSectionBehaviour menuBehaviour = GetComponent<MainMenuSectionBehaviour>();
        menuBehaviour.OnCreate = OnCreate;
        menuBehaviour.OnShow = OnShow;
        menuBehaviour.OnHide = OnHide;
    }

    private void OnCreate()
    {
        _resourceStock = ResourceStock.GetInstance();
    }

    private void OnShow()
    {
        int i = 0;
        foreach (var resource in _resourceStock.GetAllStocks())
        {
            if (resource.Value > 0 && !resource.Key.IsBasicResource)
            {
                TextMeshProUGUI text = Instantiate(_resourceTextPrefab, _resourceList);
                text.text = $"{resource.Value} x {resource.Key}";
                i++;
            }
        }
        if (i == 0)
        {
            TextMeshProUGUI text = Instantiate(_resourceTextPrefab, _resourceList);
            text.text = "No additional resources";
        }
    }

    private void OnHide()
    {
        foreach (Transform child in _resourceList)
        {
            Destroy(child.gameObject);
        }
    }
}
