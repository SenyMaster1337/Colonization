using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CollisionHanlder : MonoBehaviour
{
    private SphereCollider _circleCollider;

    private void Awake()
    {
        _circleCollider = GetComponent<SphereCollider>();

        if (_circleCollider != null)
        {
            _circleCollider.isTrigger = true;
        }
    }
}
