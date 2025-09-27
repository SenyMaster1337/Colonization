using System;
using UnityEngine;

public class ResourceBaseHandler : CollisionHanlder
{
    public event Action<Resource> ResourceBaseDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Resource resource))
        {
            ResourceBaseDetected?.Invoke(resource);
        }
    }
}