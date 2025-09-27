using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _resourcePosition = new Vector3(0.7f, 0.5f, -1f);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = _resourcePosition;
        _rigidbody.isKinematic = true;
    }

    public void Throw()
    {
        transform.SetParent(null);
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector2.zero;
    }
}
