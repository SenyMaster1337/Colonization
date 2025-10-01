using TMPro;
using UnityEngine;

public class ResourceCountShower : MonoBehaviour
{
    [SerializeField] private BaseStorage _baseStorage;
    [SerializeField] private ResourceCountText _resourceCountText;

    private void OnEnable()
    {
        _baseStorage.ResourceCountChanged += _resourceCountText.ChangeValue;
    }

    private void OnDisable()
    {
        _baseStorage.ResourceCountChanged -= _resourceCountText.ChangeValue;
    }
}
