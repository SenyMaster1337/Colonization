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
    [SerializeField] private Base _base;

    private Coroutine _coroutine;
    private Resource _resourceTarget;
    private Flag _flag;
    private Vector3 _resourceTargetStartPosition;

    private Transform _spawnPoint;
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

    public void Init(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
        transform.SetParent(spawnPoint);
    }

    public void CreateBase()
    {
        Base base1 = Instantiate(_base);
        base1.transform.position = _flag.transform.position;
    }

    public void CreateSpawnPointToBase()
    {
        Vector3 centerBaseSpawnPoint = _spawnPoint.transform.position;
        float randomOffsetX = UnityEngine.Random.Range(_minRandomOffsetX, _maxRandomOffsetX);
        float randomOffsetZ = UnityEngine.Random.Range(_minRandomOffsetZ, _maxRandomOffsetZ);

        _randomSpawnPoint = new Vector3(centerBaseSpawnPoint.x + randomOffsetX, centerBaseSpawnPoint.y, centerBaseSpawnPoint.z + randomOffsetZ);
    }

    public void StartCreateNewBase(Flag flag)
    {
        _flag = flag;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(PerformCreateNewBase());
    }

    private IEnumerator PerformCreateNewBase()
    {
        yield return RunToNewSpawnPointBase();

        CreateBase();
    }

    private IEnumerator RunToNewSpawnPointBase()
    {
        transform.SetParent(null);

        IsIdle = false;

        yield return _flipper.Flip(_transformRenderer, _flag.transform.position);

        _animatorParameters.PlayRun();

        yield return _mover.MoveToPosition(_flag.transform.position);

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
        transform.SetParent(null);

        IsIdle = false;

        yield return _flipper.Flip(_transformRenderer, _resourceTarget.transform.position);

        _animatorParameters.PlayRun();

        yield return _mover.MoveToPosition(_resourceTarget.transform.position);

        if (_resourceTarget.transform.position == _resourceTargetStartPosition)
            _resourcePicker.PickUp(_resourceTarget);
    }

    private IEnumerator RunToBase()
    {
        transform.SetParent(_spawnPoint.transform);

        yield return _flipper.Flip(_transformRenderer, _randomSpawnPoint);

        yield return _mover.MoveToPosition(_randomSpawnPoint);

        _resourceTarget.Throw();

        _animatorParameters.StopRun();

        IsIdle = true;
    }
}
