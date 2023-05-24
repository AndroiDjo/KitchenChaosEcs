using Unity.Entities;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
partial struct GameObjectVisualSystem : ISystem {
  public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (visual, entity) in SystemAPI.Query<GameObjectVisualComponent>().WithEntityAccess()) {
            GameObject go = GameObject.Instantiate(visual.Prefab);
            ecb.AddComponent<MustBeCleanedBeforeDestroyComponent>(entity);
            ecb.AddComponent(entity, new GameObjectTransformComponent{Transform = go.transform});
        
            if (go.TryGetComponent(out Animator animator)) {
                ecb.AddComponent(entity, new GameObjectAnimatorComponent{ Animator = animator });
            }

            if (go.TryGetComponent(out AudioSourceHolder audioSource)) {
                ecb.AddComponent(entity, new GameObjectAudioSourceComponent{ AudioSourceHolder = audioSource });
            }
            
            if (go.TryGetComponent(out WarningSignUI warningSignUI)) {
                ecb.AddComponent(entity, new GameObjectWarningSignUIComponent{ WarningSignUI = warningSignUI });
            }
            
            if (go.TryGetComponent(out ProgressBarImageHolder progressBarImageHolder)) {
                var progressBarImage = progressBarImageHolder.GetImageHolder();
                
                if (progressBarImage != null) {
                    ecb.AddComponent(entity, new GameObjectProgressBarComponent {
                        ProgressBarGO = go,
                        Image = progressBarImage
                    });
                }
            }

            if (go.TryGetComponent(out IngredientIconsUI ingredientIconsUI)) {
                ecb.AddComponent(entity, new GameObjectIngredientIconsUIComponent {
                    IngredientIconsUI = ingredientIconsUI
                });
                ecb.AddComponent<NeedUpdateUIComponent>(entity);
                ecb.SetComponentEnabled<NeedUpdateUIComponent>(entity, false);
            }

            ecb.RemoveComponent<GameObjectVisualComponent>(entity);
        }
  }
}