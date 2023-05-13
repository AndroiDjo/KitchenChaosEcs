using UnityEngine;
using Unity.Entities;

public class IngredientCanBeTriedToCutComponentAuthoring : MonoBehaviour {
    class Baker : Baker<IngredientCanBeTriedToCutComponentAuthoring> {
        public override void Bake(IngredientCanBeTriedToCutComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<TryToCutIngredientComponent>(entity);
            SetComponentEnabled<TryToCutIngredientComponent>(entity, false);
        }
    }
}

public struct TryToCutIngredientComponent : IComponentData, IEnableableComponent {}