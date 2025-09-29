using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

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
    [SerializeField] private NewBaseFlagPointer _newBaseFlagPointer;
    [SerializeField] private UnitSpawner _unitSpawner;

    private Coroutine _coroutine;
    private bool _isGiveCommandsEnable = true;
    private bool _isCommandToCreateNewBase = false;
    private Flag _flag;

    private void OnEnable()
    {
        _resourceScaner.ResourceDetected += _globalStorage.AddResourcePosition;
        _resourceBaseHandler.ResourceBaseDetected += _globalStorage.RemoveResource;
        _resourceBaseHandler.ResourceBaseDetected += _baseStorage.AddAvailableResource;
        _baseStorage.ResourceCountChanged += _resourceCountShower.ChangeResourceCount;
        _baseStorage.EnoughResourcesToUnitSpawned += GiveSpawnCommand;

        if (_resourcesSpawner != null && _unitSpawner != null && _newBaseFlagPointer != null)
        {
            _resourceBaseHandler.ResourceBaseDetected += _resourcesSpawner.ReleaseObjectToPool;
            _unitSpawner.UnitSpawned += AddUnit;
            _newBaseFlagPointer.FlagInstalled += GiveCreateNewBaseCommand;
        }
    }

    private void OnDisable()
    {
        _resourceScaner.ResourceDetected -= _globalStorage.AddResourcePosition;
        _resourceBaseHandler.ResourceBaseDetected -= _globalStorage.RemoveResource;
        _resourceBaseHandler.ResourceBaseDetected -= _baseStorage.AddAvailableResource;
        _baseStorage.ResourceCountChanged -= _resourceCountShower.ChangeResourceCount;
        _baseStorage.EnoughResourcesToUnitSpawned -= GiveSpawnCommand;
        _resourceBaseHandler.ResourceBaseDetected += _resourcesSpawner.ReleaseObjectToPool;
        _unitSpawner.UnitSpawned -= AddUnit;
        _newBaseFlagPointer.FlagInstalled -= GiveCreateNewBaseCommand;
    }

    public void Init(ResourcesSpawner resourcesSpawner, NewBaseFlagPointer newBaseFlagPointer, UnitSpawner unitSpawner, Unit unit)
    {
        _resourcesSpawner = resourcesSpawner;
        _newBaseFlagPointer = newBaseFlagPointer;
        _unitSpawner = unitSpawner;

        _resourceBaseHandler.ResourceBaseDetected += _resourcesSpawner.ReleaseObjectToPool;
        _unitSpawner.UnitSpawned += AddUnit;
        _newBaseFlagPointer.FlagInstalled += GiveCreateNewBaseCommand;

        AddUnit(unit);
        StartCommandGive();
    }

    public void GiveCreateNewBaseCommand(Flag flag)
    {
        _flag = flag;
        _isCommandToCreateNewBase = true;
    }

    public void GiveSpawnCommand()
    {
        _unitSpawner.SpawnUnit(_unitSpawnPoint.transform);
    }

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }

    public void GiveCommandUnit()
    {
        bool _isFoundFreeUnit = false;

        for (int i = 0; i < _units.Count && _isFoundFreeUnit == false; i++)
        {
            if (_units[i].IsIdle)
            {
                if (_isCommandToCreateNewBase == true)
                {
                    _units[i].StartCreateNewBase(_flag);
                    _units.RemoveAt(i);
                    _isCommandToCreateNewBase = false;
                }
                else
                {
                    Resource targetResource = _globalStorage.GiveTargetResource();

                    if (targetResource == null)
                        return;

                    _units[i].StartMission(targetResource);
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
