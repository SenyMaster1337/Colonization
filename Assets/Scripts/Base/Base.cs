using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourcesScaner _resourceScaner;
    [SerializeField] private ResourceBaseHandler _resourceBaseHandler;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private float _delayBeforeCommand;
    [SerializeField] private GlobalStorage _globalStorage;
    [SerializeField] private BaseStorage _baseStorage;
    [SerializeField] private ResourceCountShower _resourceCountShower;
    [SerializeField] private UnitSpawnPoint _unitSpawnPoint;
    [SerializeField] private ResourcesSpawner _resourcesSpawner;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private NewBaseFlagPointer _newBaseFlagPointer;
    [SerializeField] private BaseCreator _baseCreator;

    private Coroutine _coroutine;
    private bool _isGiveCommandsEnable = true;
    private bool _isCommandToCreateNewBase = false;
    private Flag _flag;

    public bool CanBuildNewBase { get; private set; }

    private void OnEnable()
    {
        _resourceBaseHandler.ResourceBaseDetected += _baseStorage.AddAvailableResource;
        _baseStorage.ResourceCountChanged += _resourceCountShower.ChangeResourceCount;
        _unitSpawner.UnitSpawned += AddUnit;

        if (_resourcesSpawner != null && _globalStorage != null)
        {
            _resourceScaner.ResourceDetected += _globalStorage.AddResourcePosition;
            _resourceBaseHandler.ResourceBaseDetected += _resourcesSpawner.ReleaseObjectToPool;
            _resourceBaseHandler.ResourceBaseDetected += _globalStorage.RemoveResource;
        }

        _newBaseFlagPointer.FlagInstalled += GiveCreateNewBaseCommand;
    }

    private void OnDisable()
    {
        _resourceScaner.ResourceDetected -= _globalStorage.AddResourcePosition;
        _resourceBaseHandler.ResourceBaseDetected -= _globalStorage.RemoveResource;
        _resourceBaseHandler.ResourceBaseDetected -= _baseStorage.AddAvailableResource;
        _baseStorage.ResourceCountChanged -= _resourceCountShower.ChangeResourceCount;
        _resourceBaseHandler.ResourceBaseDetected -= _resourcesSpawner.ReleaseObjectToPool;
        _unitSpawner.UnitSpawned -= AddUnit;

        _newBaseFlagPointer.FlagInstalled -= GiveCreateNewBaseCommand;
    }

    private void Start()
    {
        CanBuildNewBase = false;
    }

    public void Init(ResourcesSpawner resourcesSpawner, Unit unit, GlobalStorage globalStorage, BaseCreator baseCreator)
    {
        _resourcesSpawner = resourcesSpawner;
        _globalStorage = globalStorage;
        _baseCreator = baseCreator;
        AddUnit(unit);
        StartCommandGive();

        _resourceScaner.ResourceDetected += _globalStorage.AddResourcePosition;
        _resourceBaseHandler.ResourceBaseDetected += _resourcesSpawner.ReleaseObjectToPool;
        _resourceBaseHandler.ResourceBaseDetected += _globalStorage.RemoveResource;
    }

    public void PutFlag(Vector3 worldPosition)
    {
        _newBaseFlagPointer.Put(worldPosition);
    }

    public void GiveCreateNewBaseCommand(Flag flag)
    {
        _flag = flag;
        _isCommandToCreateNewBase = true;
    }

    public void GiveSpawnCommand()
    {
        _unitSpawner.SpawnUnit(_unitSpawnPoint.transform.position);
    }

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
        unit.InitBaseCreator(_baseCreator);
    }

    public void GiveCommandUnit()
    {
        bool _isFoundFreeUnit = false;

        for (int i = 0; i < _units.Count && _isFoundFreeUnit == false; i++)
        {
            if (_units.Count > 1)
                CanBuildNewBase = true;
            else
                CanBuildNewBase = false;

            if (_units[i].IsIdle)
            {
                if (_isCommandToCreateNewBase && _baseStorage.ResourcesCount >= 5)
                {
                    _isCommandToCreateNewBase = false;
                    _baseStorage.SpendResourcesToNewBase();
                    _units[i].StartCreateNewBase(_flag, _resourcesSpawner, _globalStorage);
                    _units.RemoveAt(i);
                    _isFoundFreeUnit = true;
                }
                else if (_baseStorage.ResourcesCount >= 3 && _isCommandToCreateNewBase == false)
                {
                    _baseStorage.SpendResourcesToNewUnit();
                    GiveSpawnCommand();
                    _isFoundFreeUnit = true;
                }
                else
                {
                    Resource targetResource = _globalStorage.GiveTargetResource();

                    if (targetResource == null)
                        return;

                    _units[i].StartMission(targetResource);
                    Debug.Log(_units[i].IsIdle);
                    _isFoundFreeUnit = true;
                }

            }
        }
    }

    public void StartCommandGive()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(GiveCommand());
    }

    private IEnumerator GiveCommand()
    {
        var wait = new WaitForSeconds(_delayBeforeCommand);

        while (_isGiveCommandsEnable)
        {
            if (_globalStorage.FreeResourcesCount > 0)
            {
                GiveCommandUnit();
            }

            yield return wait;
        }
    }
}
