using System.Collections;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Unit : MonoBehaviour
{
    [SerializeField] private AnimatorParameters _animatorParameters;
    [SerializeField] private Transform _transformRenderer;
    [SerializeField] private TargetFlipper _flipper;
    [SerializeField] private UnitMover _mover;
    [SerializeField] private ResourcePicker _resourcePicker;

    private BaseCreator _baseCreator;
    private ResourcesSpawner _resourcesSpawner;
    private GlobalStorage _globalStorage;
    private Resource _resourceTarget;
    private Flag _flag;
    private Coroutine _coroutine;

    private Vector3 _spawnPoint;
    private Vector3 _tempSpawnPoint;
    private Vector3 _resourceTargetStartPosition;
    private float _thresholdValue = 1f;

    private Vector3 _randomSpawnPoint;
    private float _minRandomOffsetX = -7f;
    private float _maxRandomOffsetX = 6.5f;
    private float _minRandomOffsetZ = -3f;
    private float _maxRandomOffsetZ = 6f;

    public bool IsIdle { get; private set; }

    private void Awake()
    {
        IsIdle = true;
    }

    public void InitSpawnPoint(Vector3 spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    public void InitBaseCreator(BaseCreator baseCreator)
    {
        _baseCreator = baseCreator;
    }

    public void CreateSpawnPointToBase()
    {
        Vector3 centerBaseSpawnPoint = _spawnPoint;
        float randomOffsetX = UnityEngine.Random.Range(_minRandomOffsetX, _maxRandomOffsetX);
        float randomOffsetZ = UnityEngine.Random.Range(_minRandomOffsetZ, _maxRandomOffsetZ);

        _randomSpawnPoint = new Vector3(centerBaseSpawnPoint.x + randomOffsetX, centerBaseSpawnPoint.y, centerBaseSpawnPoint.z + randomOffsetZ);
    }

    public void StartCreateNewBase(Flag flag, ResourcesSpawner resourcesSpawner, GlobalStorage globalStorage)
    {
        _flag = flag;
        _tempSpawnPoint = flag.transform.position;
        _resourcesSpawner = resourcesSpawner;
        _globalStorage = globalStorage;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(PerformCreateNewBase());
    }

    private IEnumerator ChangeSpawnPoint()
    {
        _spawnPoint = _tempSpawnPoint;
        CreateSpawnPointToBase();

        yield return null;
    }

    private IEnumerator PerformCreateNewBase()
    {
        yield return RunToNewSpawnPointBase();

        _baseCreator.CreateBase(_flag.transform.position, this, _resourcesSpawner, _globalStorage, _baseCreator);

        _animatorParameters.StopRun();

        IsIdle = true;
    }

    private IEnumerator RunToNewSpawnPointBase()
    {
        IsIdle = false;

        yield return _flipper.Flip(_transformRenderer, _flag.transform.position);

        _animatorParameters.PlayRun();

        yield return _mover.MoveToPosition(_flag.transform.position);

        yield return ChangeSpawnPoint();

        _flag.Remove();
    }

    public void StartMission(Resource resourceTarget)
    {
        _resourceTarget = resourceTarget;
        _resourceTargetStartPosition = resourceTarget.transform.position;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(PerformMission());
    }

    private IEnumerator PerformMission()
    {
        yield return RunToResource();

        yield return RunToBase();
    }

    private IEnumerator RunToResource()
    {
        IsIdle = false;

        yield return _flipper.Flip(_transformRenderer, _resourceTarget.transform.position);

        _animatorParameters.PlayRun();

        yield return _mover.MoveToPosition(_resourceTarget.transform.position);

        if (_resourceTarget.transform.position.IsEnoughClose(_resourceTargetStartPosition, _thresholdValue))
            _resourcePicker.PickUp(_resourceTarget);
    }

    private IEnumerator RunToBase()
    {
        yield return _flipper.Flip(_transformRenderer, _randomSpawnPoint);

        yield return _mover.MoveToPosition(_randomSpawnPoint);

        _resourceTarget.Throw();

        _animatorParameters.StopRun();

        IsIdle = true;
    }
}
