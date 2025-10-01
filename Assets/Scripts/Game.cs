using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Base _base;

    private void Start()
    {
        _base.SpawnUnit();
        _base.SpawnUnit();
        _base.StartCommandGive();
    }
}
