using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

partial class CustomInputSystem : SystemBase {
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private const string PREFS_INPUT_BINDINGS = "PrefsInputBindings";

    private CustomInputActions _customInputActions;
    
    protected override void OnCreate() {
        _customInputActions = new CustomInputActions();

        if (PlayerPrefs.HasKey(PREFS_INPUT_BINDINGS)) {
            _customInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PREFS_INPUT_BINDINGS));
        }
        
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

    public enum Binding {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause
    }

    public string GetBindingKey(Binding binding) {
        switch (binding) {
            case Binding.MoveUp:
                return _customInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return _customInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return _customInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return _customInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return _customInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return _customInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return _customInputActions.Player.Pause.bindings[0].ToDisplayString();
        }

        return string.Empty;
    }
    public void RebindBinding(Binding binding, Action UIcallback) {
        _customInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;
        
        switch (binding) {
            default:
            case Binding.MoveUp:
                inputAction = _customInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.MoveDown:
                inputAction = _customInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.MoveLeft:
                inputAction = _customInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.MoveRight:
                inputAction = _customInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = _customInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = _customInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _customInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                _customInputActions.Player.Enable();
                UIcallback();

                PlayerPrefs.SetString(PREFS_INPUT_BINDINGS, _customInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }).Start();
    }

    protected override void OnUpdate() {
        float2 currentMoveInput = _customInputActions.Player.Move.ReadValue<Vector2>();
        Entities.ForEach((ref InputMoveComponent input) => {
            input.Value = currentMoveInput;
        }).Schedule();
    }
}