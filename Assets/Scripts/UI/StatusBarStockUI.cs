using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarStockUI : MonoBehaviour
{
    [SerializeField] private List<ResourceTextUI> _availableResourcesText;

    private ResourceStock _resourceStock;

    private void Start()
    {
        _resourceStock = ResourceStock.GetInstance();
        _resourceStock.OnResourcesStockChanged += OnResourcesStockChanged;
        foreach (ResourceTextUI resourceText in _availableResourcesText)
        {
            int amount = _resourceStock.GetResourceStock(resourceText.Resource);
            resourceText.TextUI.text = $"x {amount}";
        }
    }

    private void OnResourcesStockChanged(object sender, Dictionary<ResourceData, int> newStocks)
    {
        foreach (ResourceTextUI resourceText in _availableResourcesText)
        {
            if (newStocks.TryGetValue(resourceText.Resource, out int amount))
            {
                resourceText.TextUI.text = $"x {amount}";
            }
        }
    }

    private void OnDestroy()
    {
        _resourceStock.OnResourcesStockChanged -= OnResourcesStockChanged;
    }
}
