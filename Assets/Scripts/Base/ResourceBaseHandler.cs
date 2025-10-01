using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class ResourceBaseHandler : MonoBehaviour
{
    [SerializeField] private float _radius = 11f;
    [SerializeField] private float _scanInterval = 1f;

    private SphereCollider[] _hitCollidersBuffer = new SphereCollider[50];
    private int _collidersFoundCount;
    private bool _isScanBaseEnable = true;
    private Coroutine _coroutine;

    public event Action<Resource> ResourceBaseDetected;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void Start()
    {
        StartScan();
    }

    private void StartScan()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Scan());
    }

    private IEnumerator Scan()
    {
        var wait = new WaitForSeconds(_scanInterval);

        while(_isScanBaseEnable)
        {
            ScanBase();
            yield return wait;
        }
    }

    private void ScanBase()
    {
        _collidersFoundCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _hitCollidersBuffer);

        if (_collidersFoundCount == 0)
            return;

        for (int i = 0; i < _collidersFoundCount; i++)
        {
            if (_hitCollidersBuffer[i].TryGetComponent(out Resource resource))
            {
                ResourceBaseDetected?.Invoke(resource);
            }
        }
    }
}