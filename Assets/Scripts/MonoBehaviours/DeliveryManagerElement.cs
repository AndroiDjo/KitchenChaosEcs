using TMPro;
using UnityEngine;

public class DeliveryManagerElement : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textElement;
    [SerializeField] private IngredientIconsUI iconsUI;

    public void SetText(string elementName) {
        textElement.text = elementName;
    }

    public void SetIcons(IngredientType[] ingredientTypes) {
        iconsUI.UpdateIcons(ingredientTypes);
    }
}