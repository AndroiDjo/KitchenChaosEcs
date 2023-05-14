using Helpers;
using Unity.Entities;
using Unity.Transforms;

partial class FryingSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .ForEach((Entity entity, ref FryCounterComponent fryCounter, ref ProgressBarValueComponent progressBar, 
                in IngredientNextStagePrefabComponent nextStagePrefab, in HoldedByComponent holdedBy,
                in Parent parentEntity, in LocalTransform localTransform) => {
                
                if (holdedBy.HolderType != HolderType.StoveCounter) {
                    return;
                }
        
                fryCounter.Counter += fryCounter.Speed * dt;
                progressBar.Value = fryCounter.Counter / fryCounter.Goal;
                if (fryCounter.Counter >= fryCounter.Goal) {
                    ecb.AddComponent<MustBeDestroyedComponent>(entity);
                    ecb.SetEnabled(entity, false);
                    Entity nextStageEntity = ecb.Instantiate(nextStagePrefab.Prefab);
                    EntitySystemHelper.SetNewParentToIngredient(ref ecb, nextStageEntity, new ItemPlaceholderComponent {
                        Entity = parentEntity.Value,
                        LocalPosition = localTransform
                    }, true);
                }
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
