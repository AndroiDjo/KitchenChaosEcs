using Unity.Entities;

partial class DeliveryEventsSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<IsDeliverySuccessful>()
            .ForEach((Entity entity) => {
                ecb.SetComponentEnabled<IsDeliverSuccessSoundComponent>(entity, true);
                ecb.SetComponentEnabled<NeedDrawSuccessPopup>(entity, true);
                ecb.SetComponentEnabled<IsDeliverySuccessful>(entity, false);
            }).Schedule();
        
        Entities
            .WithAll<IsDeliveryFailed>()
            .ForEach((Entity entity) => {
                ecb.SetComponentEnabled<IsDeliverFailSoundComponent>(entity, true);
                ecb.SetComponentEnabled<NeedDrawFailPopup>(entity, true);
                ecb.SetComponentEnabled<IsDeliveryFailed>(entity, false);
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}

partial class DeliveryUISystem : SystemBase {
    protected override void OnUpdate() {
        
        foreach (var (deliveryPopup, entity) in SystemAPI
                     .Query<GameObjectDeliveryPopupUIComponent>()
                     .WithEntityAccess()
                     .WithAll<NeedDrawSuccessPopup>()
                 ) {
            deliveryPopup.DeliveryPopupUI.DeliverySuccess();
            SystemAPI.SetComponentEnabled<NeedDrawSuccessPopup>(entity, false);
        }
        
        foreach (var (deliveryPopup, entity) in SystemAPI
                     .Query<GameObjectDeliveryPopupUIComponent>()
                     .WithEntityAccess()
                     .WithAll<NeedDrawFailPopup>()
                ) {
            deliveryPopup.DeliveryPopupUI.DeliveryFailed();
            SystemAPI.SetComponentEnabled<NeedDrawFailPopup>(entity, false);
        }
    }
}