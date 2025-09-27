using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourcesScaner _resourceScaner;
    [SerializeField] private ResourceBaseHandler _resourceBaseHandler;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private float _delayBeforeCommand;
    [SerializeField] private GlobalStorage _globalStorage;
    [SerializeField] private BaseStorage _baseStorage;
    [SerializeField] private ResourceCountShower _resourceCountShower;
    [SerializeField] private UnitSpawner _unitSpawner;

    private Coroutine _coroutine;
    private bool _isGiveCommandsEnable = true;

    private void OnEnable()
    {
        _resourceScaner.ResourceDetected += _globalStorage.AddResourcePosition;
        _resourceBaseHandler.ResourceBaseDetected += _globalStorage.RemoveResource;
        _resourceBaseHandler.ResourceBaseDetected += _baseStorage.AddAvailableResource;
        _baseStorage.ResourceCountChanged += _resourceCountShower.ChangeResourceCount;
        _baseStorage.EnoughResourcesToUnitSpawned += _unitSpawner.SpawnUnit;
    }

    private void OnDisable()
    {
        _resourceScaner.ResourceDetected -= _globalStorage.AddResourcePosition;
        _resourceBaseHandler.ResourceBaseDetected -= _globalStorage.RemoveResource;
        _resourceBaseHandler.ResourceBaseDetected -= _baseStorage.AddAvailableResource;
        _baseStorage.ResourceCountChanged -= _resourceCountShower.ChangeResourceCount;
        _baseStorage.EnoughResourcesToUnitSpawned -= _unitSpawner.SpawnUnit;
    }

    private void Start()
    {
        StartCommandGive();
    }

    public void GiveCommandUnit()
    {
        bool _isFoundFreeUnit = false;

        for (int i = 0; i < _units.Count && _isFoundFreeUnit == false; i++)
        {
            if (_units[i].IsIdle)
            {
                Resource targetResource = _globalStorage.GiveTargetResource();

                if (targetResource == null)
                    return;

                _units[i].StartMission(targetResource);
                _isFoundFreeUnit = true;
            }
        }
    }

    private void StartCommandGive()
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
