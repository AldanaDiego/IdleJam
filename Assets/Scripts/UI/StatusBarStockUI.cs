using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarStockUI : MonoBehaviour
{
    [SerializeField] private List<ResourceTextUI> _availableResourcesText;

    private void Start()
    {
        ResourceStock stock = ResourceStock.GetInstance();
        stock.OnResourcesStockChanged += OnResourcesStockChanged;
        foreach (ResourceTextUI resourceText in _availableResourcesText)
        {
            int amount = stock.GetResourceStock(resourceText.Resource);
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
        ResourceStock.GetInstance().OnResourcesStockChanged -= OnResourcesStockChanged;
    }
}
