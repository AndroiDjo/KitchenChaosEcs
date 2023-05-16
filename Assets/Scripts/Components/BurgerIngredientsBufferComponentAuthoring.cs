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

public struct BurgerIngredientsBufferComponent : IBufferElementData {
    public IngredientTypeComponent BurgerIngredient;
}
