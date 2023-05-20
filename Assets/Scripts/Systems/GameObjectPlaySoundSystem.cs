using Unity.Entities;
using Unity.Transforms;

partial struct GameObjectPlaySoundSystem : ISystem {
    
    public void OnUpdate(ref SystemState state) {
        
        foreach (var (localTransform, entity) in SystemAPI
                     .Query<LocalTransform>()
                     .WithEntityAccess()
                     .WithAll<IsCuttingSoundComponent>()) {
            SystemAPI.SetComponentEnabled<IsCuttingSoundComponent>(entity, false);
            SoundsManager.Instance.PlayCuttingSound(localTransform.Position);
        }
        
        foreach (var (localTransform, entity) in SystemAPI
                     .Query<LocalTransform>()
                     .WithEntityAccess()
                     .WithAll<IsTrashSoundComponent>()) {
            SystemAPI.SetComponentEnabled<IsTrashSoundComponent>(entity, false);
            SoundsManager.Instance.PlayTrashSound(localTransform.Position);
        }
        
        foreach (var (localTransform, entity) in SystemAPI
                     .Query<LocalTransform>()
                     .WithEntityAccess()
                     .WithAll<IsDeliverSuccessSoundComponent>()) {
            SystemAPI.SetComponentEnabled<IsDeliverSuccessSoundComponent>(entity, false);
            SoundsManager.Instance.PlayDeliverySuccessSound(localTransform.Position);
        }
        
        foreach (var (localTransform, entity) in SystemAPI
                     .Query<LocalTransform>()
                     .WithEntityAccess()
                     .WithAll<IsDeliverFailSoundComponent>()) {
            SystemAPI.SetComponentEnabled<IsDeliverFailSoundComponent>(entity, false);
            SoundsManager.Instance.PlayDeliveryFailSound(localTransform.Position);
        }
        
        foreach (var (localTransform, entity) in SystemAPI
                     .Query<LocalTransform>()
                     .WithEntityAccess()
                     .WithAll<MustPlayPickupSoundComponent>()) {
            SystemAPI.SetComponentEnabled<MustPlayPickupSoundComponent>(entity, false);
            SoundsManager.Instance.PlayPickupSound(localTransform.Position);
        }
        
        foreach (var (localTransform, entity) in SystemAPI
                     .Query<LocalTransform>()
                     .WithEntityAccess()
                     .WithAll<MustPlayDropSoundComponent>()) {
            SystemAPI.SetComponentEnabled<MustPlayDropSoundComponent>(entity, false);
            SoundsManager.Instance.PlayDropSound(localTransform.Position);
        }
        
        foreach (var (localTransform, entity) in SystemAPI
                     .Query<LocalTransform>()
                     .WithEntityAccess()
                     .WithAll<PlayerIsWalkingSoundComponent>()) {
            SystemAPI.SetComponentEnabled<PlayerIsWalkingSoundComponent>(entity, false);
            SoundsManager.Instance.PlayStepSound(localTransform.Position);
        }
        
    }
}