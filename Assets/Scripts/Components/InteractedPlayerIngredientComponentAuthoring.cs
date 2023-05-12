using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class InteractedPlayerIngredientComponentAuthoring : MonoBehaviour {
    class Baker : Baker<InteractedPlayerIngredientComponentAuthoring> {
        public override void Bake(InteractedPlayerIngredientComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<InteractedPlayerIngredientComponent>(entity);
        }
    }
}

public struct InteractedPlayerIngredientComponent : IComponentData {
    public IngredientEntityComponent Ingredient;
}