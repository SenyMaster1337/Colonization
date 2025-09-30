using System;
using UnityEngine;

public class UnitSpawner : Spawners.Spawner<Unit>
{
    private Vector3 _spawnPoint;

    public event Action<Unit> UnitSpawned;

    public override Unit CreateFunc()
    {
        Unit unit = Instantiate(Prefab);
        unit.InitSpawnPoint(_spawnPoint);

        return unit;
    }

    public override void ChangeParameters(Unit unit)
    {
        unit.transform.position = _spawnPoint;
        unit.CreateSpawnPointToBase();
        base.ChangeParameters(unit);
        UnitSpawned?.Invoke(unit);
    }

    public void SpawnUnit(Vector3 spawnPoint)
    {
        _spawnPoint = spawnPoint;
        SpawnObject();
    }
}
