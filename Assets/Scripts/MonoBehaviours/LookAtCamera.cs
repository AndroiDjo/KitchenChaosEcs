using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    private void LateUpdate() {
        transform.forward = Camera.main.transform.forward;
    }
}