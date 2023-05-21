using UnityEngine;
using Unity.Entities;

public class CanDeliverMealsComponentAuthoring : MonoBehaviour {
    class Baker : Baker<CanDeliverMealsComponentAuthoring> {
        public override void Bake(CanDeliverMealsComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanDeliverMealsComponent>(entity);
            AddComponent<IsDeliverSuccessSoundComponent>(entity);
            SetComponentEnabled<IsDeliverSuccessSoundComponent>(entity, false);
            AddComponent<IsDeliverFailSoundComponent>(entity);
            SetComponentEnabled<IsDeliverFailSoundComponent>(entity, false);
        }
    }
}

public struct CanDeliverMealsComponent : IComponentData {
    public int SuccessOrders;
}

public struct IsDeliverSuccessSoundComponent : IComponentData, IEnableableComponent {}
public struct IsDeliverFailSoundComponent : IComponentData, IEnableableComponent {}