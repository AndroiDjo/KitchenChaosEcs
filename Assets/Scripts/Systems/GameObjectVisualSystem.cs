using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
partial struct GameObjectVisualSystem : ISystem {
  public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (visual, entity) in SystemAPI.Query<GameObjectVisualComponent>().WithEntityAccess()) {
                var go = GameObject.Instantiate(visual.Prefab);
                ecb.AddComponent(entity, new GameObjectTransformComponent{Transform = go.transform});
                if (go.TryGetComponent<Animator>(out Animator animator)) {
                    ecb.AddComponent(entity, new GameObjectAnimatorComponent{ Animator = animator });
                }
                if (go.TryGetComponent<ProgressBarImageHolder>(out ProgressBarImageHolder progressBarImageHolder)) {
                    var progressBarImage = progressBarImageHolder.GetImageHolder();
                    
                    if (progressBarImage != null) {
                        ecb.AddComponent(entity, new GameObjectProgressBarComponent {
                            ProgressBarGO = go,
                            Image = progressBarImage
                        });
                    }
                }
                ecb.RemoveComponent<GameObjectVisualComponent>(entity);
        }
  }
}