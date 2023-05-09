using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBuffer : MonoBehaviour {
    private static PlayerInputBuffer _instance;
    private float2 currentMoveInput;
    private CustomInputActions _customInputActions;

    public event EventHandler OnInteractAction;

    public static PlayerInputBuffer Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PlayerInputBuffer>();
            }

            return _instance;
        }
    }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        }
        else {
            _instance = this;
        }

        _customInputActions = new CustomInputActions();
        _customInputActions.Player.Enable();
        
        _customInputActions.Player.Interact.performed += InteractOnperformed;
    }

    private void OnDestroy() {
        _customInputActions.Player.Interact.performed -= InteractOnperformed;
        _customInputActions.Player.Disable();
    }

    private void InteractOnperformed(InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        currentMoveInput = _customInputActions.Player.Move.ReadValue<Vector2>();
    }

    public float2 GetMoveInput() {
        return currentMoveInput;
    }
}
