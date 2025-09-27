using System.Collections;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8;

    private float _thresholdValue = 0.1f;

    public IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (transform.position.IsEnoughClose(targetPosition, _thresholdValue) == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
