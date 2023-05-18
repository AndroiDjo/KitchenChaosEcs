using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject {
    public List<IngredientType> ingredients;
    public string recipeName;
}