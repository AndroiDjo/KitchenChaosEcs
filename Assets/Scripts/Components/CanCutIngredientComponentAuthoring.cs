using UnityEngine;
using Unity.Entities;

public class CanCutIngredientComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanCutIngredientComponentAuthoring> {
        public override void Bake(CanCutIngredientComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanCutIngredientComponent>(entity);
        }
    }
}

public struct CanCutIngredientComponent : IComponentData {}