using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Flag _flag;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _baseLayerMask;

    private bool _isBaseClicked = false;
    private bool _isGroundClicked = false;

    private Ray _ray;
    private RaycastHit _hit;
    private Coroutine _coroutine;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Player.SpawnFlag.performed += OnSpawnFlag;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    public void OnSpawnFlag(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        _ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (_isBaseClicked == false)
        {
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _baseLayerMask))
            {
                _isBaseClicked = true;
                Vector3 worldPosition = _hit.point;

                Debug.Log($"Base: {worldPosition}");
                Debug.DrawRay(_ray.origin, _ray.direction * _hit.distance, Color.red, 2f);
            }

            return;
        }

        if (_isBaseClicked)
        {
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _groundLayerMask))
            {
                _isBaseClicked = false;
                Vector3 worldPosition1 = _hit.point;

                Debug.Log($"World position: {worldPosition1}");
                Debug.DrawRay(_ray.origin, _ray.direction * _hit.distance, Color.red, 2f);

                Flag flag = Instantiate(_flag);
                flag.transform.position = worldPosition1;
            }
        }
    }
}
