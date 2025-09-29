using System;
using UnityEngine;

public class UnitSpawner : Spawners.Spawner<Unit>
{
    [SerializeField] private BaseCreator _baseCreator;

    private Transform _spawnPoint;

    public event Action<Unit> UnitSpawned;

    public override Unit CreateFunc()
    {
        Unit unit = Instantiate(Prefab);
        unit.Init(_spawnPoint, _baseCreator);

        return unit;
    }

    public override void ChangeParameters(Unit unit)
    {
        unit.transform.position = _spawnPoint.position;
        unit.CreateSpawnPointToBase();
        base.ChangeParameters(unit);
        UnitSpawned?.Invoke(unit);
    }

    public void SpawnUnit(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
        SpawnObject();
    }
}
