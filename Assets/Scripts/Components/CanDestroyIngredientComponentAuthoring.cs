using UnityEngine;
using Unity.Entities;

public class CanDestroyIngredientComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanDestroyIngredientComponentAuthoring> {
        public override void Bake(CanDestroyIngredientComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanDestroyIngredientComponent>(entity);
        }
    }
}

public struct CanDestroyIngredientComponent : IComponentData {}