using UnityEngine;
using Unity.Entities;

public class CanHoldIngredientComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanHoldIngredientComponentAuthoring> {
        public override void Bake(CanHoldIngredientComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanHoldIngredientComponent>(entity);
        }
    }
}

public struct CanHoldIngredientComponent : IComponentData {}