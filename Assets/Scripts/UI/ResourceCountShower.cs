using UnityEngine;

public class ResourceCountShower : MonoBehaviour
{
    [SerializeField] private ResourceCountText _resourceCountText;

    public void ChangeResourceCount(int value)
    {
        _resourceCountText.ChangeValue(value);
    }
}
