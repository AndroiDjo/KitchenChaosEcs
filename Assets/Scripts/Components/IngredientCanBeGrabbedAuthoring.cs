using UnityEngine;
using Unity.Entities;

public class IngredientCanBeGrabbedAuthoring : MonoBehaviour {
    class Baker : Baker<IngredientCanBeGrabbedAuthoring> {
        public override void Bake(IngredientCanBeGrabbedAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<IngredientMustBeGrabbedComponent>(entity);
            SetComponentEnabled<IngredientMustBeGrabbedComponent>(entity, false);
            AddComponent<IngredientMustBeReleaseComponent>(entity);
            SetComponentEnabled<IngredientMustBeReleaseComponent>(entity, false);
        }
    }
}

public struct IngredientMustBeGrabbedComponent : IComponentData, IEnableableComponent {}
public struct IngredientMustBeReleaseComponent : IComponentData, IEnableableComponent {}