using System.Collections.Generic;
using UnityEngine;

public class GlobalStorage : MonoBehaviour
{
    private List<Resource> _resources = new List<Resource>();
    private List<Resource> _resourcesBusy = new List<Resource>();
    private int _firstResourceIndex = 0;

    private float _lastCallTime;
    private float _minInterval = 1f;

    public int FreeResourcesCount => _resources.Count;

    public void AddResourcePosition(Resource resource)
    {
        if (_resourcesBusy.Contains(resource) == false)
            _resources.Add(resource);
    }

    public Resource GiveTargetResource()
    {
        if (Time.time - _lastCallTime >= _minInterval)
        {
            _lastCallTime = Time.time;

            if (_resourcesBusy.Contains(_resources[_firstResourceIndex]) == false)
            {
                _resourcesBusy.Add(_resources[_firstResourceIndex]);
                _resources.Remove(_resources[_firstResourceIndex]);
                return _resourcesBusy[_resourcesBusy.Count - 1];
            }
            else
            {
                return null;
            }

        }

        return null;
    }

    public void RemoveResource(Resource resource)
    {
        _resourcesBusy.Remove(resource);
    }
}
