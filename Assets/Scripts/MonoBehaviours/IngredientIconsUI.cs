using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class IngredientIconsUI : MonoBehaviour {
    [SerializeField] private Canvas canvas;
    [SerializeField] private IngredientIconElement iconElementTemplate;
    [SerializeField] private IngredientsListSO ingredientsList;
    private Dictionary<IngredientType, Sprite> iconsDictionary;

    private void Awake() {
        iconsDictionary = new Dictionary<IngredientType, Sprite>();
        foreach (IngredientSO element in ingredientsList.ingredientsList) {
            iconsDictionary[element.ingredientType] = element.sprite;
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
