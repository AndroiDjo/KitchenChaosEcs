using Unity.Entities;
using UnityEngine;

partial struct CleanupEntitySystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (transformGO, entity) in SystemAPI
                     .Query<GameObjectTransformComponent>()
                     .WithEntityAccess()
                     .WithNone<MustBeCleanedBeforeDestroyComponent>()
                 ) {
            if (transformGO.Transform.gameObject != null) {
                GameObject.Destroy(transformGO.Transform.gameObject);
            }
            ecb.RemoveComponent<GameObjectTransformComponent>(entity);
        }
    }
}