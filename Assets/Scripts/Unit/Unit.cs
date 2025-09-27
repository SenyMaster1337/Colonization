using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //[SerializeField] private UnitSpawnPoint _centerBaseSpawnPoint;
    [SerializeField] private AnimatorParameters _animatorParameters;
    [SerializeField] private Transform _transformRenderer;
    [SerializeField] private TargetFlipper _flipper;
    [SerializeField] private UnitMover _mover;
    [SerializeField] private ResourcePicker _resourcePicker;

    private Coroutine _lifetimeCoroutine;
    private Resource _resourceTarget;

    private Transform _centerBaseSpawnPoint;
    private Vector3 _randomSpawnPoint;
    private float _minRandomOffsetX = -7f;
    private float _maxRandomOffsetX = 6.5f;
    private float _minRandomOffsetZ = -3f;
    private float _maxRandomOffsetZ = 6f;

    public bool IsIdle { get; private set; }

    private void Awake()
    {
        _centerBaseSpawnPoint = GetComponentInParent<UnitSpawnPoint>().transform;
        CreateSpawnPointToBase();
        IsIdle = true;
    }

    public void StartMission(Resource resourceTarget)
    {
        _resourceTarget = resourceTarget;

        if (_lifetimeCoroutine != null)
            StopCoroutine(_lifetimeCoroutine);

        _lifetimeCoroutine = StartCoroutine(PerformMission());
    }

    private void CreateSpawnPointToBase()
    {
        Vector3 centerBaseSpawnPoint = _centerBaseSpawnPoint.transform.position;
        Debug.Log(centerBaseSpawnPoint);
        float randomOffsetX = UnityEngine.Random.Range(_minRandomOffsetX, _maxRandomOffsetX);
        float randomOffsetZ = UnityEngine.Random.Range(_minRandomOffsetZ, _maxRandomOffsetZ);

        _randomSpawnPoint = new Vector3(centerBaseSpawnPoint.x + randomOffsetX, centerBaseSpawnPoint.y, centerBaseSpawnPoint.z + randomOffsetZ);
    }

    private IEnumerator PerformMission()
    {
        yield return RunToResource();

        yield return RunToBase();
    }

    private IEnumerator RunToResource()
    {
        transform.SetParent(null);

        IsIdle = false;

        yield return _flipper.Flip(_transformRenderer, _resourceTarget.transform.position);

        _animatorParameters.PlayRun();

        yield return _mover.MoveToPosition(_resourceTarget.transform.position);

        _resourcePicker.PickUp(_resourceTarget);
    }

    private IEnumerator RunToBase()
    {
        transform.SetParent(_centerBaseSpawnPoint.transform);

        yield return _flipper.Flip(_transformRenderer, _randomSpawnPoint);

        yield return _mover.MoveToPosition(_randomSpawnPoint);

        _resourceTarget.Throw();

        _animatorParameters.StopRun();

        IsIdle = true;
    }
}
