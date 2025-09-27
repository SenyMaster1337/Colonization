using System;
using UnityEngine;

public class ResourcesScaner : CollisionHanlder
{
    public event Action<Resource> ResourceDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Resource resource))
        {
            ResourceDetected?.Invoke(resource);
        }
    }
}
