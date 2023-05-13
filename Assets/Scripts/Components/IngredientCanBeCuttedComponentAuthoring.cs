using UnityEngine;
using Unity.Entities;

public class IngredientCanBeCuttedComponentAuthoring : MonoBehaviour {
    class Baker : Baker<IngredientCanBeCuttedComponentAuthoring> {
        public override void Bake(IngredientCanBeCuttedComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<IngredientMustBeCuttedComponent>(entity);
            SetComponentEnabled<IngredientMustBeCuttedComponent>(entity, false);
        }
    }
}

public struct IngredientMustBeCuttedComponent : IComponentData, IEnableableComponent {}