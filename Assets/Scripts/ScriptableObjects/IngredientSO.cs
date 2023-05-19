using System;
using UnityEngine;

[CreateAssetMenu()]
public class IngredientSO : ScriptableObject, IComparable<IngredientSO> {
    public IngredientType ingredientType;
    public Sprite sprite;
    public string ingredientName;

    public int CompareTo(IngredientSO other) {
        if (this.ingredientType < other.ingredientType) {
            return -1;
        } else if (this.ingredientType == other.ingredientType) {
            return 0;
        }

        return 1;
    }
}
