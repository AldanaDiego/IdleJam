using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatusBarStockUI : MonoBehaviour
{
    [SerializeField] private List<ResourceTextUI> _availableResourcesText;

    private ResourceStock _resourceStock;

    private void Start()
    {
        _resourceStock = ResourceStock.GetInstance();
        _resourceStock.OnResourcesStockChanged += OnResourcesStockChanged;
        UpdateStockVisuals();
    }

    private void UpdateStockVisuals()
    {
        foreach (ResourceTextUI resourceText in _availableResourcesText)
        {
            int amount = _resourceStock.GetResourceStock(resourceText.Resource);
            resourceText.TextUI.text = $"x {amount}";
        }
    }

    private void OnResourcesStockChanged(object sender, EventArgs empty)
    {
        UpdateStockVisuals();
    }

    private void OnDestroy()
    {
        _resourceStock.OnResourcesStockChanged -= OnResourcesStockChanged;
    }
}
