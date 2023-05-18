using UnityEngine;
using Unity.Entities;

public class CanDeliverMealsComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanDeliverMealsComponentAuthoring> {
        public override void Bake(CanDeliverMealsComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanDeliverMealsComponent>(entity);
        }
    }
}

public struct CanDeliverMealsComponent : IComponentData {}