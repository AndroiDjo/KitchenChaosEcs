using System;
using UnityEngine;
using Unity.Entities;

public class BurgerIngredientsBufferComponentAuthoring : MonoBehaviour {
    class Baker : Baker<BurgerIngredientsBufferComponentAuthoring> {
        public override void Bake(BurgerIngredientsBufferComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddBuffer<BurgerIngredientsBufferComponent>(entity);
        }
    }
}

public struct BurgerIngredientsBufferComponent : IBufferElementData, IComparable<BurgerIngredientsBufferComponent> {
    public IngredientTypeComponent BurgerIngredient;
    public int CompareTo(BurgerIngredientsBufferComponent other) {
        if (this.BurgerIngredient.IngredientType < other.BurgerIngredient.IngredientType) {
            return -1;
        } else if (this.BurgerIngredient.IngredientType == other.BurgerIngredient.IngredientType) {
            return 0;
        }

        return 1;
    }
}
