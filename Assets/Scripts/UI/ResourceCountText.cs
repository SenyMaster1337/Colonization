using TMPro;
using UnityEngine;

public class ResourceCountText : MonoBehaviour
{
    [SerializeField] private TMP_Text _value;

    public void ChangeValue(int value)
    {
        _value.SetText($"{value}");
    }
}
