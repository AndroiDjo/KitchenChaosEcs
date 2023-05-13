using UnityEngine;
using Unity.Entities;

public class CanGrabIngredientComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanGrabIngredientComponentAuthoring> {
        public override void Bake(CanGrabIngredientComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanGrabIngredientComponent>(entity);
            AddComponent<MustGrabIngredientComponent>(entity);
            SetComponentEnabled<MustGrabIngredientComponent>(entity, false);
        }
    }
}

public struct CanGrabIngredientComponent : IComponentData {}

public struct MustGrabIngredientComponent : IComponentData, IEnableableComponent {}