using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerInputBuffer : MonoBehaviour {
    private static PlayerInputBuffer _instance;
    private float2 currentMoveInput;

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
    }

    private void Update() {
        float2 inputVector = new float2();
        if (Input.GetKey(KeyCode.W)) {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x = 1;
        }

        currentMoveInput = math.normalizesafe(inputVector);
    }

    public float2 GetMoveInput() {
        return currentMoveInput;
    }
}
