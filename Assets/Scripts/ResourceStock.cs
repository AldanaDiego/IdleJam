using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStock : Singleton<ResourceStock>
{
    [SerializeField] private List<ResourceData> _resourceList;
    private Dictionary<ResourceData, int> _stock;

    override protected void Awake()
    {
        base.Awake();
        _stock = new Dictionary<ResourceData, int>();
        foreach (ResourceData resource in _resourceList)
        {
            _stock[resource] = 3; //TODO should be zero but testing xd
        }
    }

    public int GetResourceStock(ResourceData resource)
    {
        return _stock[resource];
    }
}
