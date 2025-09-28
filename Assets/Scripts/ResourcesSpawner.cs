using System.Collections;
using UnityEngine;

public class ResourcesSpawner : Spawners.Spawner<Resource>
{
    [SerializeField] private float _delay;

    private int _minRandomPositionX = 7;
    private int _maxRandomPositionX = 41;
    private int _minRandomPositionZ = 21;
    private int _maxRandomPositionZ = 40;
    private int _positionY = 0;
    private Coroutine _lifetimeCoroutine;
    private bool _isSpawnerEnabled = true;

    private void Start()
    {
        StartResourcesSpawn();
    }

    public override Resource CreateFunc()
    {
        Resource resource = Instantiate(Prefab);

        return resource;
    }

    public override void DestroyObject(Resource resource)
    {
        base.DestroyObject(resource);
    }

    public override void ReleaseObjectToPool(Resource resource)
    {
        resource.Throw();
        base.ReleaseObjectToPool(resource);
    }

    public override void ChangeParameters(Resource resource)
    {
        int randomPositionX = UnityEngine.Random.Range(_minRandomPositionX, _maxRandomPositionX);
        int randomPositionZ = UnityEngine.Random.Range(_minRandomPositionZ, _maxRandomPositionZ);

        resource.transform.position = new Vector3(randomPositionX, _positionY, randomPositionZ);
        base.ChangeParameters(resource);
    }

    private void StartResourcesSpawn()
    {
        if (_lifetimeCoroutine != null)
            StopCoroutine(_lifetimeCoroutine);

        _lifetimeCoroutine = StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnResources()
    {
        var wait = new WaitForSeconds(_delay);

        while (_isSpawnerEnabled)
        {
            yield return wait;
            SpawnObject();
        }
    }
}
