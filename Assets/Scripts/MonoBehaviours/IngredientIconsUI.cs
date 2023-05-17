using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class IngredientIconsUI : MonoBehaviour {
    [SerializeField] private Canvas canvas;
    [SerializeField] private IngredientIconElement iconElementTemplate;
    [SerializeField] private List<IngredientIcon> ingredientIcons;
    private Dictionary<IngredientType, Sprite> iconsDictionary;

    [Serializable]
    public struct IngredientIcon {
        public IngredientType IngredientType;
        public Sprite Sprite;
    }

    private void Awake() {
        iconsDictionary = new Dictionary<IngredientType, Sprite>();
        foreach (IngredientIcon element in ingredientIcons) {
            iconsDictionary[element.IngredientType] = element.Sprite;
        }
    }

    public void UpdateIcons(DynamicBuffer<BurgerIngredientsBufferComponent> burgerIngredients) {
        foreach (Transform child in canvas.transform) {
            Destroy(child.gameObject);
        }

        foreach (BurgerIngredientsBufferComponent ingredient in burgerIngredients) {
            if (iconsDictionary.TryGetValue(ingredient.BurgerIngredient.IngredientType, out Sprite sprite)) {
                var newIcon = Instantiate(iconElementTemplate.gameObject, canvas.transform);
                newIcon.GetComponent<IngredientIconElement>().SetSprite(sprite);
            }
        }
    }
    
}
