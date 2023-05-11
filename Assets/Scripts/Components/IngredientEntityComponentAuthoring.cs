using Unity.Entities;
using UnityEngine;

public class IngredientEntityComponentAuthoring : MonoBehaviour {
    class Baker : Baker<IngredientEntityComponentAuthoring> {
        public override void Bake(IngredientEntityComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<IngredientEntityComponent>(entity);
        }
    }
}
public struct IngredientEntityComponent : IComponentData {
    public Entity Entity;
}