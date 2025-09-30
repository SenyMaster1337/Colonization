using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Base _base;

    private void Start()
    {
        _base.GiveSpawnCommand();
        _base.GiveSpawnCommand();
        _base.StartCommandGive();
    }
}
