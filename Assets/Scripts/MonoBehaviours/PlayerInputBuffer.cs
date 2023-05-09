using Unity.Mathematics;
using UnityEngine;

public class PlayerInputBuffer : MonoBehaviour {
    private static PlayerInputBuffer _instance;
    private float2 currentMoveInput;
    private CustomInputActions _customInputActions;

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
    }

    private void Update() {
        float2 inputVector = _customInputActions.Player.Move.ReadValue<Vector2>();
        currentMoveInput = math.normalizesafe(inputVector);
    }

    public float2 GetMoveInput() {
        return currentMoveInput;
    }
}
