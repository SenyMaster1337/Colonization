using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Base _prefab;

    public void CreateBase(Vector3 pointPosition, Unit unit, ResourcesSpawner resourcesSpawner, GlobalStorage globalStorage, BaseCreator baseCreator)
    {
        Base base1 = Instantiate(_prefab);
        base1.Init(resourcesSpawner, unit, globalStorage, baseCreator);
        base1.transform.position = pointPosition;
    }
}