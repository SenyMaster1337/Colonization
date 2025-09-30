using System.Collections;
using UnityEngine;

public class ResourcePicker : MonoBehaviour
{
    public void PickUp(Resource resource)
    {
        resource.PickUp(transform);
    }
}
