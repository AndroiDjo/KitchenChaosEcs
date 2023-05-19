using TMPro;
using UnityEngine;

public class DeliveryManagerElement : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textElement;

    public void SetText(string elementName) {
        textElement.text = elementName;
    }
}