using System;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _prefab;

    public event Action<Unit> UnitSpawned;

    public void Spawn(Vector3 spawnPoint)
    {
        Unit unit = Instantiate(_prefab);
        unit.InitSpawnPoint(spawnPoint);
        unit.transform.position = spawnPoint;
        unit.CreateSpawnPointToBase();
        UnitSpawned?.Invoke(unit);
    }
}
