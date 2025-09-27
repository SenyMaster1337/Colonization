using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : Spawners.Spawner<Unit>
{
    private Vector3 _spawnPosition;

    public override Unit CreateFunc()
    {
        Unit unit = Instantiate(Prefab);

        return unit;
    }

    public override void ChangeParameters(Unit unit)
    {
        base.ChangeParameters(unit);
    }

    public void SpawnUnit()
    {
        SpawnObject();
    }
}
