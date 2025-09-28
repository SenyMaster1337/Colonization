using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStorage : MonoBehaviour
{
    private List<Resource> _resources = new List<Resource>();
    private int _newUnitPrice = 3;

    public event Action<int> ResourceCountChanged;
    public event Action EnoughResourcesToUnitSpawned;

    public void AddAvailableResource(Resource resource)
    {
        _resources.Add(resource);

        if (_resources.Count >= _newUnitPrice)
        {
            for (int i = 0; i < _newUnitPrice; i++)
            {
                _resources.RemoveAt(_resources.Count - 1);
            }

            EnoughResourcesToUnitSpawned?.Invoke();
        }

        ResourceCountChanged?.Invoke(_resources.Count);
    }
}
