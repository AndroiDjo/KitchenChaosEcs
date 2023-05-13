using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class LastInteractedEntityComponentAuthoring : MonoBehaviour {
    class Baker : Baker<LastInteractedEntityComponentAuthoring> {
        public override void Bake(LastInteractedEntityComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<LastInteractedEntityComponent>(entity);
        }
    }
}

public struct LastInteractedEntityComponent : IComponentData {
    public Entity Entity;
    public IngredientEntityComponent Ingredient;
}