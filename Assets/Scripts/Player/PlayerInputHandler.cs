using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool isThirdPerson = true;

    PlayerInputActions playerInputActions;

    public event Action OnCameraSwitchAction;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Move.performed += Move_performed;
        playerInputActions.Player.Move.canceled += Move_performed;
        playerInputActions.Player.MouseDelta.performed += MouseDelta_performed;
        playerInputActions.Player.MouseDelta.canceled += MouseDelta_performed;
        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Sprint.performed += Sprint_performed;
        playerInputActions.Player.CameraSwitch.performed += CameraSwitch_performed;

    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraSwitch_performed(InputAction.CallbackContext callback)
    {
        OnCameraSwitchAction?.Invoke();
    }

    private void Sprint_performed(InputAction.CallbackContext callback)
    {
        sprint = callback.ReadValueAsButton();
    }

    private void MouseDelta_performed(InputAction.CallbackContext callback)
    {
        look = callback.ReadValue<Vector2>();
        EventBus.Publish(new OnMouseDeltaEvent(look));
    }

    private void Jump_performed(InputAction.CallbackContext callback)
    {
        jump = callback.ReadValueAsButton();
    }

    private void Move_performed(InputAction.CallbackContext callback)
    {
        move = callback.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Move.performed -= Move_performed;
        playerInputActions.Player.MouseDelta.performed -= MouseDelta_performed;
        playerInputActions.Player.Jump.performed -= Jump_performed;
        playerInputActions.Player.MouseDelta.canceled -= MouseDelta_performed;
        playerInputActions.Player.Move.canceled -= Move_performed;
    }
}
