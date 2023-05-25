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
            
            AddComponent<IsDeliverySuccessful>(entity);
            SetComponentEnabled<IsDeliverySuccessful>(entity, false);
            AddComponent<IsDeliveryFailed>(entity);
            SetComponentEnabled<IsDeliveryFailed>(entity, false);
            
            AddComponent<NeedDrawSuccessPopup>(entity);
            SetComponentEnabled<NeedDrawSuccessPopup>(entity, false);
            AddComponent<NeedDrawFailPopup>(entity);
            SetComponentEnabled<NeedDrawFailPopup>(entity, false);
        }
    }
}

public struct CanDeliverMealsComponent : IComponentData {
    public int SuccessOrders;
}

public struct IsDeliverySuccessful : IComponentData, IEnableableComponent {}
public struct IsDeliveryFailed : IComponentData, IEnableableComponent {}

public struct IsDeliverSuccessSoundComponent : IComponentData, IEnableableComponent {}
public struct IsDeliverFailSoundComponent : IComponentData, IEnableableComponent {}

public struct NeedDrawSuccessPopup : IComponentData, IEnableableComponent {}
public struct NeedDrawFailPopup : IComponentData, IEnableableComponent {}