using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private PlayerInput _playerInput;

    public event Action<Ray> RayShooting;

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

        RayShooting?.Invoke(_mainCamera.ScreenPointToRay(mousePosition));
    }
}
