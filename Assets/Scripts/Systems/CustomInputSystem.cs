using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

partial class CustomInputSystem : SystemBase {
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    
    private CustomInputActions _customInputActions;
    
    protected override void OnCreate() {
        _customInputActions = new CustomInputActions();
        _customInputActions.Player.Enable();
        
        _customInputActions.Player.Interact.performed += InteractOnperformed;
        _customInputActions.Player.InteractAlternate.performed += InteractAlternateOnperformed;
        _customInputActions.Player.Pause.performed += PauseOnperformed;
    }

    protected override void OnDestroy() {
        _customInputActions.Player.Interact.performed -= InteractOnperformed;
        _customInputActions.Player.InteractAlternate.performed -= InteractAlternateOnperformed;
        _customInputActions.Player.Pause.performed -= PauseOnperformed;
        
        _customInputActions.Dispose();
    }

    private void PauseOnperformed(InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternateOnperformed(InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractOnperformed(InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnUpdate() {
        float2 currentMoveInput = _customInputActions.Player.Move.ReadValue<Vector2>();
        Entities.ForEach((ref InputMoveComponent input) => {
            input.Value = currentMoveInput;
        }).Schedule();
    }
}