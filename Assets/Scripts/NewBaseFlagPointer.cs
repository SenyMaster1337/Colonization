using System;
using UnityEngine;

public class NewBaseFlagPointer : MonoBehaviour
{
    [SerializeField] private PlayerInputController _controller;
    [SerializeField] private Flag _flag;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _baseLayerMask;

    private bool _isBaseClicked = false;
    private Ray _ray;
    private RaycastHit _hit;

    public event Action<Flag> FlagInstalled;

    private void OnEnable()
    {
        _controller.RayShooting += SpawnBasePoint;
    }

    private void OnDisable()
    {
        _controller.RayShooting -= SpawnBasePoint;
    }

    public void SpawnBasePoint(Ray screenPoint)
    {
        _ray = screenPoint;

        if (_isBaseClicked == false)
        {
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _baseLayerMask))
            {
                _isBaseClicked = true;
                Vector3 worldPosition = _hit.point;

                Debug.Log($"Base: {worldPosition}");
                Debug.DrawRay(_ray.origin, _ray.direction * _hit.distance, Color.red, 2f);
            }

            return;
        }

        if (_isBaseClicked)
        {
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _groundLayerMask))
            {
                _isBaseClicked = false;
                Vector3 worldPosition1 = _hit.point;

                Debug.Log($"World position: {worldPosition1}");
                Debug.DrawRay(_ray.origin, _ray.direction * _hit.distance, Color.red, 2f);

                Flag flag = Instantiate(_flag);
                flag.transform.position = worldPosition1;

                FlagInstalled?.Invoke(flag);
            }
        }

    }
}
