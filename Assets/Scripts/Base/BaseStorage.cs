using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    private List<Resource> _resources = new List<Resource>();
    private int _newUnitPrice = 3;
    private int _newBasePrice = 5;

    public event Action<int> ResourceCountChanged;

    public int ResourcesCount => _resources.Count;

    public void AddAvailableResource(Resource resource)
    {
        _resources.Add(resource);
        ResourceCountChanged?.Invoke(_resources.Count);
    }

    public void SpendResourcesToNewUnit()
    {
        SpendResources(_newUnitPrice);
        ResourceCountChanged?.Invoke(_resources.Count);
    }

    public void SpendResourcesToNewBase()
    {
        SpendResources(_newBasePrice);
        ResourceCountChanged?.Invoke(_resources.Count);
    }

    private void SpendResources(int price)
    {
        for (int i = 0; i < price; i++)
        {
            _resources.RemoveAt(_resources.Count - 1);
        }
    }
}
