using System;
using UnityEngine;
using Unity.Entities;

public class IngredientTypeComponentAuthoring : MonoBehaviour {
    public IngredientType IngredientType;
    
    class Baker : Baker<IngredientTypeComponentAuthoring> {
        public override void Bake(IngredientTypeComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new IngredientTypeComponent { IngredientType = authoring.IngredientType });
        }
    }
}

public struct IngredientTypeComponent : IComponentData {
    public IngredientType IngredientType;

    public bool IsBurgerIngredient() {
        switch (IngredientType)
        {
            case IngredientType.TomatoSlices:
                return true;
            case IngredientType.CheeseSlices:
                return true;
            case IngredientType.CabbageSlices:
                return true;
            case IngredientType.MeatCooked:
                return true;
            case IngredientType.MeatBurned:
                return true;
            case IngredientType.Bread:
                return true;
            default:
                return false;
        }
    }
}
public enum IngredientType {
    None,
    Tomato,
    TomatoSlices,
    Cheese,
    CheeseSlices,
    Cabbage,
    CabbageSlices,
    Meat,
    MeatCooked,
    MeatBurned,
    Bread,
    Plate
}