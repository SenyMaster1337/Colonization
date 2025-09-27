using System.Collections;
using UnityEngine;

public class TargetFlipper : MonoBehaviour
{
    public IEnumerator Flip(Transform transform, Vector3 position)
    {
        transform.LookAt(position);
        yield return null;
    }
}
