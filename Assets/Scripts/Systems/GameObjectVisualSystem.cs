using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
partial struct GameObjectVisualSystem : ISystem {
    [BurstCompile]
    public void OnCreate(ref SystemState state) {
        EntityQueryBuilder qb = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<GameObjectVisualComponent>();
        var visualsQuery = state.GetEntityQuery(qb);
        state.RequireForUpdate(visualsQuery);
    }
    
    public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (visual, entity) in SystemAPI.Query<GameObjectVisualComponent>().WithEntityAccess()) {
                var go = GameObject.Instantiate(visual.Prefab);
                ecb.AddComponent(entity, new GameObjectTransformComponent{Transform = go.transform});
                Animator animator = go.GetComponent<Animator>();
                if (animator != null) {
                    ecb.AddComponent(entity, new GameObjectAnimatorComponent{ Animator = animator });
                }
                ecb.RemoveComponent<GameObjectVisualComponent>(entity);
        }
    }
}