using UnityEngine;
using Unity.Entities;

public class CanFryIngredientComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanFryIngredientComponentAuthoring> {
        public override void Bake(CanFryIngredientComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanFryIngredientComponent>(entity);
        }
    }
}

public struct CanFryIngredientComponent : IComponentData {}