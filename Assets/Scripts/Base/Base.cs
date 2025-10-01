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
    }

    private void OnDisable()
    {
        _resourceScaner.ResourceDetected -= _globalStorage.AddResourcePosition;
        _resourceBaseHandler.ResourceBaseDetected -= _globalStorage.RemoveResource;
        _resourceBaseHandler.ResourceBaseDetected -= _baseStorage.AddAvailableResource;
        _baseStorage.ResourceCountChanged -= _resourceCountShower.ChangeResourceCount;
        _resourceBaseHandler.ResourceBaseDetected -= _resourcesSpawner.ReleaseObjectToPool;
        _unitSpawner.UnitSpawned -= AddUnit;
    }

    private void Start()
    {
        CanBuildNewBase = false;
    }

    public void Init(ResourcesSpawner resourcesSpawner, Unit unit)
    {
        _resourcesSpawner = resourcesSpawner;
        AddUnit(unit);
        StartCommandGive();

        _resourceScaner.ResourceDetected += _globalStorage.AddResourcePosition;
        _resourceBaseHandler.ResourceBaseDetected += _resourcesSpawner.ReleaseObjectToPool;
        _resourceBaseHandler.ResourceBaseDetected += _globalStorage.RemoveResource;
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
    }

    public void GiveCommandUnit()
    {
        bool isFoundFreeUnit = false;

        for (int i = 0; i < _units.Count && isFoundFreeUnit == false; i++)
        {
            if (_units.Count > 1)
                CanBuildNewBase = true;
            else
                CanBuildNewBase = false;

            if (_units[i].IsIdle)
            {
                if (_isCommandToCreateNewBase && _baseStorage.ResourcesCount >= 5)
                {
                    isFoundFreeUnit = true;
                    CreateNewBase(_units[i]);
                }
                else if (_baseStorage.ResourcesCount >= 3 && _isCommandToCreateNewBase == false)
                {
                    isFoundFreeUnit = true;
                    AddNewUnit();
                }
                else
                {
                    isFoundFreeUnit = true;
                    SendUnitToResource(_units[i]);
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

    private void CreateNewBase(Unit unit)
    {
        _isCommandToCreateNewBase = false;
        _baseStorage.SpendResourcesToNewBase();
        unit.StartCreateNewBase(_flag, _resourcesSpawner);
        _units.Remove(unit);
    }

    private void SendUnitToResource(Unit unit)
    {
        Resource targetResource = _globalStorage.GiveTargetResource();

        if (targetResource == null)
            return;

        unit.StartMission(targetResource);
    }

    private void AddNewUnit()
    {
        _baseStorage.SpendResourcesToNewUnit();
        GiveSpawnCommand();
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
