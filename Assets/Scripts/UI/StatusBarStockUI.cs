using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarStockUI : MonoBehaviour
{
    [SerializeField] private List<ResourceTextUI> _availableResourcesText;

    private void Start()
    {
        ResourceStock stock = ResourceStock.GetInstance();
        foreach (ResourceTextUI resourceText in _availableResourcesText)
        {
            int amount = stock.GetResourceStock(resourceText.Resource);
            resourceText.TextUI.text = $"x {amount}";
        }
    }
}
