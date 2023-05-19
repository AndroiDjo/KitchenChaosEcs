using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class IngredientIconsUI : MonoBehaviour {
    [SerializeField] private Transform container;
    [SerializeField] private IngredientIconElement iconElementTemplate;
    [SerializeField] private IngredientsListSO ingredientsList;
    private Dictionary<IngredientType, Sprite> iconsDictionary;

    private void Awake() {
        iconsDictionary = new Dictionary<IngredientType, Sprite>();
        foreach (IngredientSO element in ingredientsList.ingredientsList) {
            iconsDictionary[element.ingredientType] = element.sprite;
        }
        iconElementTemplate.gameObject.SetActive(false);
    }

    public void UpdateIcons(DynamicBuffer<BurgerIngredientsBufferComponent> burgerIngredients) {
        ClearContainer();

        foreach (BurgerIngredientsBufferComponent ingredient in burgerIngredients) {
            SetSpriteByIngredientType(ingredient.BurgerIngredient.IngredientType);
        }
    }
    
    public void UpdateIcons(IngredientType[] ingredientTypes) {
        ClearContainer();

        foreach (IngredientType ingredientType in ingredientTypes) {
            SetSpriteByIngredientType(ingredientType);
        }
    }

    private void SetSpriteByIngredientType(IngredientType ingredientType) {
        if (iconsDictionary.TryGetValue(ingredientType, out Sprite sprite)) {
            var newIcon = Instantiate(iconElementTemplate.gameObject, container);
            newIcon.GetComponent<IngredientIconElement>().SetSprite(sprite);
            newIcon.SetActive(true);
        }
    }
    
    private void ClearContainer() {
        foreach (Transform child in container) {
            if (child != iconElementTemplate.transform) {
                Destroy(child.gameObject);
            }
        }
    }
    
}
