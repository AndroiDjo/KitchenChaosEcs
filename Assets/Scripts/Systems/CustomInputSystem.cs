using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

partial class CustomInputSystem : SystemBase {
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    
    private CustomInputActions _customInputActions;
    
    protected override void OnCreate() {
        _customInputActions = new CustomInputActions();
        _customInputActions.Player.Enable();
        
        _customInputActions.Player.Interact.performed += InteractOnperformed;
        _customInputActions.Player.InteractAlternate.performed += InteractAlternateOnperformed;
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