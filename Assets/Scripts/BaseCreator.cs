using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Base _prefab;
    [SerializeField] private ResourcesSpawner _resourcesSpawner;
    [SerializeField] private NewBaseFlagPointer _newFlagPointer;
    [SerializeField] private UnitSpawner _unitSpawner;

    public void CreateBase(Vector3 pointPosition, Unit unit)
    {
        Base base1 = Instantiate(_prefab);
        base1.Init(_resourcesSpawner, _newFlagPointer, _unitSpawner, unit);
        base1.transform.position = pointPosition;
    }
}
