using System;
using System.Xml.Linq;
using UnityEngine;

public class NewBaseFlagPointer : MonoBehaviour
{
    [SerializeField] private Flag _flag;

    public event Action<Flag> FlagInstalled;

    private bool _isFlagInstalled = false;
    private Flag _tempFlag;

    private void Start()
    {
        _tempFlag = Instantiate(_flag);
    }

    public void Put(Vector3 worldPosition)
    {
        if (_isFlagInstalled)
            _tempFlag.gameObject.SetActive(false);

        _tempFlag.gameObject.SetActive(true);
        _tempFlag.transform.position = worldPosition;
        FlagInstalled?.Invoke(_tempFlag);
        _isFlagInstalled = true;
    }

    public Flag GetFlag()
    {
        return _tempFlag;
    }
}
