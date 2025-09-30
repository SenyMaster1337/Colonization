using System;
using UnityEngine;

public class NewBaseFlagPointer : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    public event Action<Flag> FlagInstalled;

    private bool _isFlagInstalled = false;
    private Flag _tempFlag;

    public void Put(Vector3 worldPosition1)
    {
        if(_isFlagInstalled)
            _tempFlag.Remove();

        _tempFlag = Instantiate(_flag);
        _tempFlag.transform.position = worldPosition1;
        FlagInstalled?.Invoke(_tempFlag);
        _isFlagInstalled = true;
    }
}
