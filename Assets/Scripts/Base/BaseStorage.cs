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
            EnoughResourcesToUnitSpawned?.Invoke();

            for (int i = 0; i < _newUnitPrice; i++)
            {
                _resources.RemoveAt(i);
            }
        }

        ResourceCountChanged?.Invoke(_resources.Count);
    }
}
