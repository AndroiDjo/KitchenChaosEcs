using UnityEngine;
using UnityEngine.UI;

public class ProgressBarImageHolder : MonoBehaviour {
    [SerializeField] private Image imageHolder;

    public Image GetImageHolder() {
        return imageHolder;
    }
}