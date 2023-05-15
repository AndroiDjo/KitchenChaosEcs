using Helpers;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(SelectedItemInteractSystem))]
partial class CuttingSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .WithAll<TryToCutIngredientComponent>()
            .ForEach((Entity entity, ref CutCounterComponent cutCounter, ref ProgressBarValueComponent progressBar,
                in IngredientNextStagePrefabComponent nextStagePrefab,
                in Parent parentEntity, in LocalTransform localTransform) => {
                
                ecb.SetComponentEnabled<TryToCutIngredientComponent>(entity, false);
                cutCounter.Counter++;
                progressBar.Value = (float)cutCounter.Counter / cutCounter.Goal;
                if (cutCounter.Counter >= cutCounter.Goal) {
                    ecb.DestroyEntity(entity);
                    Entity nextStageEntity = ecb.Instantiate(nextStagePrefab.Prefab);
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, nextStageEntity, new ItemPlaceholderComponent {
                        Entity = parentEntity.Value,
                        LocalPosition = localTransform
                    }, true);
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}