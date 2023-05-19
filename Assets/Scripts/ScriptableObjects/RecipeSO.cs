using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject {
    public List<IngredientSO> ingredients;
    public string recipeName;
}